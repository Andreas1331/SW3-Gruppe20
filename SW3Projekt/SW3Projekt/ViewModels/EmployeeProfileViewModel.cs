using Caliburn.Micro;
using SW3Projekt.DatabaseDir;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SW3Projekt.Tools;

namespace SW3Projekt.ViewModels
{
    public class EmployeeProfileViewModel : Screen
    {
        #region Properties
        //Design Prop
        public int cornerRadius { get; set; } = 0;

        // Selected employee is being parsed from the previous page,
        // when the user double clicks an entry in the table.
        public Employee SelectedEmployee { get; set; }

        // The new route is where the information the user adds is stored.
        private Route _newRoute;
        public Route NewRoute
        {
            get
            {
                return _newRoute;
            }
            set
            {
                _newRoute = value;
                NotifyOfPropertyChange(() => NewRoute);
            }
        }

        // Selected workplace is set when the user uses the combobox.
        private Workplace _selectedWorkplace;
        public Workplace SelectedWorkplace
        {
            get
            {
                return _selectedWorkplace;
            }
            set
            {
                _selectedWorkplace = value;
                NewRoute.WorkplaceID = (SelectedWorkplace != null) ? SelectedWorkplace.Id : 0;
                NewRoute.LinkedWorkplace = SelectedWorkplace;
            }
        }

        // Selected route is set when the user clicks on an element in the table over routes.
        private Route _selectedRoute;
        public Route SelectedRoute
        {
            get
            {
                return _selectedRoute;
            }
            set
            {
                _selectedRoute = value;
                NotifyOfPropertyChange(() => SelectedRoute);
                NotifyOfPropertyChange(() => CanBtnDeleteSelectedRoute);
            }
        }
        public bool CanBtnDeleteSelectedRoute
        {
            get
            {
                return SelectedRoute != null;
            }
        }

        // Workplaces used to display in the options on the combobox.
        private BindableCollection<Workplace> _workplaces;
        public BindableCollection<Workplace> Workplaces
        {
            get
            {
                return _workplaces;
            }
            set
            {
                _workplaces = value;
                NotifyOfPropertyChange(() => Workplaces);
            }
        }

        // Determines whatever the information fields are active or not.
        private bool _canEditEmployee = false;
        public bool CanEditEmployee
        {
            get
            {
                return _canEditEmployee;
            }
            set
            {
                _canEditEmployee = value;
                NotifyOfPropertyChange(() => CanEditEmployee);
            }
        }

        // The route collection is used to display all the employees unique routes.
        public BindableCollection<Route> RouteCollection
        {
            get
            {
                return new BindableCollection<Route>(SelectedEmployee.Routes);
            }
        }

        // Week and year used for displaying figuring out which timesheets to display
        private int _selectedWeek;
        private int _selectedYear;
        public int SelectedWeek
        {
            get
            {
                return _selectedWeek;
            }
            set
            {
                _selectedWeek = value;
                NotifyOfPropertyChange(() => SelectedWeek);
            }
        }
        public int SelectedYear
        {
            get
            {
                return _selectedYear;
            }
            set
            {
                _selectedYear = value;
                NotifyOfPropertyChange(() => SelectedYear);
            }
        }
        // All timesheetentries/vismaentries currently being shown in the table
        private BindableCollection<EntryFormatted> _entriesCollection;
        public BindableCollection<EntryFormatted> EntriesCollection
        {
            get
            {
                return _entriesCollection;
            }
            set
            {
                _entriesCollection = value;
                NotifyOfPropertyChange(() => EntriesCollection);
            }
        }

        private BindableCollection<ProjectFormat> _projectCollection;
        public BindableCollection<ProjectFormat> ProjectCollection {
            get {
                return _projectCollection;
            }
            set {
                _projectCollection = value;
                NotifyOfPropertyChange(() => ProjectCollection);
            }
        }

        private List<SixtyDayHolder> _sixtyDayHolders = new List<SixtyDayHolder>();
        public BindableCollection<SixtyDayHolder> SixtyDayCollection
        {
            get
            {
                return new BindableCollection<SixtyDayHolder>(_sixtyDayHolders);
            }
        }

        public double TotalHoursForThisYear
        {
            get
            {
                return GetTotalHours();
            }

        }

        public double AverageHoursPerWeek
        {
            get
            {
                return GetAverageHoursPerWeek();
            }
        }

        public double NumberOfSickHours
        {
            get
            {
                return GetNumberOfSickHours();
            }
        }

        public string TitleInformation
        {
            get
            {
                return $"{SelectedEmployee.Fullname} #{SelectedEmployee.Id} ({(SelectedEmployee.IsFired ? "Ikke ansat" : "Ansat")})";
            }
        }

