using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.ViewModels
{
    public class TimesheetConfirmationViewModel : Conductor<object>
    {
        public TimesheetTemplateViewModel Timesheet { get; set; }

        public BindableCollection<TimesheetEntryConfirmationViewModel> SundayEntries { get; set; } = new BindableCollection<TimesheetEntryConfirmationViewModel>();

        public TimesheetConfirmationViewModel(TimesheetTemplateViewModel timesheet)
        {
            Timesheet = timesheet;

            foreach (TimesheetEntryViewModel entry in Timesheet.SundayEntries) 
            {
                SundayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry));
            }

        }


    }
}
