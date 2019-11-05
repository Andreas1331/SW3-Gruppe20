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
using SW3Projekt.Tools;
using System.Windows.Forms;

namespace SW3Projekt.ViewModels
{
    public class TimesheetTemplateViewModel : Conductor<object>
    {
        public ShellViewModel ShellViewModel { get; set; }

        public Timesheet Timesheet { get; set; }

        public TimesheetTemplateViewModel(ShellViewModel shellViewModel)
        {
            Timesheet = new Timesheet();
            ShellViewModel = shellViewModel;
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
            MondayEntries.Add(new TimesheetEntryViewModel(this));
        }

        public void BtnTuesdayAddEntry()
        {
            TuesdayEntries.Add(new TimesheetEntryViewModel(this));
        }

        public void BtnWednesdayAddEntry()
        {
            WednesdayEntries.Add(new TimesheetEntryViewModel(this));
        }

        public void BtnThursdayAddEntry()
        {
            ThursdayEntries.Add(new TimesheetEntryViewModel(this));
        }

        public void BtnFridayAddEntry()
        {
            FridayEntries.Add(new TimesheetEntryViewModel(this));
        }

        public void BtnSaturdayAddEntry()
        {
            SaturdayEntries.Add(new TimesheetEntryViewModel(this));
        }

        public void BtnSundayAddEntry()
        {
            SundayEntries.Add(new TimesheetEntryViewModel(this));
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
            WeekEntries.Clear();
            WeekEntries.Add(MondayEntries);
            WeekEntries.Add(TuesdayEntries);
            WeekEntries.Add(WednesdayEntries);
            WeekEntries.Add(ThursdayEntries);
            WeekEntries.Add(FridayEntries);
            WeekEntries.Add(SaturdayEntries);
            WeekEntries.Add(SundayEntries);
            addTimesheetEntriesToList();
            Calculator.AddVismaEntries(Timesheet);
            ActivateItem(new TimesheetConfirmationViewModel(this));
            //Ske lige her
            // new TimesheetTemplateConfirmViewModel(Timesheet, og alle timesheet entries);
            
        }

        public void addTimesheetEntriesToList() {
            int i = 0;

            foreach (BindableCollection<TimesheetEntryViewModel> day in WeekEntries)
            {
                foreach (TimesheetEntryViewModel tsentry in day)
                {
                    tsentry.TimesheetEntry.EmployeeID = Timesheet.EmployeeID;
                    tsentry.TimesheetEntry.Date = GetDate(i);
                    if (!Timesheet.TSEntries.Contains(tsentry.TimesheetEntry))
                        Timesheet.TSEntries.Add(tsentry.TimesheetEntry); 
                }
                i++;
            }
        }

        public DateTime GetDate(int daysToAdd)
        {
            DateTime jan1 = new DateTime(Timesheet.Year, 1, 1);
            int daysOffsetThursday = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffsetThursday);

            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            int weeknumber = Timesheet.WeekNumber;

            if (firstWeek == 1)
            {
                weeknumber -= 1;
            }

            return firstThursday.AddDays((weeknumber * 7) - 3 + daysToAdd);
        }

    }
}
