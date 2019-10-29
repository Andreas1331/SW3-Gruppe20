using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SW3Projekt.ViewModels
{
    public class TimesheetEntryViewModel
    {
        public TimesheetEntry TimesheetEntry { get; set; }

        public TimesheetTemplateViewModel TSTemplateModel { get; set; }

        public Timesheet Timesheet { get; set; }
        public string HoursTextBox { get; set; }


        public TimesheetEntryViewModel(TimesheetTemplateViewModel timesheetViewModel)
        {
            TimesheetEntry = new TimesheetEntry();
            TSTemplateModel = timesheetViewModel;
            TimesheetEntry.timesheet = TSTemplateModel.Timesheet;
        }

        public void BtnRemoveEntry()
        {
            TSTemplateModel.RemoveEntry(this);
        }

    }
}
