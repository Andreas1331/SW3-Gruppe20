using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Caliburn.Micro;
using System.Collections.ObjectModel;
using SW3Projekt.Tools;

namespace SW3Projekt.ViewModels
{
    public class TimesheetEntryViewModel : Screen
    {
        #region Properties
        private const int ResetValue = 0;
        public ObservableCollection<ComboBoxItem> RouteNamesCombobox { get; set; } = new ObservableCollection<ComboBoxItem>();
        public ObservableCollection<ComboBoxItem> TypeNamesCombobox { get; set; } = new ObservableCollection<ComboBoxItem>();

        public TimesheetEntry TimesheetEntry { get; set; }

        public TimesheetTemplateViewModel TSTemplateModel { get; set; }

        private bool _isBoxesEnabled = true;
        public bool IsBoxesEnabled 
        { 
            get 
            {
                return _isBoxesEnabled;
            } 
            set 
            {
                _isBoxesEnabled = value;
                NotifyOfPropertyChange(() => IsBoxesEnabled);
            } 
        }
        public string ProjectID
        { 
            get 
            {
                return TimesheetEntry.ProjectID;
            } 
            set 
            {
                TimesheetEntry.ProjectID = value;
                NotifyOfPropertyChange(() => ProjectID);
            } 
        }
        private string _hourstextboxstring { get; set; }

        private ComboBoxItem _selectedTypeComboBoxItem;
        public ComboBoxItem SelectedTypeComboBoxItem 
        { 
            get 
            {
                return _selectedTypeComboBoxItem;
            }
            set 
            {
                _selectedTypeComboBoxItem = value;
                TimesheetEntry.SelectedTypeComboBoxItem = (string)_selectedTypeComboBoxItem.Content;
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
                if (EndTimePicker < value)
                {
                    EndTimePicker = new DateTime(1, 1, 1, 23, 59, 0);
                }
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
                if (value < StartTimePicker)
                    TimesheetEntry.EndTime = new DateTime(1, 1, 1, 23, 59, 0);
                else
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
                value = (float)Calculator.RoundToNearest25th(value);
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

        public double KrTextBox
        {
            get
            {
                return TimesheetEntry.KrTextBox;
            }
            set
            {
                TimesheetEntry.KrTextBox = value;
                NotifyOfPropertyChange(() => KrTextBox);
            }
        }
        #endregion


        public TimesheetEntryViewModel(TimesheetTemplateViewModel timesheetViewModel)
        {
            // Each instance aggregates a TimesheetEntry object and references its timesheet.
            TimesheetEntry = new TimesheetEntry();
            TSTemplateModel = timesheetViewModel;
            TimesheetEntry.timesheet = TSTemplateModel.Timesheet;

            // HoursTextBox is generated with default values on construction.
            UpdateHoursTextbox();

            foreach (string typeName in ShellViewModel.Singleton.TypesOfRatesList)
            {
                if (typeName != "Andet" && typeName != "Hidden")
                {
                    TypeNamesCombobox.Add(new ComboBoxItem() { Content = typeName });
                }
            }
            // Default selection for the type ComboBox is "Arbejde" (Work).
            SelectedTypeComboBoxItem = TypeNamesCombobox.Where(type => (string)type.Content == "Arbejde").First();

            // The ComboBox with employee routes is generated from the routes list on the TimesheetTemplateViewModel.
            TSTemplateModel.EmployeeRoutes.ForEach(route => RouteNamesCombobox.Add(new ComboBoxItem { Content = route.LinkedWorkplace.Abbreviation }));
            RouteNamesCombobox.Add(new ComboBoxItem() {Content = ""});
        }

        public void BtnRemoveEntry()
        {
            Console.WriteLine(TimesheetEntry.StartTime);
            Console.WriteLine(TimesheetEntry.EndTime);
            TSTemplateModel.RemoveEntry(this);
        }

        // HoursTextBox is updated by subtracting start time and break time from end time entered.
        private void UpdateHoursTextbox()
        {
            var timeInterval = EndTimePicker - StartTimePicker;
            double displayedValue = Calculator.RoundToNearest25th(timeInterval.TotalHours);

            HoursTextBox = displayedValue.ToString();
        }

        // When a route is selected on the combobox.
        public void OnSelected(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selecteditem = sender as ComboBoxItem;
            if ((string)selecteditem.Content != "")
            {

                // Find the employee route associated with this.
                Route route = TSTemplateModel.EmployeeRoutes
                                .Where(r => r.LinkedWorkplace.Abbreviation == (string)selecteditem.Content).FirstOrDefault();

                // The km textbox on the view is set to the routes associated value. 
                KrTextBox = route.Distance * route.RateValue;

                // Driverate is needed for the Calculator.
                TimesheetEntry.DriveRate = route.RateValue;

                // The new ComboBoxItem is set.
                TimesheetEntry.SelectedRouteComboBoxItem = (string)selecteditem.Content;

                TimesheetEntry.WorkplaceID = route.WorkplaceID;
            }
            else 
            {
                // If the blank route is chosen, the other fields will be set to 0 to prevent any accidental routes to be added.
                KrTextBox = ResetValue;
                TimesheetEntry.DriveRate = ResetValue;
                TimesheetEntry.WorkplaceID = ResetValue;
                TimesheetEntry.SelectedRouteComboBoxItem = (string)selecteditem.Content;
                TimesheetEntry.WorkplaceID = null;
            }
        }
        public void CheckSelectedType(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selecteditem = sender as ComboBoxItem;
            if ((string)selecteditem.Content == "Ferie" || (string)selecteditem.Content == "SH-dage")
                IsBoxesEnabled = false;
            else if ((string)selecteditem.Content == "Afspadsering (ind)" || (string)selecteditem.Content == "Afspadsering (ud)")
            {
                ProjectID = "1226";
            }
            else
                IsBoxesEnabled = true;
        }
    }
}