using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SW3Projekt.Models.Repository;

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
        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get
            {
                return _selectedEmployee;
            }
            set
            {
                _selectedEmployee = value;
                NotifyOfPropertyChange(() => CanBtnViewEmployeeProfile);
            }
        }

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

        // This checks if BtnAddNewEmployee can be clicked
        private bool _canBtnAddNewEmployee = true;
        public bool CanBtnAddNewEmployee
        {
            get
            {
                return _canBtnAddNewEmployee;
            }
            set
            {
                _canBtnAddNewEmployee = value;
                NotifyOfPropertyChange(() => CanBtnAddNewEmployee);
            }
        }

        public bool CanBtnViewEmployeeProfile
        {
            get
            {
                return SelectedEmployee != null;
            }
        }
        #endregion

        public IRepository<Employee> RepositoryEmployees { get; set; }

        public EmployeesViewModel()
        {
            //var repoo = IoC.Get<IRepository<Employee>>();
            NewEmployee = new Employee();
            Task.Run(async () =>
            {
                AllEmployees = await GetEmployeesAsync();
                EmployeeCollection = new BindableCollection<Employee>(AllEmployees);
            });
        }

        public async Task BtnAddNewEmployee()
        {
            CanBtnAddNewEmployee = false;
            Cursor.Current = Cursors.WaitCursor;

            bool success = await Task<bool>.Run(() =>
            {
                try
                {
                    RepositoryEmployees.Add(NewEmployee);
                    RepositoryEmployees.Save();
                    return true;
                }
                catch (Exception ex)
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
            else
            {
                new Notification(Notification.NotificationType.Error, "Der skete en fejl. Tjek de indtastede informationer og prøv igen.", 7.5f);
            }

            CanBtnAddNewEmployee = true;
            Cursor.Current = Cursors.Default;
        }

        public void BtnViewEmployeeProfile()
        {
            EmployeeDoubleClicked();
        }

        public void EmployeeDoubleClicked()
        {
            if (SelectedEmployee == null)
                return;

            Cursor.Current = Cursors.WaitCursor;
            ShellViewModel.Singleton.ActivateItem(new EmployeeProfileViewModel(SelectedEmployee));
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
            List<Employee> employees = await Task.Run(() => RepositoryEmployees.GetAll().OrderBy(p => p.IsFired).ToList());
            employees.OrderBy(p => p.IsFired).ToList();
            return employees;
        }
    }
}
