using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.ViewModels
{
    public class TimesheetEntryViewModel
    {
        private TimesheetEntry _timesheetEntry;
        public TimesheetEntry TimesheetEntry
        {
            get { return _timesheetEntry; }
            set { _timesheetEntry = value; }
        }

        private TimesheetTemplateViewModel _tSTemplateModel;
        public TimesheetTemplateViewModel TSTempalteModel {
            get { return _tSTemplateModel; }
            set { _tSTemplateModel = value; }
        }

        private Timesheet _timesheet;
        public Timesheet Timesheet
        {
            get { return _timesheet; }
            set { _timesheet = value; }
        }

        public TimesheetEntryViewModel(TimesheetTemplateViewModel timesheetViewModel)
        {
            //Timesheet = timesheet;
            TSTempalteModel = timesheetViewModel;
        }
        public void BtnRemoveEntry() {
            TSTempalteModel.RemoveEntry(this);
        }

    }
}
