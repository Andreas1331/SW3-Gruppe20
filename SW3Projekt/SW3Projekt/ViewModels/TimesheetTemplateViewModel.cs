using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Globalization;

namespace SW3Projekt.ViewModels
{
    public class TimesheetTemplateViewModel : Conductor<object>
    {
        private Timesheet _timesheet;
        private TimesheetTemplateViewModel _timesheetTemplateViewmodel;
        public TimesheetTemplateViewModel TimesheetViewModel {
            get { return _timesheetTemplateViewmodel; }
            set { _timesheetTemplateViewmodel = value; }
        }

        public Timesheet Timesheet
        {
            get { return _timesheet; }
            set { _timesheet = value; }
        }
        public string WeekTextbox { get; set; }
        public string YearTextbox { get; set; }
        public string SalaryIDTextbox { get; set; }


        public TimesheetTemplateViewModel()
        {
            Timesheet = new Timesheet();
            TimesheetViewModel = this;
        }


        public BindableCollection<TimesheetEntryViewModel> MondayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();
        public BindableCollection<TimesheetEntryViewModel> TuesdayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();
        public BindableCollection<TimesheetEntryViewModel> WednesdayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();
        public BindableCollection<TimesheetEntryViewModel> ThursdayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();
        public BindableCollection<TimesheetEntryViewModel> FridayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();
        public BindableCollection<TimesheetEntryViewModel> SaturdayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();
        public BindableCollection<TimesheetEntryViewModel> SundayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();

        public List<BindableCollection<TimesheetEntryViewModel>> WeekEntries { get; set; } = new List<BindableCollection<TimesheetEntryViewModel>>();


        public void BtnMondayAddEntry()
        {
            MondayEntries.Add(new TimesheetEntryViewModel(TimesheetViewModel));
        }

        public void BtnTuesdayAddEntry()
        {
            TuesdayEntries.Add(new TimesheetEntryViewModel(TimesheetViewModel));
        }

        public void BtnWednesdayAddEntry()
        {
            WednesdayEntries.Add(new TimesheetEntryViewModel(TimesheetViewModel));
        }

        public void BtnThursdayAddEntry()
        {
            ThursdayEntries.Add(new TimesheetEntryViewModel(TimesheetViewModel));
        }

        public void BtnFridayAddEntry()
        {
            FridayEntries.Add(new TimesheetEntryViewModel(TimesheetViewModel));
        }

        public void BtnSaturdayAddEntry()
        {
            SaturdayEntries.Add(new TimesheetEntryViewModel(TimesheetViewModel));
        }

        public void BtnSundayAddEntry()
        {
            SundayEntries.Add(new TimesheetEntryViewModel(TimesheetViewModel));
        }

        public void RemoveEntry(TimesheetEntryViewModel entry) {
            if (MondayEntries.Contains(entry))
            {
                MondayEntries.Remove(entry);
            }
            else if (TuesdayEntries.Contains(entry))
            {
                TuesdayEntries.Remove(entry);
            }
            else if (WednesdayEntries.Contains(entry))
            {
                WednesdayEntries.Remove(entry);
            }
            else if (ThursdayEntries.Contains(entry))
            {
                ThursdayEntries.Remove(entry);
            }
            else if (FridayEntries.Contains(entry))
            {
                FridayEntries.Remove(entry);
            }
            else if (SaturdayEntries.Contains(entry))
            {
                SaturdayEntries.Remove(entry);
            }
            else if (SundayEntries.Contains(entry))
            {
                SundayEntries.Remove(entry);
            }
        }





        public void BtnBeregn()
        {
            WeekEntries.Add(MondayEntries);
            WeekEntries.Add(TuesdayEntries);
            WeekEntries.Add(WednesdayEntries);
            WeekEntries.Add(ThursdayEntries);
            WeekEntries.Add(FridayEntries);
            WeekEntries.Add(SaturdayEntries);
            WeekEntries.Add(SundayEntries);
            addTimesheetEntriesToList();
            //Ske lige her
            // new TimesheetTemplateConfirmViewModel(Timesheet, og alle timesheet entries);

        }

        public void addTimesheetEntriesToList() {
            int i = 0;
            foreach (BindableCollection<TimesheetEntryViewModel> day in WeekEntries)
            {
                
                foreach (TimesheetEntryViewModel tsentry in day)
                {
                    tsentry.TimesheetEntry.EmployeeID = int.Parse(SalaryIDTextbox);
                    tsentry.TimesheetEntry.Date = GetDate(i);

                    tsentry.TimesheetEntry.Comment = tsentry.CommentTextBox;
                    tsentry.TimesheetEntry.ProjectID = tsentry.ProjectIDTextBox;
                    
                    tsentry.TimesheetEntry.StartTime = int.Parse(tsentry.StartTimeTextBox);
                    tsentry.TimesheetEntry.EndTime = int.Parse(tsentry.EndTimeTextBox);

                    tsentry.TimesheetEntry.BreakTime = int.Parse(tsentry.PauseTextBox);
                    tsentry.TimesheetEntry.SelectedRouteComboBoxItem = tsentry.SelectedRouteComboBoxItem;
                    tsentry.TimesheetEntry.KmTextBox = tsentry.KmTextBox;
                    tsentry.TimesheetEntry.SelectedTypeComboBoxItem = tsentry.SelectedTypeComboBoxItem;

                    tsentry.TimesheetEntry.DietTextBox = tsentry.DietTextBox;
                    tsentry.TimesheetEntry.SelectedDisplacedHoursComboBoxItem = tsentry.SelectedDisplacedHoursComboBoxItem;
                    tsentry.TimesheetEntry.ValueTextbox = tsentry.ValueTextbox;
                    tsentry.TimesheetEntry.SelectedMiscellaneousComboBoxItem = tsentry.SelectedMiscellaneousComboBoxItem;
                    tsentry.TimesheetEntry.ValueMiscellaneousTextBox = tsentry.ValueMiscellaneousTextBox;
                }
                i++;
            }
        }

        public DateTime GetDate(int daysToAdd)
        {
            DateTime jan1 = new DateTime(int.Parse(YearTextbox), 1, 1);
            int daysOffsetThursday = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffsetThursday);

            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            int weeknumber = int.Parse(WeekTextbox);

            if (firstWeek == 1)
            {
                weeknumber -= 1;
            }

            return firstThursday.AddDays((weeknumber * 7) - 3 + daysToAdd);
        }

    }
}
