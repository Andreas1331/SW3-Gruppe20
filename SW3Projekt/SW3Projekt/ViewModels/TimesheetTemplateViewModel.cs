using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.ViewModels
{
    public class TimesheetTemplateViewModel : Conductor<object>
    {
        private Timesheet _timesheet;

        public Timesheet Timesheet
        {
            get { return _timesheet; }
            set { _timesheet = value; }
        }

        public TimesheetTemplateViewModel()
        {
            Timesheet = new Timesheet();
        }


        public BindableCollection<TimesheetEntryViewModel> ThursdayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();

        public void BtnThursdayAddEntry()
        {
            ThursdayEntries.Add(new TimesheetEntryViewModel(Timesheet));
        }
        //public void RemoveEntry(object I) {
        //    ThursdayEntries.Remove(I);


        //}

        public void BtnBeregn()
        {
            // new TimesheetTemplateConfirmViewModel(Timesheet, og alle timesheet entries);
        }
    }
}
