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

        public TimesheetConfirmationViewModel(TimesheetTemplateViewModel timesheet)
        {
            Timesheet = timesheet;
        }


    }
}
