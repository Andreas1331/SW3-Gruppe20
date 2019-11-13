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

        private string _pagetitle = "Ny Timeseddel";

        public string PageTitle 
        {
            get 
            { 
                return _pagetitle; 
            }
            set
            {
                _pagetitle = value;
                NotifyOfPropertyChange(() => PageTitle);
            }
        }

        public string EmployeeName;

        public int WeekTextBox 
        { 
            get { return Timesheet.WeekNumber; } 
            set 
            { 
                Timesheet.WeekNumber = Math.Min(53, Math.Abs(value)); 
                NotifyOfPropertyChange(() => WeekTextBox);
            } 
        }

        public int YearTextBox
        {
            get { return Timesheet.Year; }
            set 
            { 
                Timesheet.Year = Math.Abs(value);
                NotifyOfPropertyChange(() => YearTextBox);
            }
        }

        public List<Route> EmployeeRoutes { get; set; }

        #region Days lists
        public BindableCollection<TimesheetEntryViewModel> MondayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();
        public BindableCollection<TimesheetEntryViewModel> TuesdayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();
        public BindableCollection<TimesheetEntryViewModel> WednesdayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();
        public BindableCollection<TimesheetEntryViewModel> ThursdayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();
        public BindableCollection<TimesheetEntryViewModel> FridayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();
        public BindableCollection<TimesheetEntryViewModel> SaturdayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();
        public BindableCollection<TimesheetEntryViewModel> SundayEntries { get; set; } = new BindableCollection<TimesheetEntryViewModel>();
        #endregion

        public List<BindableCollection<TimesheetEntryViewModel>> WeekEntries { get; set; } = new List<BindableCollection<TimesheetEntryViewModel>>();


        // Aggregates a Timesheet object during the entry proces (not saved in the DB, however).
        public TimesheetTemplateViewModel(ShellViewModel shellViewModel)
        {
            Timesheet = new Timesheet();
            ShellViewModel = shellViewModel;
        }

        #region Buttons for adding entries
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
        #endregion


        #region Buttons for removing entries
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
        #endregion


        public void BtnBeregn()
        {

            // WeekEntries is cleared in order to prevent duplication across several navigations.
            WeekEntries.Clear();
            WeekEntries.Add(MondayEntries);
            WeekEntries.Add(TuesdayEntries);
            WeekEntries.Add(WednesdayEntries);
            WeekEntries.Add(ThursdayEntries);
            WeekEntries.Add(FridayEntries);
            WeekEntries.Add(SaturdayEntries);
            WeekEntries.Add(SundayEntries);

            // TimesheetEntries are added to the list on the Timesheet.
            AddTimesheetEntriesToList();

            //VismaEntries are added to the lists on the TimesheetEntries.
            Calculator.AddVismaEntries(Timesheet);

            ActivateItem(new TimesheetConfirmationViewModel(this));
        }

        public void AddTimesheetEntriesToList() 
        {
            // daysToAdd is used to get the correct offset date from the week number entered.
            int daysToAdd = 0;

            // Each TimesheetEntry in aggregated on a TimesheetEntryViewModel.
            foreach (BindableCollection<TimesheetEntryViewModel> day in WeekEntries)
            {
                foreach (TimesheetEntryViewModel tsentry in day)
                {
                    tsentry.TimesheetEntry.EmployeeID = Timesheet.EmployeeID;

                    // The correct date is fetched based on the value of daysToAdd.
                    tsentry.TimesheetEntry.Date = GetDate(daysToAdd);

                    // Only new entries across navigations are added.
                    if (!Timesheet.TSEntries.Contains(tsentry.TimesheetEntry))
                    {
                        Timesheet.TSEntries.Add(tsentry.TimesheetEntry);
                    }
                }
                daysToAdd++;
            }
        }

        /* The date of the TimeSheetEntries are derived from the first Thursday of the year. */
        public DateTime GetDate(int daysToAdd)
        {
            /* Important: January 1st is not neccesarily in week 1! */
            DateTime jan1 = new DateTime(Timesheet.Year, 1, 1);

            DateTime firstThursday = jan1;

            /* The first thursday after January 1st is in week 1 of the new year in the Gregorian Calender system. */
            while (firstThursday.DayOfWeek != DayOfWeek.Thursday)
            {
                firstThursday = firstThursday.AddDays(1);
            }

            /* Weeks to add is offset by one because weeks are added in the next statement from week 1 and not week "0". */
            int weeksToAdd = Timesheet.WeekNumber - 1;

            /* Adding weeksToAdd * 7 to the firstThursday gives the date of Thursday in the correct week.
               Subtracting 3 returns the monday of the week, and daysToAdd provides the correct offset. */
            return firstThursday.AddDays((weeksToAdd * 7) - 3 + daysToAdd);
        }

        public void BtnConfirmNumber()
        {
            // It is checked whether the Employee ID entered is in the database.
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                Employee employee = ctx.Employees.Where(emp => emp.Id == Timesheet.EmployeeID).FirstOrDefault();

                // If no such ID exists, an error message is shown.
                if (employee == null)
                {
                    string caption = "Ukendt lønnummer";
                    string message = "Lønnummer ikke fundet. Prøv igen.";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;

                    MessageBox.Show(message, caption, buttons);
                }
                // Else the title on the View is updated with the Employee name, and the Employee routes are retrived from the DB.
                else
                {
                    EmployeeName = employee.Fullname;
                    PageTitle += " - " + EmployeeName;
                    EmployeeRoutes = ctx.Routes
                                        .Include("LinkedWorkplace")
                                        .Where(route => route.EmployeeID == Timesheet.EmployeeID)
                                        .OrderBy(route => route.LinkedWorkplace.Abbreviation)
                                        .ToList();
                }
            }
        }
    }
}
