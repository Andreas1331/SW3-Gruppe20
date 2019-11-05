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

        public double TotalHoursForThisYear {
            get {
                return GetTotalHours();
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

            Task.Run(async () =>
            {
                var workplaces = await GetWorkplacesAsync();
                Workplaces = new BindableCollection<Workplace>(workplaces);
            });
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
                foreach(TimesheetEntry ts in entries)
                {
                    //entriesFormatted.Add(new EntryFormatted(ts.StartTime.ToString("mm"), ts.EndTime.ToString("mm"), ;
                    foreach(VismaEntry visma in ts.vismaEntries)
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
                Console.WriteLine("Count: " + entriesFormatted.Count);
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
                ctx.Entry(SelectedEmployee).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public void BtnDeleteEmployee()
        {
            using (var ctx = new DatabaseDir.Database())
            {
                ctx.Employees.Attach(SelectedEmployee);
                ctx.Entry(SelectedEmployee).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }
        }

        public void BtnAddNewRoute()
        {
            using (var ctx = new DatabaseDir.Database())
            {
                ctx.Routes.Add(NewRoute);
                ctx.SaveChanges();

                SelectedEmployee.Routes.Add(NewRoute);
                NewRoute = new Route();
                NewRoute.EmployeeID = SelectedEmployee.Id;
                SelectedWorkplace = null;
                NotifyOfPropertyChange(() => RouteCollection);
            }
        }
        #endregion

        private async Task<List<Workplace>> GetWorkplacesAsync()
        {
            using (var ctx = new DatabaseDir.Database())
            {
                var lst = await Task.Run(() => ctx.Workplaces.ToList());
                return lst;
            }
        }

        private double GetTotalHours () {
            using (var ctx = new DatabaseDir.Database())
            {
                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                Calendar cal = dfi.Calendar;
                List<TimesheetEntry> timesheetEntries = ctx.TimesheetEntries.Include(x => x.vismaEntries).ToList().Where(x => x.Date.Year == DateTime.Now.Year).ToList();

                double totalHours = timesheetEntries.Sum(x => x.vismaEntries.Sum(k => k.Value));

                return totalHours;
            }
        } 
    }

    public class EntryFormatted
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
}
