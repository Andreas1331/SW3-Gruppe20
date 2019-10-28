using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SW3Projekt.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        public ShellViewModel()
        {
            ActivateItem(new HomeViewModel());
            CreateSomeDemoShitEmployees();
        }

        #region Navigation Methods
        public void BtnHome()
        {
            ActivateItem(new HomeViewModel());
        }

        public void BtnNewTimesheet()
        {
            ActivateItem(new TimesheetTemplateViewModel(this));
        }

        public void BtnOverview()
        {
            ActivateItem(new OverviewViewModel());
        }

        public void BtnWorkplaces()
        {
            ActivateItem(new WorkplacesViewModel());
        }

        public void BtnEmployees()
        {
            ActivateItem(new EmployeesViewModel());
        }
        public void BtnExport()
        {
            ActivateItem(new ExportViewModel());
        }
        public void BtnNotifications()
        {
            ActivateItem(new NotificationsViewModel());
        }
        public void BtnAgreements()
        {
            ActivateItem(new AgreementsViewModel());
        }
        public void BtnSettings()
        {
            ActivateItem(new SettingsViewModel());
        }
        #endregion

        public void CreateSomeDemoShitEmployees()
        {
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                //Employee emp = new Employee()
                //{
                //    Id = 34,
                //    Firstname = "Lars",
                //    Surname = "Pedersen",
                //    PhoneNumber = "0045 32 23 23 43",
                //    Email = "Dinkelberg@sima.dk",
                //    DateHired = DateTime.Now
                //};

                //ctx.Employees.Add(emp);
                //ctx.SaveChanges();

                //List<Workplace> workplaces = new List<Workplace>();

                //Workplace wp1 = new Workplace()
                //{
                //    Name = "Nordjyllands Værket",
                //    Abbreviation = "NJV",
                //    Address = "Nordjyllandsvej 13, 9000 Aalborg",
                //    Archived = false
                //};

                //Workplace wp2 = new Workplace()
                //{
                //    Name = "Verdo Randers",
                //    Abbreviation = "VDO R",
                //    Address = "Randersvej 233, 8900 Randers",
                //    Archived = false
                //};

                //Workplace wp3 = new Workplace()
                //{
                //    Name = "Aalborg Universitet",
                //    Abbreviation = "AAU",
                //    Address = "Univej 42, 9220 Aalborg",
                //    Archived = false
                //};

                //workplaces.Add(wp1);
                //workplaces.Add(wp2);
                //workplaces.Add(wp3);

                //ctx.Workplaces.AddRange(workplaces);
                //ctx.SaveChanges();

                //Route route1 = new Route()
                //{
                //    EmployeeID = 34,
                //    WorkplaceID = 2,
                //    Distance = 100.2f,
                //    RateValue = 1.6f
                //};

                //ctx.Routes.Add(route1);
                //ctx.SaveChanges();


                //Employee emp = ctx.Employees.Where(x => x.Id == 34).Include(x => x.Routes).FirstOrDefault();
                //foreach (Route route in emp.Routes)
                //    Console.WriteLine("Route: " + route.Distance);


                //CollectiveAgreement ca = new CollectiveAgreement()
                //{
                //    Name = "Overenskomst for 2019 til 2021",
                //    Start = DateTime.Now.AddYears(-1),
                //    End = DateTime.Now.AddYears(1),
                //    IsActive = true
                //};
                //ctx.CollectiveAgreements.Add(ca);
                //ctx.SaveChanges();


                //Rate r1 = new Rate()
                //{
                //    Name = "Normal",
                //    VismaID = 1100,
                //    RateValue = 25,
                //    StartTime = DateTime.Now,
                //    EndTime = DateTime.Now,
                //    CollectiveAgreementID = 1
                //};

                //Rate r2 = new Rate()
                //{
                //    Name = "Tillæg 1. + 2. Time",
                //    VismaID = 1311,
                //    RateValue = 50,
                //    StartTime = DateTime.Now,
                //    EndTime = DateTime.Now,
                //    CollectiveAgreementID = 1
                //};

                //Rate r3 = new Rate()
                //{
                //    Name = "Tillæg 3. + 4. Time",
                //    VismaID = 1312,
                //    RateValue = 100,
                //    StartTime = DateTime.Now,
                //    EndTime = DateTime.Now,
                //    CollectiveAgreementID = 1
                //};

                //ctx.Rates.Add(r1);
                //ctx.Rates.Add(r2);
                //ctx.Rates.Add(r3);
                //ctx.SaveChanges();

            
            }
        }
    }
}
