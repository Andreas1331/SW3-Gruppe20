using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SW3Projekt.ViewModels
{
    public class TimesheetTemplateViewModel : Conductor<object>
    {
        private Timesheet _timesheet;
        private TimesheetTemplateViewModel _timesheetTemplateViewmodel;
        public TimesheetTemplateViewModel TimesheetViewModel {
            get { return _timesheetTemplateViewmodel; }
            set { _timesheetTemplateViewmodel = value; }
        }

        public Timesheet Timesheet
        {
            get { return _timesheet; }
            set { _timesheet = value; }
        }

        public TimesheetTemplateViewModel()
        {
            Timesheet = new Timesheet();
            TimesheetViewModel = this;
        }
       

        public BindableCollection<TimesheetEntryViewModel> MondayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();
        public BindableCollection<TimesheetEntryViewModel> TuesdayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();
        public BindableCollection<TimesheetEntryViewModel> WednesdayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();
        public BindableCollection<TimesheetEntryViewModel> ThursdayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();
        public BindableCollection<TimesheetEntryViewModel> FridayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();
        public BindableCollection<TimesheetEntryViewModel> SaturdayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();
        public BindableCollection<TimesheetEntryViewModel> SundayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();



        public void BtnMondayAddEntry()
        {
            MondayEntries.Add(new TimesheetEntryViewModel(TimesheetViewModel));
        }

        public void BtnTuesdayAddEntry()
        {
            TuesdayEntries.Add(new TimesheetEntryViewModel(TimesheetViewModel));
        }

        public void BtnWednesdayAddEntry()
        {
            WednesdayEntries.Add(new TimesheetEntryViewModel(TimesheetViewModel));
        }

        public void BtnThursdayAddEntry()
        {
            ThursdayEntries.Add(new TimesheetEntryViewModel(TimesheetViewModel));
        }

        public void BtnFridayAddEntry()
        {
            FridayEntries.Add(new TimesheetEntryViewModel(TimesheetViewModel));
        }

        public void BtnSaturdayAddEntry()
        {
            SaturdayEntries.Add(new TimesheetEntryViewModel(TimesheetViewModel));
        }

        public void BtnSundayAddEntry()
        {
            SundayEntries.Add(new TimesheetEntryViewModel(TimesheetViewModel));
        }

        public void RemoveEntry(TimesheetEntryViewModel entry) {
            if (MondayEntries.Contains(entry))
            {
                MondayEntries.Remove(entry);
            }
            else if (TuesdayEntries.Contains(entry))
            {
                TuesdayEntries.Remove(entry);
            }
            else if (WednesdayEntries.Contains(entry))
            {
                WednesdayEntries.Remove(entry);
            }
            else if (ThursdayEntries.Contains(entry))
            {
                ThursdayEntries.Remove(entry);
            }
            else if (FridayEntries.Contains(entry))
            {
                FridayEntries.Remove(entry);
            }
            else if (SaturdayEntries.Contains(entry))
            {
                SaturdayEntries.Remove(entry);
            }
            else if (SundayEntries.Contains(entry))
            {
                SundayEntries.Remove(entry);
            }
        }

        public void TestKnap() {
            GetRates();
        }
        public List<Rate> GetRates()
        {
            List<Rate> returnList = new List<Rate>();

            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {

                var activeAgreement = ctx.CollectiveAgreements.FirstOrDefault(agreement => agreement.IsActive);
                returnList = ctx.Rates.Where(rate => rate.CollectiveAgreementID == activeAgreement.Id).ToList();
            }
            return returnList;
        }




        public void BtnBeregn()
        {
            // new TimesheetTemplateConfirmViewModel(Timesheet, og alle timesheet entries);
     
        }
        





    }
}