        private double _stateRouteRate;
        public double StateRouteRate
        {
            get
            {
                return _stateRouteRate;
            }
            set
            {
                _stateRouteRate = value;
            }
        }
        #endregion

        public EmployeeProfileViewModel(Employee emp)
        {
            SelectedEmployee = emp;

            // Instantiate the new route and set the foreignkey value to the,
            // currently selected employee.
            NewRoute = new Route();
            NewRoute.EmployeeID = SelectedEmployee.Id;

            // Get all the workplaces from the database and store them.
            Task.Run(async () =>
            {
                var workplaces = await GetWorkplacesAsync();
                Workplaces = new BindableCollection<Workplace>(workplaces);
            });

            // Find the active collective agreement from the database, 
            // and pull the rate containing the rate for routes determined by the state.
            using (var ctx = new DatabaseDir.Database())
            {
                var routeRate = ctx.CollectiveAgreements.Include(r => r.Rates).FirstOrDefault(x => x.IsActive).Rates.FirstOrDefault(k => k.Type.Equals("Kørsel"));
                if (routeRate != null)
                {
                    NewRoute.RateValue = StateRouteRate = routeRate.RateValue;
                }
            }

            Task.Run(async () =>
            {
                _sixtyDayHolders = await GetSixtyDayDataAsync();
                NotifyOfPropertyChange(() => SixtyDayCollection);
            });
       
            // TODO 1: Get all TimesheetEntries and the projectID
            // TODO 2: Query for all VismaEntries linked to the TimesheetEntries (DONE)
            // TODO 3: Format all the data into a new bindablecollection to display on the table

            using (var ctx = new DatabaseDir.Database())
            {
                List<TimesheetEntry> entries = ctx.TimesheetEntries.Include(k => k.vismaEntries.Select(p => p.LinkedRate)).Where(x => x.EmployeeID == SelectedEmployee.Id).ToList();

                List<ProjectFormat> projectFormats = new List<ProjectFormat>();
                foreach (TimesheetEntry ts in entries)
                {
                    VismaEntry visma = ts.vismaEntries.FirstOrDefault(x => x.LinkedRate.Name == "Normal");
                    if (visma != null)
                    {
                        //ts.ProjectID;
                        //visma.Value;

                        ProjectFormat pf = projectFormats.FirstOrDefault(k => k.ProjectID == ts.ProjectID);
                        if (pf == null)
                        {
                            pf = new ProjectFormat(ts.ProjectID); 
                        }
                           
                        pf.Hours += visma.Value;

                        if (!projectFormats.Contains(pf))
                            projectFormats.Add(pf);
                    }
                }

                ProjectCollection = new BindableCollection<ProjectFormat>(projectFormats);

            }
        }

