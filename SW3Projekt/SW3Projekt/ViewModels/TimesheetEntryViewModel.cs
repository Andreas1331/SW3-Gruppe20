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

        public TimesheetTemplateViewModel TSTempalteModel { get; set; }

        public Timesheet Timesheet { get; set; }



       

        public TimesheetEntryViewModel(TimesheetTemplateViewModel timesheetViewModel)
        {
            //Timesheet = timesheet;
            TimesheetEntry = new TimesheetEntry();
            TSTempalteModel = timesheetViewModel;
        }
        public void BtnRemoveEntry() {
            TSTempalteModel.RemoveEntry(this);
        }
        public string HoursTextBox { get; set; }
    }
}
