using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SW3Projekt.ViewModels
{
    public class EmployeesViewModel : Conductor<object>
    {
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

        private string _searchEmployeeText = "";
        public string SearchEmployeeText {
            get { 
                return _searchEmployeeText; }
            set {
                _searchEmployeeText = value;
                SearchForEmployee(SearchEmployeeText);
                NotifyOfPropertyChange(() => SearchEmployeeText);
            } 
        }

        public EmployeesViewModel()
        {
            AllEmployees = GetEmployees();
            EmployeeCollection = new BindableCollection<Employee>(AllEmployees);
        }

        public void EmployeeDoubleClicked()
        {
            Console.WriteLine("NAME: " + SelectedEmployee?.Firstname);
            ActivateItem(new EmployeeProfileViewModel(SelectedEmployee));
        }

        public void SearchForEmployee(string criteria)
        {
            Console.WriteLine("Value: " + criteria);
            // First determin if the user is searching by employee ID (int) or name (string)
            int employeeID;
            if(int.TryParse(criteria, out employeeID))
            {
                // Searched by ID
                //EmployeeCollection = new BindableCollection<Employee>(AllEmployees.Where(x => x.EmployeeID.ToString().Contains(criteria)).ToList());
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
                    EmployeeCollection = new BindableCollection<Employee>(AllEmployees.Where(x => x.Fullname.ToLower().Contains(criteria.ToLower())).ToList());
                }
            }
        }

        private List<Employee> GetEmployees()
        {
            // TODO: Query the database for the employees instead of generating a testing list
            return new List<Employee>()
                    {
                    //    new Employee(){Firstname = "Andreas", Surname = "Christensen", EmployeeID = 2313},
                    //    new Employee(){Firstname = "Andreas", Surname = "Andersen", EmployeeID = 513},
                    //    new Employee(){Firstname = "Michael", Surname = "Michaelsen", EmployeeID = 90},
                    //    new Employee(){Firstname = "Martin", Surname = "Martinsen", EmployeeID = 12345},
                    //    new Employee(){Firstname = "Shpend", Surname = "G", EmployeeID = 60},
                    //    new Employee(){Firstname = "Filip", Surname = "Filipsen", EmployeeID = 930},
                    //    new Employee(){Firstname = "Emil", Surname = "Emilsen", EmployeeID = 930}
                    };
        }
    }
}
