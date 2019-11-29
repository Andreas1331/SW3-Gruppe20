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
        //Properties for chosen year
        private int _chosenYear = DateTime.Now.Year;
        public int ChosenYear
        {
            get
            {
                return _chosenYear;
            }
            set
            {
                _chosenYear = value;
                NotifyOfPropertyChange(() => ChosenYear);
            }
        }

        // Key being the weeknumber (Used for Method 1 & 2 further down the pipe)
        private Dictionary<int, YearCount> Years = new Dictionary<int, YearCount>();

        //Properties to combobox selected item
        private string _valueToDisplay = "Timer";
        public string ValueToDisplay
        {
            get
            {
                return _valueToDisplay;
            }
            set
            {
                _valueToDisplay = value;
                NotifyOfPropertyChange(() => ValueToDisplay);
            }
        }

        // Display value option - ComboBox binds to this
        private ObservableCollection<string> _valueToDisplayCbox = new ObservableCollection<string>() { "Timer", "Kroner" };
        public ObservableCollection<string> ValueToDisplayCbox { get { return _valueToDisplayCbox; } }

        // Collection property, the Datagrid binds to this
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

        //Button methods
        public void BtnChooseValueToDisplay()
        {
            //Check if the user have chosen hours or money in the combobox
            if(ValueToDisplay == "Timer")
            {   
                //If hours is selected, call the displaydata method with the false statement, since we won't calculate the money
                DisplayDataInGrid(false);
                NotifyOfPropertyChange(() => YearCountCollection);
            } 
            else
            {
                //If money is chosen, then call with true statement because the money should be calculated.
                DisplayDataInGrid(true);
                NotifyOfPropertyChange(() => YearCountCollection);
            }
        }

        //The main method to get the corresponding data from the databse
        public void DisplayDataInGrid(bool displayAsMoney)
        {
            int VismaIdSickness          = 14;
            int VismaId6MonthSickness    = 15;
            int VismaIdChildSickness     = 13;
            int VismaIdNormHours         = 1100;
            int VismaIdRate1Hours        = 1311;
            int VismaIdRate2_1Hours      = 1312;
            int VismaIdRate2_2Hours      = 1316;
            int VismaIdRate3Hours        = 1318;
            int VismaIdRate4_1Hours      = 1313;
            int VismaIdRate4_2Hours      = 1315;
            int VismaIdRate4_3Hours      = 1317;
            int VismaIdRate4_4Hours      = 1319;
            int VismaIdDietHours         = 9020;
            int VismaIdTaxFreeDriveHours = 9010;
            int VismaIdTaxDriveHours     = 1181;
            int VismaIdPaidLeaveHours    = 1400;

            //Clear the dictionary to display correct data, since we increase the numbers everytime the method AddHoursToWeek is called
            Years.Clear();

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
                        //Now we collect the data with the correct weeknumber, vismaId, etc.
                        var sumSickness     = GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdSickness, dfi, cal, i, false)
                                            + GetAmountOfHoursTotalOfRate(timesheetEntries, VismaId6MonthSickness, dfi, cal, i, false) 
                                            + GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdChildSickness, dfi, cal, i, false);
                        var sumTotalHours   = GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdNormHours, dfi, cal, i, false);
                        var sumRate1        = GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdRate1Hours, dfi, cal, i, displayAsMoney);
                        var sumRate2        = GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdRate2_1Hours, dfi, cal, i, displayAsMoney) +
                                            + GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdRate2_2Hours, dfi, cal, i, displayAsMoney);
                        var sumRate3        = GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdRate3Hours, dfi, cal, i, displayAsMoney);
                        var sumRate4        = GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdRate4_1Hours, dfi, cal, i, displayAsMoney)
                                            + GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdRate4_2Hours, dfi, cal, i, displayAsMoney)
                                            + GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdRate4_3Hours, dfi, cal, i, displayAsMoney)
                                            + GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdRate4_4Hours, dfi, cal, i, displayAsMoney);
                        var sumDiet         = GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdDietHours, dfi, cal, i, false);
                        var sumDriveTaxFree = GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdTaxFreeDriveHours, dfi, cal, i, false);
                        var sumDriveTax     = GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdTaxDriveHours, dfi, cal, i, false);
                        var sumPaidLeave    = GetAmountOfHoursTotalOfRate(timesheetEntries, VismaIdPaidLeaveHours, dfi, cal, i, displayAsMoney);
                        
                        //Lastly we add the data to the dictionary to be displayed in the datagrid.
                        AddHoursToWeek(i, sumSickness, sumTotalHours, sumRate1, sumRate2, sumRate3, sumRate4, sumDiet, sumDriveTaxFree, sumDriveTax, sumPaidLeave);
                    }
                }
            }
        }

        //Returns the numbers in doubles, from the database with the corresponding visma id and date
        double GetAmountOfHoursTotalOfRate(List<TimesheetEntry> tsEntry, int vismaId, DateTimeFormatInfo dfi, Calendar cal, int index, bool asMoney)
        {
            if(asMoney)
            {
                return tsEntry.Where(x => x.Date.Year == ChosenYear)
                              .Where(x => cal.GetWeekOfYear(x.Date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == index)
                              .Sum(x => x.vismaEntries
                                  .Where(k => k.VismaID == vismaId)
                                  .Sum(k => k.Value * k.RateValue));
            }
            else
            {
                return tsEntry.Where(x => x.Date.Year == ChosenYear)
                              .Where(x => cal.GetWeekOfYear(x.Date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == index)
                              .Sum(x => x.vismaEntries
                                  .Where(k => k.VismaID == vismaId)
                                  .Sum(k => k.Value));
            }
        }

        // TODO: Consider renaming method, and pick a method to go with.
        private void AddHoursToWeek(int i, double sickness, double normHours, double rate1, double rate2, double rate3, double rate4, double diet, double driveTaxFree, double driveTax, double paidLeave)
        {
            /* Method 1 */
            YearCount year;
            bool exists = Years.TryGetValue(i, out year);
            if (exists)
            {
                year.IllnessTotal += sickness;
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
                year.IllnessTotal += sickness;
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
