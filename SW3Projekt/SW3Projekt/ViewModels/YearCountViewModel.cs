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
                        //var sumTotalHours = timesheetEntries.Where(x => cal.GetWeekOfYear(x.Date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == i)
                                                         //.Sum(x => x.vismaEntries.Sum(k => k.Value));

                        var sumTotalHours = timesheetEntries.Where(x => cal.GetWeekOfYear(x.Date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == i)
                                                        .Sum(x => x.vismaEntries.Where(k => k.VismaID == 1100).Sum(k => k.Value));

                        var sumRate1 = timesheetEntries.Where(x => cal.GetWeekOfYear(x.Date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == i)
                                                        .Sum(x => x.vismaEntries.Where(k => k.VismaID == 1311).Sum(k => k.Value));

                        var sumRate2 = timesheetEntries.Where(x => cal.GetWeekOfYear(x.Date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == i)
                                                        .Sum(x => x.vismaEntries.Where(k => k.VismaID == 1312).Sum(k => k.Value));
                        
                        var sumRate3 = timesheetEntries.Where(x => cal.GetWeekOfYear(x.Date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == i)
                                                        .Sum(x => x.vismaEntries.Where(k => k.VismaID == 1313).Sum(k => k.Value));

                        var sumRate4 = timesheetEntries.Where(x => cal.GetWeekOfYear(x.Date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == i)
                                                        .Sum(x => x.vismaEntries.Where(k => k.VismaID == 1314).Sum(k => k.Value));

                        var sumDiet = timesheetEntries.Where(x => cal.GetWeekOfYear(x.Date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == i)
                                                        .Sum(x => x.vismaEntries.Where(k => k.VismaID == 9020).Sum(k => k.Value));

                        var sumDriveTaxFree = timesheetEntries.Where(x => cal.GetWeekOfYear(x.Date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == i)
                                                        .Sum(x => x.vismaEntries.Where(k => k.VismaID == 9010).Sum(k => k.Value));
                        
                        var sumDriveTax = timesheetEntries.Where(x => cal.GetWeekOfYear(x.Date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == i)
                                                        .Sum(x => x.vismaEntries.Where(k => k.VismaID == 1181).Sum(k => k.Value));

                        var sumPaidLeave = timesheetEntries.Where(x => cal.GetWeekOfYear(x.Date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == i)
                                                        .Sum(x => x.vismaEntries.Where(k => k.VismaID == 1400).Sum(k => k.Value));

                        AddNormHoursToWeek(i, sumTotalHours, sumRate1, sumRate2, sumRate3, sumRate4, sumDiet, sumDriveTaxFree, sumDriveTax, sumPaidLeave);
                    }
                }
            }
        }

        // TODO: Consider renaming method, and pick a method to go with.
        private void AddNormHoursToWeek(int i, double normHours, double rate1, double rate2, double rate3, double rate4, double diet, double driveTaxFree, double driveTax, double paidLeave)
        {
            /* Method 1 */
            YearCount year;
            bool exists = Years.TryGetValue(i, out year);
            if (exists)
            {
                year.TotalHours += normHours;
                year.Rate1 += rate1;
                year.Rate2 += rate2;
                year.Rate3 += rate3;
                year.Rate4 += rate4;
                year.Diet += diet;
                year.TaxFreeKM1 += driveTaxFree;
                year.TaxableKM += driveTax;
                year.PaidLeave += paidLeave;
            }
            else
            {
                year = new YearCount() { WeekNumber = i };
                year.TotalHours += normHours;
                year.Rate1 += rate1;
                year.Rate2 += rate2;
                year.Rate3 += rate3;
                year.Rate4 += rate4;
                year.Diet += diet;
                year.TaxFreeKM1 += driveTaxFree;
                year.TaxableKM += driveTax;
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
