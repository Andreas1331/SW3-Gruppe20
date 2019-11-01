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

        private BindableCollection<YearCount> _yearCountEntries;
        public BindableCollection<YearCount> YearCountEntries
        {
            get
            {
                return _yearCountEntries;
            }
            set
            {
                _yearCountEntries = value;
                NotifyOfPropertyChange(() => YearCountEntries);
            }
        }

        private int _weekNumber { get; set; }


        public YearCountViewModel()
        {
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                for (int i = 1; i <= 1; i++)
                {
                    YearCount yc = new YearCount();
                    yc.WeekNumber = i;

                    
                    foreach (Employee employee in ctx.Employees)
                    {
                        //yc.TotalHours = employee.Timesheets.ForEach(x => x.TSEntries.ForEach(y => y.vismaEntries.ForEach(y => y.Value)));
                        if (employee.Timesheets != null)
                        {
                            foreach (var empTs in employee.Timesheets)
                            {
                                foreach (var EmpTsEnt in empTs.TSEntries)
                                {
                                    foreach (var EmpTsEntVis in EmpTsEnt.vismaEntries)
                                    {
                                        yc.TotalHours += EmpTsEntVis.Value;
                                    }
                                }
                            }
                        }
                        AllEmployees.Add(employee);
                    }

                    YearCountEntries.Add(yc);
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

        }
    }
}
