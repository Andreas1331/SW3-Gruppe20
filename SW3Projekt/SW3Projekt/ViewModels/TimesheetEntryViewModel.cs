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
            TimesheetEntry = new TimesheetEntry();
            TSTempalteModel = timesheetViewModel;
        }
        public void BtnRemoveEntry() {
            TSTempalteModel.RemoveEntry(this);
        }
        public string ProjectIDTextBox { get; set; }
        public string StartTimeTextBox { get; set; }
        public string EndTimeTextBox { get; set; }
        public string HoursTextBox { get; set; }
        public string PauseTextBox { get; set; }
        public ComboBoxItem SelectedRouteComboBoxItem { get; set; }
        public string KmTextBox { get; set; }
        public ComboBoxItem SelectedTypeComboBoxItem { get; set; }
        public string DietTextBox { get; set; }
        public ComboBoxItem SelectedDisplacedHoursComboBoxItem { get; set; }
        public string ValueTextbox { get; set; }
        public ComboBoxItem SelectedMiscellaneousComboBoxItem { get; set; }
        public string ValueMiscellaneousTextBox { get; set; }
        public string CommentTextBox { get; set; }
    }
}
