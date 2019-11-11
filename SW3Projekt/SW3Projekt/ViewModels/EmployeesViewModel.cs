using Caliburn.Micro;
using SW3Projekt.DatabaseDir;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Data.Entity;

namespace SW3Projekt.ViewModels
{
    public class EmployeesViewModel : Conductor<object>
    {
        #region Properties
        public int cornerRadius { get; set; } = 0;

        // List to constantly keep track of all the employees
        private List<Employee> AllEmployees { get; set; }
        // Collection used to determin which employees are currently being shown on the datagrid
        private BindableCollection<Employee> _employeeCollection;
        public BindableCollection<Employee> EmployeeCollection {
            get { return _employeeCollection; }
            set {
                _employeeCollection = value;
                NotifyOfPropertyChange(() => EmployeeCollection);
            }
        }

        // Reference to the employee currently selected by the user on the datagrid
        public Employee SelectedEmployee { get; set; }

        private Employee _newEmployee;
        public Employee NewEmployee {
            get {
                return _newEmployee;
            }
            set {
                _newEmployee = value;
                NotifyOfPropertyChange<Employee>(() => NewEmployee);
            }
        }

        // This datetime is used to display the current day when,
        // adding a new employee.
        public DateTime DaysDate { get; } = DateTime.Now;

        private string _searchEmployeeText = "";
        public string SearchEmployeeText {
            get {
                return _searchEmployeeText;
            }
            set {
                _searchEmployeeText = value;
                NotifyOfPropertyChange(() => SearchEmployeeText);
                Task.Run(async () => await SearchForEmployeeAsync(SearchEmployeeText));
            }
        }

        // This checks if BtnAddNewEmployee can be clicked
        public bool CanBtnAddNewEmployee { get { return CanAddNewEmployee; } }

        private bool _canAddNewEmployee = true;
        public bool CanAddNewEmployee {
            get {
                return _canAddNewEmployee;
            }
            set {
                _canAddNewEmployee = value;
                NotifyOfPropertyChange(() => CanAddNewEmployee);
                NotifyOfPropertyChange(() => CanBtnAddNewEmployee);
            }
        }
        #endregion

        public EmployeesViewModel()
        {
            NewEmployee = new Employee();
            Task.Run(async () =>
            {
                AllEmployees = await GetEmployeesAsync();
                EmployeeCollection = new BindableCollection<Employee>(AllEmployees);
            });
        }

        public async Task BtnAddNewEmployee()
        {
            using (var ctx = new DatabaseDir.Database())
            {
                CanAddNewEmployee = false;

                bool success = await Task<bool>.Run(() =>
                {
                    try
                    {
                        ctx.Employees.Add(NewEmployee);
                        ctx.SaveChanges();
                        return true;
                    } catch (DbUpdateException ex)
                    {
                        // Should we log exceptions?
                        return false;
                    }
                });

                if (success)
                {
                    new Notification(Notification.NotificationType.Added, $"{NewEmployee.Fullname} er blevet tilføjet til databasen.");
                    NewEmployee = new Employee();

                    AllEmployees = await GetEmployeesAsync();
                    EmployeeCollection = new BindableCollection<Employee>(AllEmployees);
                }

                CanAddNewEmployee = true;
            }
        }

        public void EmployeeDoubleClicked()
        {
            ActivateItem(new EmployeeProfileViewModel(SelectedEmployee));
        }

        // TODO: Move this searching logic to another place perhaps?? Maybe
        public async Task SearchForEmployeeAsync(string criteria)
        {
            // First determine if the user is searching by employee ID (int) or name (string)
            int employeeID;
            if (int.TryParse(criteria, out employeeID))
            {
                // Searched by ID
                EmployeeCollection = await Task.Run(() => new BindableCollection<Employee>(AllEmployees.Where(x => x.Id.ToString().Contains(criteria)).ToList()));
            }
            else
            {
                // Searched by name
                // Check if the user cleared the field, and then just display everyone once more
                if (String.IsNullOrEmpty(criteria))
                {
                    EmployeeCollection = new BindableCollection<Employee>(AllEmployees);
                }
                // Otherwise attempt to find employees based on the search value
                else
                {
                    EmployeeCollection = await Task.Run(() => new BindableCollection<Employee>(AllEmployees.Where(x => x.Fullname.ToLower().Contains(criteria.ToLower())).ToList()));
                }
            }
        }

        private async Task<List<Employee>> GetEmployeesAsync()
        {
            using (var ctx = new DatabaseDir.Database())
            {
                List<Employee> employees = await Task.Run(() => ctx.Employees.Include(x => x.Routes).ToList());
                employees = ctx.Employees.Include(emp => emp.Routes.Select(k => k.LinkedWorkplace)).ToList();

                foreach (var item in employees)
                {
                    if (item.Routes == null || item.Routes.Count <= 0)
                        continue;

                    //item.Routes[0].LinkedWorkplace.ToString();
                    continue;

                    Console.WriteLine("Type equals: " + (typeof(Workplace) == item.Routes[0].LinkedWorkplace.GetType()));

                    Console.WriteLine("Value: " + item.Routes[0].LinkedWorkplace + "  -  Type: " + item.Routes[0].LinkedWorkplace.GetType());
                }
                return employees;
            }
        }
    }
}
