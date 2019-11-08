using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Caliburn.Micro;
using System.Collections.ObjectModel;

namespace SW3Projekt.ViewModels
{
    public class TimesheetEntryViewModel : Screen
    {
        public ObservableCollection<ComboBoxItem> RouteNamesCombobox { get; set; } = new ObservableCollection<ComboBoxItem>();

        public TimesheetEntry TimesheetEntry { get; set; }

        public TimesheetTemplateViewModel TSTemplateModel { get; set; }

        public Timesheet Timesheet { get; set; }

        private string _hourstextboxstring { get; set; }

        public string SelectedTypeComboBoxItem 
        { 
            get 
            {
                return TimesheetEntry.SelectedTypeComboBoxItem;
            }
            set 
            {
                TimesheetEntry.SelectedTypeComboBoxItem = value; 
                NotifyOfPropertyChange(() => SelectedTypeComboBoxItem); 
            } 
        }


        // Setting the Timepickers or the BreakTimeBox will update the HoursTextBox.
        public DateTime StartTimePicker
        {
            get
            {
                return TimesheetEntry.StartTime;
            }
            set
            {
                TimesheetEntry.StartTime = value;
                NotifyOfPropertyChange(() => StartTimePicker);
                UpdateHoursTextbox();
            }
        }

        public DateTime EndTimePicker
        {
            get
            {
                return TimesheetEntry.EndTime;
            }
            set
            {
                TimesheetEntry.EndTime = value;
                NotifyOfPropertyChange(() => EndTimePicker);
                UpdateHoursTextbox();
            }
        }

        public float BreakTimeBox
        {
            get
            {
                return (float)TimesheetEntry.BreakTime;
            }
            set
            {
                TimesheetEntry.BreakTime = value;
                NotifyOfPropertyChange(() => BreakTimeBox);
                UpdateHoursTextbox();
            }
        }

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

        public double KmTextBox
        {
            get
            {
                return TimesheetEntry.KmTextBox;
            }
            set
            {
                TimesheetEntry.KmTextBox = value;
                NotifyOfPropertyChange(() => KmTextBox);
            }
        }



        public TimesheetEntryViewModel(TimesheetTemplateViewModel timesheetViewModel)
        {

            // Each instance aggregates a TimesheetEntry object and references its timesheet.
            TimesheetEntry = new TimesheetEntry();
            TSTemplateModel = timesheetViewModel;
            TimesheetEntry.timesheet = TSTemplateModel.Timesheet;

            // HoursTextBox is generated with default values on construction.
            UpdateHoursTextbox();

            // Default selection for the type ComboBox is "Work".
            SelectedTypeComboBoxItem = "Arbejde";

            // The ComboBox with employee routes is generated from the routes list on the TimesheetTemplateViewModel.
            TSTemplateModel.EmployeeRoutes.ForEach(route => RouteNamesCombobox.Add(new ComboBoxItem { Content = route.LinkedWorkplace.Abbreviation }));

        }

        public void BtnRemoveEntry()
        {
            Console.WriteLine(TimesheetEntry.StartTime);
            Console.WriteLine(TimesheetEntry.EndTime);
            TSTemplateModel.RemoveEntry(this);
        }

        // HoursTextBox is updated by subtracting start time and breaktime from endtime entered.
        private void UpdateHoursTextbox()
        {
            var timeInterval = EndTimePicker - StartTimePicker;

            HoursTextBox = (timeInterval.TotalHours - BreakTimeBox).ToString();
        }

        // When a route is selected on the combobox.
        public void OnSelected(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selecteditem = sender as ComboBoxItem;

            // Find the employee route associated with this.
            Route route = TSTemplateModel.EmployeeRoutes
                            .Where(r => r.LinkedWorkplace.Abbreviation == (string)selecteditem.Content).FirstOrDefault();

            // The km textbox on the view is set to the routes associated value. 
            KmTextBox = route.Distance;

            // Driverate is needed for the Calculator.
            TimesheetEntry.DriveRate = route.RateValue;

            // The new ComboBoxItem is set.
            TimesheetEntry.SelectedRouteComboBoxItem = (string)selecteditem.Content;
        }

    }
}
