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
        private System.Windows.Visibility _confirmVisibility = System.Windows.Visibility.Visible;
        public System.Windows.Visibility ConfirmVisibility
        {
            get 
            {
                return _confirmVisibility;
            }
            set 
            {
                _confirmVisibility = value;
                NotifyOfPropertyChange (()=> ConfirmVisibility);
            }
        }
        private bool _confirmIsEnabled = true;
        public bool ConfirmIsEnabled 
        {
            get 
            {
                return _confirmIsEnabled;
            }
            set 
            {
                _confirmIsEnabled = value;
                NotifyOfPropertyChange(() => ConfirmIsEnabled);
            }
        }

        private System.Windows.Visibility _pageVisibility = System.Windows.Visibility.Hidden;
        public System.Windows.Visibility PageVisibility
        {
            get
            {
                return _pageVisibility;
            }
            set
            {
                _pageVisibility = value;
                NotifyOfPropertyChange(() => PageVisibility);
            }
        }
        private bool _pageIsEnabled;
        public bool PageIsEnabled
        {
            get
            {
                return _pageIsEnabled;
            }
            set
            {
                _pageIsEnabled = value;
                NotifyOfPropertyChange(() => PageIsEnabled);
            }
        }

        private bool _employeeIdReadOnly;
        public bool EmployeeIdReadOnly
        {
            get
            {
                return _employeeIdReadOnly;
            }
            set
            {
                _employeeIdReadOnly = value;
                NotifyOfPropertyChange(() => PageVisibility);
            }
        }

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


        // Aggregates a Timesheet object during the entry process (not saved in the DB, however).
        public TimesheetTemplateViewModel(ShellViewModel shellViewModel)
        {
            Timesheet = new Timesheet();
            ShellViewModel = shellViewModel;
        }

        // Constructor to ease unit testing.
        public TimesheetTemplateViewModel()
        {
            Timesheet = new Timesheet();
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


        #region Method for removing entries
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
            if (AnyMissingProjectIds())
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

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

            // If the total of normal hours exceed 35 hours it automatically adds the "Afspadsering (ind)" to the timesheet.
            if (Timesheet.TotalNormalHours > 35)
            {
                SundayEntries.Add(new TimesheetEntryViewModel(this));
                TimesheetEntryViewModel newEntry = SundayEntries.Last();
                newEntry.SelectedTypeComboBoxItem.Content = "Afspadsering (ind)";
                newEntry.ProjectID = "1226";
                newEntry.TimesheetEntry.EmployeeID = Timesheet.EmployeeID;
                newEntry.TimesheetEntry.Date = GetDate(6);
                Timesheet.TSEntries.Add(newEntry.TimesheetEntry);

                newEntry.TimesheetEntry.vismaEntries.Add(new VismaEntry
                {
                    VismaID = Timesheet.rates.FirstOrDefault(x => x.Name == "Afspadsering (ind)").VismaID,
                    Value = Timesheet.TotalNormalHours - 35,
                    TimesheetEntryID = newEntry.TimesheetEntry.Id,
                    LinkedRate = Timesheet.rates.FirstOrDefault(x => x.Name == "Afspadsering(ind)"),
                    RateID = Timesheet.rates.FirstOrDefault(x => x.Name == "Afspadsering (ind)").Id
                });
            }
            ShellViewModel.Singleton.ActivateItem(new TimesheetConfirmationViewModel(this));
        }

        private bool AnyMissingProjectIds()
        {
            string message = "Manglende projekt-ID ";
            string messagePart2 = " Vil du fortsætte?";
            string caption = "Manglende projekt-ID";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;

            if (CheckDayForMissingProjectIDs(MondayEntries))
            {
                DialogResult dialogResult = MessageBox.Show(message + "mandag." + messagePart2, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button2);

                return dialogResult == DialogResult.No;
            }
            else if (CheckDayForMissingProjectIDs(TuesdayEntries))
            {
                DialogResult dialogResult = MessageBox.Show(message + "tirsdag." + messagePart2, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button2);

                return dialogResult == DialogResult.No;
            }
            else if (CheckDayForMissingProjectIDs(WednesdayEntries))
            {
                DialogResult dialogResult = MessageBox.Show(message + "onsdag." + messagePart2, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button2);

                return dialogResult == DialogResult.No;
            }
            else if (CheckDayForMissingProjectIDs(ThursdayEntries))
            {
                DialogResult dialogResult = MessageBox.Show(message + "torsdag." + messagePart2, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button2);

                return dialogResult == DialogResult.No;
            }
            else if (CheckDayForMissingProjectIDs(FridayEntries))
            {
                DialogResult dialogResult = MessageBox.Show(message + "fredag." + messagePart2, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button2);

                return dialogResult == DialogResult.No;
            }
            else if (CheckDayForMissingProjectIDs(SaturdayEntries))
            {
                DialogResult dialogResult = MessageBox.Show(message + "lørdag." + messagePart2, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button2);

                return dialogResult == DialogResult.No;
            }
            else if (CheckDayForMissingProjectIDs(SundayEntries))
            {
                DialogResult dialogResult = MessageBox.Show(message + "søndag." + messagePart2, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button2);

                return dialogResult == DialogResult.No;
            }

            return false;
        }

        private bool CheckDayForMissingProjectIDs(BindableCollection<TimesheetEntryViewModel> dayEntries)
        {
            return dayEntries.Any(tsvm => String.IsNullOrWhiteSpace(tsvm.TimesheetEntry.ProjectID));
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

        // The date of the TimeSheetEntries are derived from the first Thursday of the year. 
        public DateTime GetDate(int daysToAdd)
        {
            // Important: January 1st is not necessarily in week 1! 
            DateTime jan1 = new DateTime(Timesheet.Year, 1, 1);

            DateTime firstThursday = jan1;

            // The first Thursday after January 1st is in week 1 of the new year in the Gregorian Calender system. 
            while (firstThursday.DayOfWeek != DayOfWeek.Thursday)
            {
                firstThursday = firstThursday.AddDays(1);
            }

            // Weeks to add is offset by one because weeks are added in the next statement from week 1 and not week "0". 
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
                // Else the title on the View is updated with the Employee name, and the Employee routes are retrived from the database.
                else
                {
                    PageIsEnabled = true;
                    PageVisibility = System.Windows.Visibility.Visible;
                    EmployeeIdReadOnly = true;
                    ConfirmIsEnabled = false;
                    ConfirmVisibility = System.Windows.Visibility.Hidden;
                    EmployeeName = employee.Fullname;
                    PageTitle += " - " + EmployeeName;
                    EmployeeRoutes = ctx.Routes
                                        .Include("LinkedWorkplace")
                                        .Where(route => route.EmployeeID == Timesheet.EmployeeID && route.LinkedWorkplace.Archived == false)
                                        .OrderBy(route => route.LinkedWorkplace.Abbreviation)
                                        .ToList();
                }
            }
        }
    }
}
