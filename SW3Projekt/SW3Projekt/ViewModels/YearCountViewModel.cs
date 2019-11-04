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

        //Properties to combobox and its items
        public bool DisplayHours;
        private ObservableCollection<string> _valueToDisplayCbox = new ObservableCollection<string>() { "Timer", "Penge" };
        public ObservableCollection<string> ValueToDisplayCbox { get { return _valueToDisplayCbox; } }


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

        public YearCountViewModel()
        {
            int VismaIdNormHours         = 1100;
            int VismaIdRate1Hours        = 1311;
            int VismaIdRate2Hours        = 1312;
            int VismaIdRate3Hours        = 1313;
            int VismaIdRate4Hours        = 1314;
            int VismaIdDietHours         = 9020;
            int VismaIdTaxFreeDriveHours = 9010;
            int VismaIdTaxDriveHours     = 1181;
            int VismaIdPaidLeaveHours    = 1400;

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
                    List<TimesheetEntry> timesheetEntries = ctx.TimesheetEntries.Include(k => k.vismaEntries).Where(x => x.EmployeeID == employee.Id).ToList();

                    // Loop through 52 + 1 weeks and sum up his total work hours for each week.
                    for (int i = 1; i <= 53; i++)
                    {
                        var sumTotalHours   = GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdNormHours, dfi, cal, i);
                        var sumRate1        = GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdRate1Hours, dfi, cal, i);
                        var sumRate2        = GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdRate2Hours, dfi, cal, i);
                        var sumRate3        = GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdRate3Hours, dfi, cal, i);
                        var sumRate4        = GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdRate4Hours, dfi, cal, i);
                        var sumDiet         = GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdDietHours, dfi, cal, i);
                        var sumDriveTaxFree = GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdTaxFreeDriveHours, dfi, cal, i);
                        var sumDriveTax     = GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdTaxDriveHours, dfi, cal, i);
                        var sumPaidLeave    = GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdPaidLeaveHours, dfi, cal, i);
                          
                        AddHoursToWeek(i, sumTotalHours, sumRate1, sumRate2, sumRate3, sumRate4, sumDiet, sumDriveTaxFree, sumDriveTax, sumPaidLeave);
                    }
                }
            }
        }

        double GetAmountOfHoursTotalOfRate(List<TimesheetEntry> tsEntry, int vismaId, DateTimeFormatInfo dfi, Calendar cal, int index)
        {
            return tsEntry.Where(x => cal.GetWeekOfYear(x.Date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == index)
                          .Sum(x => x.vismaEntries.Where(k => k.VismaID == vismaId).Sum(k => k.Value));
        }

        // TODO: Consider renaming method, and pick a method to go with.
        private void AddHoursToWeek(int i, double normHours, double rate1, double rate2, double rate3, double rate4, double diet, double driveTaxFree, double driveTax, double paidLeave)
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
