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

        public readonly List<string> TypesOfRatesList = new List<string>() { "Afspadsering (ind)", "Afspadsering (ud)", "Andet" , "Arbejde", "Barn syg", "Diæt",  "Ferie", "Feriefri", "Forskudttid", "Logi", "SH-dage", "Sygdom", "Hidden"};
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
    }
}
