using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Caliburn.Micro;


namespace SW3Projekt.ViewModels
{
    public class TimesheetEntryViewModel : Screen
    {
        public TimesheetEntry TimesheetEntry { get; set; }

        public TimesheetTemplateViewModel TSTemplateModel { get; set; }

        public Timesheet Timesheet { get; set; }

        private string _hourstextboxstring { get; set; }
        public string HoursTextBox
        {
            get
            {
                return _hourstextboxstring;
            }
            set
            {
                _hourstextboxstring = value;
                NotifyOfPropertyChange(() => HoursTextBox);
            }
        }

        public int StartTimeBox
        {
            get
            {
                return TimesheetEntry.StartTime;
            }
            set
            {
                TimesheetEntry.StartTime = value;
                NotifyOfPropertyChange(() => StartTimeBox);
                UpdateHoursTextbox();
            }
        }

        public int EndTimeBox
        {
            get
            {
                return TimesheetEntry.EndTime;
            }
            set
            {
                TimesheetEntry.EndTime = value;
                NotifyOfPropertyChange(() => EndTimeBox);
                UpdateHoursTextbox();
            }
        }
        public float BreakTimeBox
        {
            get
            {
                return TimesheetEntry.BreakTime;
            }
            set
            {
                TimesheetEntry.BreakTime = value;
                NotifyOfPropertyChange(() => BreakTimeBox);
                UpdateHoursTextbox();
            }
        }



        public TimesheetEntryViewModel(TimesheetTemplateViewModel timesheetViewModel)
        {
            TimesheetEntry = new TimesheetEntry();
            TSTemplateModel = timesheetViewModel;
            TimesheetEntry.timesheet = TSTemplateModel.Timesheet;
            UpdateHoursTextbox();
        }

        public void BtnRemoveEntry()
        {
            TSTemplateModel.RemoveEntry(this);
        }

        private void UpdateHoursTextbox()
        {
            //the calculation for hours:
            float numberOfWholeHours = (float)(Math.Floor((double)EndTimeBox / 100) - Math.Ceiling((double)StartTimeBox / 100));

            //the  calculations for minutes:
            float numberOfMinutes = (60 - (StartTimeBox % 100 == 0 ? 60 : StartTimeBox % 100) + EndTimeBox % 100) * (5 / 3) / (float)100;

            HoursTextBox = (numberOfMinutes + numberOfWholeHours - BreakTimeBox).ToString();
        }


    }
}
