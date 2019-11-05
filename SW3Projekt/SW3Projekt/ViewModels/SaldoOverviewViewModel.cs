﻿using Caliburn.Micro;
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
    public class SaldoOverviewViewModel : Screen
    {
        public double BoxPaidLeaveTotal { get; set; }
        public double BoxHolidayFreeTotal { get; set; }
        public double BoxHolidayTotal { get; set; }
        public double BoxIllnessTotal { get; set; }
        public double BoxWorkhoursTotal { get; set; }
        public double BoxAvgIllnessPercantage { get; set; }
        public int OverallValueBoxSizes { get; set; } = 80;


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

        // Display Methods
        public SaldoOverviewViewModel()
        {
            //Clear the dictionary to display correct data, since we increase the numbers everytime the method AddHoursToWeek is called
            SaldoOverviewCollection.Clear();

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
                    So.Illness = GetTotalValueFromVismaId(timesheetEntries, 14, dfi, cal);
                    So.WorkHours = GetTotalValueFromVismaId(timesheetEntries, 1100, dfi, cal);
                    So.EmployeePhonenumber = employee.PhoneNumber;
                    So.IsEmployeeFired = employee.IsFired;
                    So.PercentIllness = CalcPercantageTopBottom(So.Illness, So.WorkHours);

                    SaldoOverviewCollection.Add(So);

                    // Add the values to the overall total value
                    BoxPaidLeaveTotal += So.PaidLeave;
                    BoxHolidayFreeTotal += So.HolidayFree;
                    BoxHolidayTotal += So.Holiday;
                    BoxIllnessTotal += So.Illness;
                    BoxWorkhoursTotal += So.WorkHours;

                    
                } 
            }

            BoxAvgIllnessPercantage = CalcPercantageTopBottom(BoxIllnessTotal, BoxWorkhoursTotal);
        }

        // Returns the weekly value of the selected Visma Id
        private double GetWeeklyValueFromVismaId(List<TimesheetEntry> tsEntry, int vismaId, DateTimeFormatInfo dfi, Calendar cal, int chosenWeekNumber)
        {
            {
                return tsEntry.Where(x => cal.GetWeekOfYear(x.Date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == chosenWeekNumber)
                       .Sum(x => x.vismaEntries.Where(k => k.VismaID == vismaId).Sum(k => k.Value));
            }
        }

        // Returns the yearly value of the selected visma Id 
        private double GetTotalValueFromVismaId(List<TimesheetEntry> tsEntry, int vismaId, DateTimeFormatInfo dfi, Calendar cal)
        {
            double totalValue= 0;

            for (int i = 1; i <= 53; i++)
            {
                totalValue+= tsEntry.Where(x => cal.GetWeekOfYear(x.Date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) == i)
                       .Sum(x => x.vismaEntries.Where(k => k.VismaID == vismaId).Sum(k => k.Value));
            }

            return totalValue;
        }

        private double CalcPercantageTopBottom(double top, double bottom)
        {
            if(bottom != 0)
            {
                return Math.Round((top / bottom) * 100, 1);
            } 
            else
            {
                return 0;
            }
        }
    }
}
