using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        public ShellViewModel()
        {
            ActivateItem(new HomeViewModel());
        }

        public void BtnHome()
        {
            ActivateItem(new HomeViewModel());
        }
        public void BtnNewTimesheet()
        {
            ActivateItem(new TimesheetTemplateViewModel());
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
    }
}