        #region Buttons
        public void BtnSearchForEntries()
        {
            // TODO 1: Get all TimesheetEntries based on selected week, year and employee (DONE)
            // TODO 2: Query for all VismaEntries linked to the TimesheetEntries (DONE)
            // TODO 3: Query for all Rates linked to the VismaEntries (DONE)
            // TODO 4: Format all the data into a new bindablecollection to display on the table
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            using (var ctx = new DatabaseDir.Database())
            {
                List<TimesheetEntry> entries = ctx.TimesheetEntries.Include(k => k.vismaEntries.Select(p => p.LinkedRate)).ToList().Where(x => (cal.GetWeekOfYear(x.Date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == SelectedWeek)
                                                && x.Date.Year == SelectedYear).ToList();

                List<EntryFormatted> entriesFormatted = new List<EntryFormatted>();
                foreach (TimesheetEntry ts in entries)
                {
                    //entriesFormatted.Add(new EntryFormatted(ts.StartTime.ToString("mm"), ts.EndTime.ToString("mm"), ;
                    foreach (VismaEntry visma in ts.vismaEntries)
                    {
                        entriesFormatted.Add(new EntryFormatted(
                            ts.Date.ToString("dd/MM/yyyy"),
                            ts.StartTime.ToString("HH:mm"),
                            ts.EndTime.ToString("HH:mm"),
                            visma.Value,
                            visma.LinkedRate.Name,
                            visma.LinkedRate.VismaID,
                            visma.Comment
                            ));
                    }
                }
                entriesFormatted = entriesFormatted.OrderBy(x => x.Date).ToList();
                EntriesCollection = new BindableCollection<EntryFormatted>(entriesFormatted);
            }

            //Console.WriteLine("{0:d}: Week {1} ({2})", date1,
            //                  cal.GetWeekOfYear(date1, dfi.CalendarWeekRule,
            //                                    dfi.FirstDayOfWeek),
            //                  cal.ToString().Substring(cal.ToString().LastIndexOf(".") + 1));
        }

        public void BtnEditEmployee()
        {
            CanEditEmployee = !CanEditEmployee;
        }

        public void BtnSaveEmployeeChanges()
        {
            using (var ctx = new DatabaseDir.Database())
            {
                ctx.Employees.Attach(SelectedEmployee);
                ctx.Entry(SelectedEmployee).State = EntityState.Modified;
                ctx.SaveChanges();
                new Notification(Notification.NotificationType.Edited, $"{SelectedEmployee.Fullname} er blevet opdateret i databasen.");
            }
        }

        public void BtnFireSelectedEmployee()
        {
            Task.Run(() =>
            {
                using (var ctx = new DatabaseDir.Database())
                {
                    // Flip the fired status of the employee and update the database
                    SelectedEmployee.IsFired = !SelectedEmployee.IsFired;
                    ctx.Employees.Attach(SelectedEmployee);
                    ctx.Entry(SelectedEmployee).State = EntityState.Modified;
                    ctx.SaveChanges();
                    NotifyOfPropertyChange(() => TitleInformation);
                    new Notification(Notification.NotificationType.Edited, $"{SelectedEmployee.Fullname} er blevet opdateret i databasen.");
                }
            });
        }

        public void BtnDeleteSelectedRoute()
        {
            Task.Run(() =>
            {
                using (var ctx = new DatabaseDir.Database())
                {
                    ctx.Routes.Attach(SelectedRoute);
                    ctx.Entry(SelectedRoute).State = EntityState.Deleted;
                    ctx.SaveChanges();
                    new Notification(Notification.NotificationType.Removed, "Den valgte rute er blevet fjernet fra databasen.");
                    SelectedEmployee.Routes.Remove(SelectedRoute);
                    SelectedRoute = null;
                    NotifyOfPropertyChange(() => RouteCollection);
                }
            });
        }

        public void BtnAddNewRoute()
        {
            Task.Run(() =>
            {
                // Make sure the entered rate value is valid compared to the distance
                double calculatedRate = (NewRoute.LinkedWorkplace.MaxPayout / NewRoute.Distance);
                bool isValid = (calculatedRate <= StateRouteRate);
                if (!isValid)
                {
                    new Notification(Notification.NotificationType.Error, $"Satsen {calculatedRate},- DKK/km overskrider statens takst {StateRouteRate},- DDK/km");
                    NewRoute.RateValue = StateRouteRate;
                }

                using (var ctx = new DatabaseDir.Database())
                {
                    ctx.Routes.Add(NewRoute);
                    ctx.Entry(NewRoute.LinkedWorkplace).State = EntityState.Detached;
                    ctx.SaveChanges();
                    // Reload the virtual property again
                    ctx.Entry(NewRoute).Reference(c => c.LinkedWorkplace).Load();

                    SelectedEmployee.Routes.Add(NewRoute);
                    NewRoute = new Route();
                    NewRoute.EmployeeID = SelectedEmployee.Id;
                    new Notification(Notification.NotificationType.Added, $"Ruten er blevet tilføjet i databasen.");
                    SelectedWorkplace = null;
                    NotifyOfPropertyChange(() => RouteCollection);
                }
            });
        }
        #endregion

        private async Task<List<SixtyDayHolder>> GetSixtyDayDataAsync()
        {
            List<SixtyDayHolder> lst = new List<SixtyDayHolder>();
            await Task.Run(() => 
            {
                using (var ctx = new DatabaseDir.Database())
                {
                    // Query the database for all the timesheet entries belonging to this year.
                    var data = ctx.TimesheetEntries.Include(p => p.LinkedWorkplace).Where(x => x.EmployeeID == SelectedEmployee.Id).ToList();

                    // Setup the datetime classes to calculate the weeknumbers.
                    DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                    Calendar cal = dfi.Calendar;

                    // Loop through the timesheet entries and figure out which row and column it belongs to.
                    foreach (var ent in data)
                    {
                        bool droveToWorkplace = ent.vismaEntries.FirstOrDefault(x => x.LinkedRate.Name == "Kørsel") != null;
                        if (!droveToWorkplace)
                            continue;

                        // Check the rows for any existing workplace.
                        SixtyDayHolder sixHolder = lst.FirstOrDefault(x => x.WorkplaceID == ent.LinkedWorkplace.Id
                                                                                     && x.Year == ent.Date.Year);

                        // If there's no workplace added for this year, instantiate a new row.
                        if (sixHolder == null)
                            sixHolder = new SixtyDayHolder(ent.LinkedWorkplace.Name, (int)ent.WorkplaceID, ent.Date.Year);

                        // Calculate the index for the timesheet entry.
                        int index;
                        int week = cal.GetWeekOfYear(ent.Date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                        if (week % 2 == 0)
                        {
                            index = (week / 2);
                        }
                        else
                        {
                            index = (week - 1) / 2;
                        }
                        // In case the month is january, and week is either 52 or 53,
                        // we'll manually set the index to the first column.
                        if (ent.Date.Month == 1 && (week == 52 || week == 53))
                            index = 0;

                        // Increment the value for the column at the calculated index.
                        sixHolder.WeekValues[index] += 1;

                        // Add the row to the list if it doesn't 
                        if (!lst.Contains(sixHolder))
                            lst.Add(sixHolder);
                    }
                }
            });
            return lst;
        }

        private async Task<List<Workplace>> GetWorkplacesAsync()
        {
            using (var ctx = new DatabaseDir.Database())
            {
                var lst = await Task.Run(() => ctx.Workplaces.ToList());
                return lst;
            }
        }

        private double GetTotalHours()
        {
            using (var ctx = new DatabaseDir.Database())
            {
                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                Calendar cal = dfi.Calendar;
                //List<TimesheetEntry> timesheetEntries = ctx.TimesheetEntries.Include(x => x.vismaEntries).ToList().Where(x => x.Date.Year == DateTime.Now.Year && x.EmployeeID == SelectedEmployee.Id).ToList();
                //double totalHours = timesheetEntries.Sum(x => x.vismaEntries.Where(p => p.VismaID == 1100).Sum(k => k.Value));

                return 10;
            }
        }

        private double GetAverageHoursPerWeek()
        {
            using (var ctx = new DatabaseDir.Database())
            {
                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                Calendar cal = dfi.Calendar;
                List<TimesheetEntry> timesheetEntries = ctx.TimesheetEntries.Include(x => x.vismaEntries).ToList().Where(x => x.Date.Year == DateTime.Now.Year && x.EmployeeID == SelectedEmployee.Id).ToList();

                double totalHours = timesheetEntries.Sum(x => x.vismaEntries.Where(p => p.VismaID == 1100).Sum(k => k.Value));
                double averageHours = totalHours / DateHelper.GetWeekNumber(DateTime.Now);
                // Rounds the average hours to two decimals
                return averageHours = Math.Round(averageHours, 2, MidpointRounding.AwayFromZero);
            }
        }

        private double GetNumberOfSickHours()
        {
            using (var ctx = new DatabaseDir.Database())
            {
                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                Calendar cal = dfi.Calendar;
                List<TimesheetEntry> timesheetEntries = ctx.TimesheetEntries.Include(x => x.vismaEntries).ToList().Where(x => x.Date.Year == DateTime.Now.Year && x.EmployeeID == SelectedEmployee.Id).ToList();

                double totalSickHours = timesheetEntries.Sum(x => x.vismaEntries.Where(p => p.VismaID == 14).Sum(k => k.Value));
                // Rounds the average hours to two decimals
                return totalSickHours;
            }
        }

    }

    public struct EntryFormatted
    {
        public string Date { get; }
        public string Start { get; }
        public string End { get; }
        public double Value { get; }
        public string RateName { get; }
        public int RateID { get; }
        public string Comment { get; }

        public EntryFormatted(string date, string start, string end, double value, string rateName, int rateID, string comment)
        {
            this.Date = date;
            this.Start = start;
            this.End = end;
            this.Value = value;
            this.RateName = rateName;
            this.RateID = rateID;
            this.Comment = comment;
        }
    }

    // Equals one row in the datagrid
    public class SixtyDayHolder
    {
        private string WorkplaceName { get; }
        public int WorkplaceID { get; }
        public int Year { get; }
        public List<int> WeekValues { get; private set; }

        public string Title
        {
            get
            {
                return $"{Year} {WorkplaceName}";
            }
        }

        public SixtyDayHolder(string workplaceName, int workplaceID, int year)
        {
            this.WorkplaceName = workplaceName;
            this.WorkplaceID = workplaceID;
            this.Year = year;
            this.WeekValues = new List<int>(new int[27]); // 26 columns
        }
    }

    public class ProjectFormat
    {
        public string ProjectID { get; }
        public double Hours { get; set; }

        public ProjectFormat(string projectID)
        {
            this.ProjectID = projectID;
            this.Hours = 0;
        }
    }
}
