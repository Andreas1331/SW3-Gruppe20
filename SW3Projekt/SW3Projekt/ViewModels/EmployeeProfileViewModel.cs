using Caliburn.Micro;
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
        public Employee SelectedEmployee { get; set; }

        public EmployeeProfileViewModel(Employee emp)
        {
            SelectedEmployee = emp;
        }
    }
}
