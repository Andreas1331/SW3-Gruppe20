using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Globalization;

namespace SW3Projekt.ViewModels
{
    public class YearCountViewModel : Screen
    {
        #region Properties
        private List<Employee> AllEmployees = new List<Employee>();

        // Key being the weeknumber (Used for Method 1 & 2 further down the pipe)
        private Dictionary<int, YearCount> Years = new Dictionary<int, YearCount>();
        
        private List<YearCount> _yearCounts = new List<YearCount>();
        public List<YearCount> YearCounts
        {
            get { return _yearCounts; }
            set { _yearCounts = value; }
        }

        private BindableCollection<YearCount> _yearCountCollection;
        public BindableCollection<YearCount> YearCountCollection
        {
            get
            {
                return new BindableCollection<YearCount>(Years.Values.ToList());
                //return new BindableCollection<YearCount>(YearCounts);
            }
            set
            {
                // CONSIDER: Probably wont need a setter, as it returns the value of another property.
                _yearCountCollection = value;
                NotifyOfPropertyChange(() => YearCountCollection);
            }
        }

        private int _weekNumber { get; set; }

        public YearCountViewModel()
        {
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                // TODO: Also take the year into consideration and not only the weeknumber!
                List<Employee> emps = ctx.Employees.ToList();
                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                Calendar cal = dfi.Calendar;

                // Loop through all the employeees.
                foreach (Employee employee in emps)
                {
                    // Query for the current employees timesheetentries and vismaentries.
                    var timesheetEntries = ctx.TimesheetEntries.Include(k => k.vismaEntries).Where(x => x.EmployeeID == employee.Id).ToList();

                    // Loop through 52 + 1 weeks and sum up his total work hours for each week.
                    for (int i = 1; i <= 53; i++)
                    {
                        var sum = timesheetEntries.Where(x => cal.GetWeekOfYear(x.Date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == i)
                                                         .Sum(x => x.vismaEntries.Sum(k => k.Value));
                        AddHoursToWeek(i, sum);
                    }
                }
            }
        }

        // TODO: Consider renaming method, and pick a method to go with.
        private void AddHoursToWeek(int i, double value)
        {
            /* Method 1 */
            YearCount year;
            bool exists = Years.TryGetValue(i, out year);
            if (exists)
            {
                year.TotalHours += value;
            }
            else
            {
                year = new YearCount() { WeekNumber = i };
                year.TotalHours += value;
                Years.Add(i, year);
            }

            /* Method 2 */
            //YearCount ye = (Years.ContainsKey(i)) ? Years[i] : new YearCount() { WeekNumber = 1 };
            //ye.TotalHours += value;
            //if (!Years.ContainsKey(i))
            //    Years.Add(i, ye);

            /* Method 3 */
            //YearCount year = (YearCounts.Find(x => x.WeekNumber == i)) ?? new YearCount() { WeekNumber = i };
            //year.TotalHours += value;
            //Console.WriteLine("i:" + i + " - Val:" + year.TotalHours);
            //if (!YearCounts.Contains(year))
            //    YearCounts.Add(year);
        }
    }
}
