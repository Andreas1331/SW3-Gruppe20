using Caliburn.Micro;
using SW3Projekt.DatabaseDir;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SW3Projekt.ViewModels
{
    public class EmployeesViewModel : Conductor<object>
    {
        // List to constantly keep track of all the employees
        private List<Employee> AllEmployees { get; set; }
        // Collection used to determin which employees are currently being shown on the datagrid
        private BindableCollection<Employee> _employeeCollection;
        public BindableCollection<Employee> EmployeeCollection
        {
            get { return _employeeCollection; }
            set
            {
                _employeeCollection = value;
                NotifyOfPropertyChange(() => EmployeeCollection);
            }
        }
        // Reference to the employee currently selected by the user on the datagrid
        public Employee SelectedEmployee { get; set; }

        private Employee _newEmployee;
        public Employee NewEmployee
        {
            get
            {
                return _newEmployee;
            }
            set
            {
                _newEmployee = value;
                NotifyOfPropertyChange<Employee>(() => NewEmployee);
            }
        }
        // This datetime is used to display the current day when,
        // adding a new employee.
        public DateTime DaysDate { get; } = DateTime.Now;

        private string _searchEmployeeText = "";
        public string SearchEmployeeText
        {
            get
            {
                return _searchEmployeeText;
            }
            set
            {
                _searchEmployeeText = value;
                NotifyOfPropertyChange(() => SearchEmployeeText);
                Task.Run(async () => await SearchForEmployeeAsync(SearchEmployeeText));
            }
        }

        private string _addingProgressState = "";
        public string AddingProgressState
        {
            get
            {
                return _addingProgressState;
            }
            set
            {

            }
        }

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
            using (var ctx = new Database())
            {
                AddingProgressState = "Vent venligst...";

                ctx.Employees.Add(NewEmployee);
                await ctx.SaveChangesAsync();

                AddingProgressState = "";

                AllEmployees = await GetEmployeesAsync();
                EmployeeCollection = new BindableCollection<Employee>(AllEmployees);
            }
        }
        public void EmployeeDoubleClicked()
        {
            Console.WriteLine("NAME: " + SelectedEmployee?.Firstname);
            ActivateItem(new EmployeeProfileViewModel(SelectedEmployee));
        }

        public bool CanBtnAddNewEmployee()
        {
            return false;
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
            using (var ctx = new Database())
            {
                List<Employee> employees = await Task.Run(() => ctx.Employees.ToList());
                return employees;
            }
            return new List<Employee>()
                    {
                        new Employee(){Firstname = "Andreas", Surname = "Christensen", Id = 2313},
                        new Employee(){Firstname = "Andreas", Surname = "Andersen", Id = 513},
                        new Employee(){Firstname = "Michael", Surname = "Michaelsen", Id = 90},
                        new Employee(){Firstname = "Martin", Surname = "Martinsen", Id = 12345},
                        new Employee(){Firstname = "Shpend", Surname = "G", Id = 60},
                        new Employee(){Firstname = "Filip", Surname = "Filipsen", Id = 930},
                        new Employee(){Firstname = "Emil", Surname = "Emilsen", Id = 930}
            };
        }
    }
}
