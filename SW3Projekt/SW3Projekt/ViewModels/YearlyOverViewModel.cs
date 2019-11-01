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
    public class YearlyOverViewModel : Screen
    {
        //Properties
        private List<Employee> AllEmployees = new List<Employee>();
        public ObservableCollection<int> WeekNumbers{ get; set;}
        private int _weekNumber { get; set; }


        public YearlyOverViewModel()
        {
            for (int i = 0; i < 53; i++)
            {
                WeekNumbers[i] = i + 1;
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
                foreach (Timesheet TS in employee.Timesheets)
                {
                    Console.WriteLine(TS.WeekNumber);

                    
                }
            }

            //WeekNumbers.Add(AllEmployees[1].Timesheets[1].WeekNumber);

            
        }
    }
}
