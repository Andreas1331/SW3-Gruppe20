using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.ViewModels
{
    public class YearCountViewModel : Screen
    {
        //Properties
        private List<Employee> AllEmployees = new List<Employee>();
        private BindableCollection<int> _weekNumbers;
        public BindableCollection<YearCount> WeekNumbers { get { return _weekNumbers; } set { _weekNumbers = value; NotifyOfPropertyChange(() => WeekNumbers); } }
        private int _weekNumber { get; set; }


        public YearCountViewModel()
        {
            for (int i = 1; i <= 53; i++)
            {
                WeekNumbers.Add(i);
            }

            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                foreach (Employee employee in ctx.Employees)
                {
                    AllEmployees.Add(employee);
                }
            }

            foreach (Employee employee in AllEmployees)
            {
                if (employee.Timesheets != null)
                {
                    foreach (Timesheet TS in employee.Timesheets)
                    {
                        Console.WriteLine(TS.WeekNumber);


                    }
                }
            }

        }    //WeekNumbers.Add(AllEmployees[1].Timesheets[1].WeekNumber);
    }
}
