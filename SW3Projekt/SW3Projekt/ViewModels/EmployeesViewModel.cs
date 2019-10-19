using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;

namespace SW3Projekt.ViewModels
{
    public class EmployeesViewModel : Screen
    {
        public BindableCollection<Employee> Employees { get; set; }
        public Employee SelectedEmployee { get; set; }

        public EmployeesViewModel()
        {
            Employees = new BindableCollection<Employee>(GetEmployees());
        }

        public void EmployeeDoubleClicked()
        {
            Console.WriteLine("NAME: " + SelectedEmployee?.Firstname);
        }

        private List<Employee> GetEmployees()
        {
            // TODO: Query the database for the employees instead of generating a testing list
            return new List<Employee>()
                    {
                        new Employee(){Firstname = "Andreas", Surname = "Christensen"},
                        new Employee(){Firstname = "Andreas", Surname = "Andersen"},
                        new Employee(){Firstname = "Michael", Surname = "Michaelsen"},
                    };
        }
    }
}
