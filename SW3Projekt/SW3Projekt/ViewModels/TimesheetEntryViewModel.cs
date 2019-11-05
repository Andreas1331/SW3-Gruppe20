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

        public ComboBoxItem selectedRoute { get; set; }


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
            TimesheetEntry = new TimesheetEntry();
            TSTemplateModel = timesheetViewModel;
            TimesheetEntry.timesheet = TSTemplateModel.Timesheet;
            UpdateHoursTextbox();
            //magic!
            SelectedTypeComboBoxItem = "Arbejde";

            TSTemplateModel.employeeRoutes.ForEach(route => RouteNamesCombobox.Add(new ComboBoxItem { Content = route.LinkedWorkplace.Abbreviation }));

        }

        public void BtnRemoveEntry()
        {
            Console.WriteLine(TimesheetEntry.StartTime);
            Console.WriteLine(TimesheetEntry.EndTime);
            TSTemplateModel.RemoveEntry(this);
        }

        private void UpdateHoursTextbox()
        {
            var timeInterval = EndTimePicker - StartTimePicker;

            HoursTextBox = (timeInterval.TotalHours - BreakTimeBox).ToString();
        }

        public void OnSelected(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selecteditem = sender as ComboBoxItem;

            Route route = TSTemplateModel.employeeRoutes
                            .Where(r => r.LinkedWorkplace.Abbreviation == (string)selecteditem.Content).FirstOrDefault();

            KmTextBox = route.Distance;
        }

    }
}
