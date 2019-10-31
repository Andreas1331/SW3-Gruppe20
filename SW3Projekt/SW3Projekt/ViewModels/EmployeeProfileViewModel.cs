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
        public Employee SelectedEmployee { get; set; }

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

        public EmployeeProfileViewModel(Employee emp)
        {
            SelectedEmployee = emp;
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
    }
}
