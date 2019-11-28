using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.IO;
using SW3Projekt.Tools;

namespace SW3Projekt.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        private static ShellViewModel _singleton;
        public static ShellViewModel Singleton
        {
            get { return _singleton ?? new ShellViewModel(); }
            private set
            {
                _singleton = value;
            }
        }
        
        private System.Windows.Visibility _notificationsVisibility = System.Windows.Visibility.Visible;
        public System.Windows.Visibility NotificationsVisibility
        {
            get
            {
                return _notificationsVisibility;
            }
            set
            {
                _notificationsVisibility = value;
                NotifyOfPropertyChange(() => NotificationsVisibility);
            }
        }



        public List<DBNotification> DBNotifications = new List<DBNotification>();
        public readonly List<NotificationViewModel> NotificationList = new List<NotificationViewModel>();
        public ObservableCollection<NotificationViewModel> Notifications
        {
            get
            {
                if (NotificationList.Count == 0)
                {
                    NotificationsVisibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    NotificationsVisibility = System.Windows.Visibility.Visible;
                }
                return new ObservableCollection<NotificationViewModel>(NotificationList);
            }
        }

        public readonly List<string> TypesOfRatesList = new List<string>() { "Afspadsering", "Andet" , "Arbejde", "Barn syg", "Diæt",  "Ferie", "Feriefri", "Forskudttid", "Logi", "SH-dage", "Sygdom"};
        public ObservableCollection<string> TypesOfRates
        {
            get
            {
                return new ObservableCollection<string>(TypesOfRatesList);
            }
        }

        public ShellViewModel()
        {
            Singleton = this;
            //Initialize Messagebox Manager - so we can create custom buttons.
            MessageBoxManager.Register();
            ActivateItem(new HomeViewModel());
            //CreateSomeDemoShitEmployees();


            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                DBNotifications = ctx.Notifications.ToList();
            }
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            filePath += "\\SIMPayrollConstants.txt";
            if (!File.Exists(filePath)) 
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine("SixtyDayThreshold = 60");
                    sw.WriteLine("TwentyThousindThreshold = 20000");
                    sw.WriteLine("MLE-40-FRAV");
                    sw.WriteLine("MLE-40-LONA");
                }
            }
            else
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    int i = 0;
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        i++;
                        switch (i) {
                            case 1:
                                {
                                    CommonValuesRepository.SixtyDayThreshold = int.Parse(line.Replace("SixtyDayThreshold = ", ""));
                                    break;
                                }
                            case 2: 
                                {

                                    CommonValuesRepository.TwentyThousindThreshold = int.Parse(line.Replace("TwentyThousindThreshold = ", ""));
                                    break;
                                }
                            case 3:
                                {
                                    CommonValuesRepository.ColumnCSick = line;
                                    break;
                                }
                            case 4:
                                {
                                    CommonValuesRepository.ColumnCWork = line;
                                    break;
                                }
                        }
                    }
                }
            }
            BtnHome();
        }

        #region Design Properties
        public int ShellStackPanelWidth { get; set; } = 178;

        public string BtnBgColorDefault = "#212121";
        public string BtnBgColorSelected = "#1565c0";
        public string BtnBgHome { get; set; }
        public string BtnBgNewTs { get; set; }
        public string BtnBgOverview { get; set; }
        public string BtnBgWorkplaces { get; set; }
        public string BtnBgEmps { get; set; }
        public string BtnBgProjects { get; set; }
        public string BtnBgExport { get; set; }
        public string BtnBgNotits { get; set; }
        public string BtnBgAgreements { get; set; }
        public string BtnBgSettings { get; set; }
        public string BtnBgTerminate { get; set; }
        #endregion

        #region Design Methods

        public void SetAllBtnBgToDefault()
        {
            BtnBgHome = BtnBgColorDefault;
            BtnBgNewTs = BtnBgColorDefault;
            BtnBgOverview = BtnBgColorDefault;
            BtnBgWorkplaces = BtnBgColorDefault;
            BtnBgEmps = BtnBgColorDefault;
            BtnBgProjects = BtnBgColorDefault;
            BtnBgExport = BtnBgColorDefault;
            BtnBgNotits = BtnBgColorDefault;
            BtnBgAgreements = BtnBgColorDefault;
            BtnBgSettings = BtnBgColorDefault;
            BtnBgTerminate = BtnBgColorDefault;
        }

        public void UpdateAllBtnBgColors()
        {
            NotifyOfPropertyChange(() => BtnBgHome);
            NotifyOfPropertyChange(() => BtnBgNewTs);
            NotifyOfPropertyChange(() => BtnBgOverview);
            NotifyOfPropertyChange(() => BtnBgWorkplaces);
            NotifyOfPropertyChange(() => BtnBgEmps);
            NotifyOfPropertyChange(() => BtnBgProjects);
            NotifyOfPropertyChange(() => BtnBgExport);
            NotifyOfPropertyChange(() => BtnBgNotits);
            NotifyOfPropertyChange(() => BtnBgAgreements);
            NotifyOfPropertyChange(() => BtnBgSettings);
            NotifyOfPropertyChange(() => BtnBgTerminate);
        }

        #endregion
        
        #region Navigation Methods
        public void BtnHome()
        {
            SetAllBtnBgToDefault();
            BtnBgHome = BtnBgColorSelected;
            UpdateAllBtnBgColors();
            ActivateItem(new HomeViewModel());
        }

        public void BtnNewTimesheet()
        {
            SetAllBtnBgToDefault();
            BtnBgNewTs = BtnBgColorSelected;
            UpdateAllBtnBgColors();
            ActivateItem(new TimesheetTemplateViewModel(this));
        }

        public void BtnOverview()
        {
            SetAllBtnBgToDefault();
            BtnBgOverview = BtnBgColorSelected;
            UpdateAllBtnBgColors();
            ActivateItem(new OverviewViewModel());
        }

        public void BtnWorkplaces()
        {
            SetAllBtnBgToDefault();
            BtnBgWorkplaces = BtnBgColorSelected;
            UpdateAllBtnBgColors();
            ActivateItem(new WorkplacesViewModel());
        }

        public void BtnEmployees()
        {
            SetAllBtnBgToDefault();
            BtnBgEmps = BtnBgColorSelected;
            UpdateAllBtnBgColors();
            ActivateItem(new EmployeesViewModel());
        }
        public void BtnProjects()
        {
            SetAllBtnBgToDefault();
            BtnBgProjects = BtnBgColorSelected;
            UpdateAllBtnBgColors();
            ActivateItem(new ProjectsViewModel());
        }

        public void BtnExport()
        {
            SetAllBtnBgToDefault();
            BtnBgExport = BtnBgColorSelected;
            UpdateAllBtnBgColors();
            ActivateItem(new ExportViewModel());
        }

        public void BtnNotifications()
        {
            SetAllBtnBgToDefault();
            BtnBgNotits = BtnBgColorSelected;
            UpdateAllBtnBgColors();
            ActivateItem(new NotificationsViewModel());
        }

        public void BtnAgreements()
        {
            SetAllBtnBgToDefault();
            BtnBgAgreements = BtnBgColorSelected;
            UpdateAllBtnBgColors();
            ActivateItem(new AgreementsViewModel(this));
        }

        public void BtnSettings()
        {
            SetAllBtnBgToDefault();
            BtnBgSettings = BtnBgColorSelected;
            UpdateAllBtnBgColors();
            ActivateItem(new SettingsViewModel());
        }

        #endregion



        // Should be removed at some point
        //public void CreateSomeDemoShitEmployees()
        //{
        //    using (var ctx = new SW3Projekt.DatabaseDir.Database())
        //    {
        //        //Employee emp = new Employee()
        //        //{
        //        //    Id = 34,
        //        //    Firstname = "Lars",
        //        //    Surname = "Pedersen",
        //        //    PhoneNumber = "0045 32 23 23 43",
        //        //    Email = "Dinkelberg@sima.dk",
        //        //    DateHired = DateTime.Now
        //        //};

        //        //ctx.Employees.Add(emp);
        //        //ctx.SaveChanges();

        //        //List<Workplace> workplaces = new List<Workplace>();

        //        //Workplace wp1 = new Workplace()
        //        //{
        //        //    Name = "Nordjyllands Værket",
        //        //    Abbreviation = "NJV",
        //        //    Address = "Nordjyllandsvej 13, 9000 Aalborg",
        //        //    Archived = false
        //        //};

        //        //Workplace wp2 = new Workplace()
        //        //{
        //        //    Name = "Verdo Randers",
        //        //    Abbreviation = "VDO R",
        //        //    Address = "Randersvej 233, 8900 Randers",
        //        //    Archived = false
        //        //};

        //        //Workplace wp3 = new Workplace()
        //        //{
        //        //    Name = "Aalborg Universitet",
        //        //    Abbreviation = "AAU",
        //        //    Address = "Univej 42, 9220 Aalborg",
        //        //    Archived = false
        //        //};

        //        //workplaces.Add(wp1);
        //        //workplaces.Add(wp2);
        //        //workplaces.Add(wp3);

        //        //ctx.Workplaces.AddRange(workplaces);
        //        //ctx.SaveChanges();

        //        //Route route1 = new Route()
        //        //{
        //        //    EmployeeID = 34,
        //        //    WorkplaceID = 2,
        //        //    Distance = 100.2f,
        //        //    RateValue = 1.6f
        //        //};

        //        //ctx.Routes.Add(route1);
        //        //ctx.SaveChanges();


        //        //Employee emp = ctx.Employees.Where(x => x.Id == 34).Include(x => x.Routes).FirstOrDefault();
        //        //foreach (Route route in emp.Routes)
        //        //    Console.WriteLine("Route: " + route.Distance);


        //        //CollectiveAgreement ca = new CollectiveAgreement()
        //        //{
        //        //    Name = "Overenskomst for 2019 til 2021",
        //        //    Start = DateTime.Now.AddYears(-1),
        //        //    End = DateTime.Now.AddYears(1),
        //        //    IsActive = true
        //        //};
        //        //ctx.CollectiveAgreements.Add(ca);
        //        //ctx.SaveChanges();


        //        //Rate r1 = new Rate()
        //        //{
        //        //    Name = "Normal",
        //        //    VismaID = 1100,
        //        //    RateValue = 25,
        //        //    StartTime = DateTime.Now,
        //        //    EndTime = DateTime.Now,
        //        //    CollectiveAgreementID = 1
        //        //};

        //        //Rate r2 = new Rate()
        //        //{
        //        //    Name = "Tillæg 1. + 2. Time",
        //        //    VismaID = 1311,
        //        //    RateValue = 50,
        //        //    StartTime = DateTime.Now,
        //        //    EndTime = DateTime.Now,
        //        //    CollectiveAgreementID = 1
        //        //};

        //        //Rate r3 = new Rate()
        //        //{
        //        //    Name = "Tillæg 3. + 4. Time",
        //        //    VismaID = 1312,
        //        //    RateValue = 100,
        //        //    StartTime = DateTime.Now,
        //        //    EndTime = DateTime.Now,
        //        //    CollectiveAgreementID = 1
        //        //};

        //        //ctx.Rates.Add(r1);
        //        //ctx.Rates.Add(r2);
        //        //ctx.Rates.Add(r3);
        //        //ctx.SaveChanges();


        //    }
        //}
    }
}
