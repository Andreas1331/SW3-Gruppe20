using Caliburn.Micro;
using SW3Projekt.DatabaseDir;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.ViewModels
{
    public class EmployeeProfileViewModel : Screen
    {
        #region Properties
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
        public Workplace SelectedWorkplace {
            get {
                return _selectedWorkplace;
            }
            set {
                _selectedWorkplace = value;
                NewRoute.WorkplaceID = (SelectedWorkplace != null) ? SelectedWorkplace.Id : 0;
                NewRoute.LinkedWorkplace = SelectedWorkplace;
            }
        }

        // Workplaces used to display in the options on the combobox.
        private BindableCollection<Workplace> _workplaces;
        public BindableCollection<Workplace> Workplaces
        {
            get { 
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
        public bool CanEditEmployee {
            get {
                return _canEditEmployee;
            }
            set {
                _canEditEmployee = value;
                NotifyOfPropertyChange(() => CanEditEmployee);
            }
        }

        // The route collection is used to display all the employees unique routes.
        public BindableCollection<Route> RouteCollection {
            get {
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
        #endregion

        public EmployeeProfileViewModel(Employee emp)
        {
            SelectedEmployee = emp;

            // Instantiate the new route and set the foreignkey value to the,
            // currently selected employee.
            NewRoute = new Route();
            NewRoute.EmployeeID = SelectedEmployee.Id;

            Task.Run(async () => {
                var workplaces = await GetWorkplacesAsync();
                Workplaces = new BindableCollection<Workplace>(workplaces);
            });             
        }

        #region Buttons
        public void BtnSearchForEntries()
        {
            // TODO 1: Get all TimesheetEntries based on selected week, year and employee
            // TODO 2: Query for all VismaEntries linked to the TimesheetEntries
            // TODO 3: Query for all Rates linked to the VismaEntries
            // TODO 4: Format all the data into a new bindablecollection to display on the table

        }

        public void BtnEditEmployee()
        {
            CanEditEmployee = !CanEditEmployee;
        }

        public void BtnSaveEmployeeChanges()
        {
            using (var ctx = new Database())
            {
                ctx.Employees.Attach(SelectedEmployee);
                ctx.Entry(SelectedEmployee).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }
        }
        
        public void BtnDeleteEmployee()
        {
            using (var ctx = new Database())
            {
                ctx.Employees.Attach(SelectedEmployee);
                ctx.Entry(SelectedEmployee).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }
        }

        public void BtnAddNewRoute()
        {
            using (var ctx = new Database())
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
    }

    internal class EntryFormatted
    {
        public string Start { get; private set; }
        public string End { get; private set; }
        public float Value { get; private set; }
        public string RateName { get; private set; }
        public int RateID { get; private set; }
        public string Comment { get; private set; }
    }
}
