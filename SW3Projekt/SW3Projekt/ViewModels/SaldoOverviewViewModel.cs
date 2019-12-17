using Caliburn.Micro;
using SW3Projekt.Models;
using SW3Projekt.Tools;
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
    public class SaldoOverviewViewModel : Screen
    {
        private double _boxPaidLeaveTotal;
        private double _boxHolidayFreeTotal;
        private double _boxHolidayTotal;
        private double _boxIllnessTotal;
        private double _boxWorkhoursTotal;
        private double _boxAvgIllnessPercantage;        
        public double BoxPaidLeaveTotal
        {
            get
            {
                return _boxPaidLeaveTotal;
            }
            set
            {
                _boxPaidLeaveTotal = value;
                NotifyOfPropertyChange(() => BoxPaidLeaveTotal);
            }
        }
        public double BoxHolidayFreeTotal
        {
            get
            {
                return _boxHolidayFreeTotal;
            }
            set
            {
                _boxHolidayFreeTotal = value;
                NotifyOfPropertyChange(() => BoxHolidayFreeTotal);
            }
        }
        public double BoxHolidayTotal
        {
            get
            {
                return _boxHolidayTotal;
            }
            set
            {
                _boxHolidayTotal = value;
                NotifyOfPropertyChange(() => BoxHolidayTotal);
            }
        }
        public double BoxIllnessTotal
        {
            get
            {
                return _boxIllnessTotal;
            }
            set
            {
                _boxIllnessTotal = value;
                NotifyOfPropertyChange(() => BoxIllnessTotal);
            }
        }
        public double BoxWorkhoursTotal
        {
            get
            {
                return _boxWorkhoursTotal;
            }
            set
            {
                _boxWorkhoursTotal = value;
                NotifyOfPropertyChange(() => BoxWorkhoursTotal);
            }
        }
        public double BoxAvgIllnessPercantage
        {
            get
            {
                return _boxAvgIllnessPercantage;
            }
            set
            {
                _boxAvgIllnessPercantage = value;
                NotifyOfPropertyChange(() => BoxAvgIllnessPercantage);
            }
        }
        public int OverallValueBoxSizes { get; set; } = 60;
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

        // The collection to display in the datagrid
        private BindableCollection<SaldoOverview> _saldoOverviewCollection = new BindableCollection<SaldoOverview>();
        public BindableCollection<SaldoOverview> SaldoOverviewCollection
        {
            get
            {
                return _saldoOverviewCollection;
            }
            set
            {
                _saldoOverviewCollection = value;
                NotifyOfPropertyChange(() => SaldoOverviewCollection);
            }
        }

        // Button method
        public void BtnCalcSaldoOverview()
        {
            CalcSaldoOverview();
        }
        public void BtnPrintPage()
        {
            Printer.PrintPdf(this);
        }

        // Display Methods
        public void CalcSaldoOverview()
        {
            //Clear the collection and dictionary to display correct data, since we increase the numbers everytime the method AddHoursToWeek is called
            SaldoOverviewCollection.Clear();

            // Clear the overall values
            BoxPaidLeaveTotal = 0;
            BoxHolidayFreeTotal = 0;
            BoxHolidayTotal = 0;
            BoxIllnessTotal = 0;
            BoxWorkhoursTotal = 0;

            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                List<Employee> employees = ctx.Employees.ToList();
                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                Calendar cal = dfi.Calendar;

                // Loop through all the employeees.
                foreach (Employee employee in employees)
                {
                    // Query for the current employees timesheetentries and vismaentries.
                    List<TimesheetEntry> timesheetEntries = ctx.TimesheetEntries.Include(k => k.vismaEntries).Where(x => x.EmployeeID == employee.Id).ToList();

                    SaldoOverview So = new SaldoOverview();
                    
                    So.EmployeeId = employee.Id;
                    So.EmployeeName = employee.Firstname + " " + employee.Surname;
                    So.PaidLeave = GetTotalValueFromVismaId(timesheetEntries, 1400, dfi, cal);
                    So.HolidayFree = GetTotalValueFromVismaId(timesheetEntries, 61, dfi, cal);
                    So.Holiday = GetTotalValueFromVismaId(timesheetEntries, 40, dfi, cal);
                    So.Illness = GetTotalValueFromVismaId(timesheetEntries, 14, dfi, cal) + GetTotalValueFromVismaId(timesheetEntries, 15, dfi, cal) + GetTotalValueFromVismaId(timesheetEntries, 13, dfi, cal);
                    So.WorkHours = GetTotalValueFromVismaId(timesheetEntries, 1100, dfi, cal);
                    So.EmployeePhonenumber = employee.PhoneNumber;
                    So.IsEmployeeFired = employee.IsFired;
                    So.PercentIllness = GetCalcPercantageTopBottom(So.Illness, So.WorkHours);

                    SaldoOverviewCollection.Add(So);

                    // Add the values to the overall total value
                    BoxPaidLeaveTotal += So.PaidLeave;
                    BoxHolidayFreeTotal += So.HolidayFree;
                    BoxHolidayTotal += So.Holiday;
                    BoxIllnessTotal += So.Illness;
                    BoxWorkhoursTotal += So.WorkHours;
                }
            }

            BoxAvgIllnessPercantage = GetCalcPercantageTopBottom(BoxIllnessTotal, BoxWorkhoursTotal);
        }

        // Returns the yearly value of the selected visma Id 
        private double GetTotalValueFromVismaId(List<TimesheetEntry> tsEntry, int vismaId, DateTimeFormatInfo dfi, Calendar cal)
        {
            double totalValue= 0;

            for (int i = 1; i <= 53; i++)
            {
                totalValue+= tsEntry.Where(x => x.Date.Year == ChosenYear).Where(x => cal.GetWeekOfYear(x.Date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == i)
                       .Sum(x => x.vismaEntries.Where(k => k.VismaID == vismaId).Sum(k => k.Value));
            }

            return totalValue;
        }

        private double GetCalcPercantageTopBottom(double top, double bottom)
        {
            if(bottom != 0)
            {
                return Math.Round((top / (bottom + top )) * 100, 1);
            } 
            else
            {
                return 0;
            }
        }
    }
}
