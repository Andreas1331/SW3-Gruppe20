using Caliburn.Micro;
using SW3Projekt.Models;
using SW3Projekt.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SW3Projekt.Models.Rate;

namespace SW3Projekt.Tools
{
    static class Calculator
    {

        public static void AddVismaEntries(Timesheet timesheet)
        {
            foreach (TimesheetEntry tsentry in timesheet.TSEntries)
            {
            //For every rate it calls the method to check if the rate is applicable
                foreach (Rate rate in timesheet.rates)
                {
                    IsRateApplicable(tsentry, rate);
                }
            }
        }

        // Spørg, om man kan hente gældende rates på en nemmere måde.
        public static List<Rate> GetRates()
        {
            List<Rate> returnList = new List<Rate>();
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                var activeAgreement = ctx.CollectiveAgreements.FirstOrDefault(agreement => agreement.IsActive);
                returnList = ctx.Rates.Where(rate => rate.CollectiveAgreementID == activeAgreement.Id).ToList();
            }
            return returnList;
        }

        private static void IsRateApplicable(TimesheetEntry entry, Rate rate)
        {
            // Alle informationerne fra alle felterne er gemt i hvert entry. så i kan finde de informationer i skal bruge 
            // (i skal selv konvertere fra string til datetime)


            //Check weither the rate is about illness and that was illness was chosen in timesheetEntry
            if (rate.Name.ToLower().Contains("syg"))
            {
                if (entry.SelectedTypeComboBoxItem == rate.Name)
                {
                    ApplyHourlyRate(entry, rate);
                }
            }
            //Checks weither the rate is about holidays, and then figures out if the holiday was chosen for the timesheetEntry. 
            else if (rate.Name.ToLower().Contains("ferie"))
            {
                if (entry.SelectedTypeComboBoxItem == rate.Name)
                {
                    //Then it checks weither the rate is holidays in a full day (ferie) or hours (feriefri).
                    if (rate.VismaID == 40)
                    {
                        ApplyDailyRate(entry, rate);
                    }
                    else if (rate.VismaID == 61)
                    {
                        ApplyHourlyRate(entry, rate);
                    }
                }
            }
            // Checks weither the rate is about driving.
            else if (rate.VismaID == 9010)
            {
                //if the timesheetEntry contains a destination it then adds the vismaentry for driving.
                if (entry.SelectedRouteComboBoxItem != null) {
                    ApplyDriveRate(entry, rate);
                }
            }
            // Checks weither the rate is about public holidays
            else if (rate.Name.ToLower().Contains("sh-dage"))
            {
                if (entry.SelectedTypeComboBoxItem == rate.Name)
                {
                        ApplyDailyRate(entry, rate);
                }
            }
            // Checks weither the rate is about paid leave. If so it just returns since the user will control this manualy
            else if (rate.Name.ToLower().Contains("afspadsering")) {
                return;
            }
            // Checks for rates about work, first it checks if the day is within the range the rate covers. 
            else if ((rate.DaysPeriod & ((Days)Math.Pow(2, (int)entry.Date.DayOfWeek))) > 0)
            {
                //Then it makes sure that if the following options were selected on the timesheetEntry to prevent them from for example gaining "work" rates while on holidays
                if (entry.SelectedTypeComboBoxItem.ToLower().Contains("syg") || entry.SelectedTypeComboBoxItem.ToLower().Contains("ferie") || entry.SelectedTypeComboBoxItem.ToLower().Contains("sh"))
                    return;
                //then it makes sure that the rate does contain a timespan (if both are 0 there is no timespan)
                if (rate.StartTime != new DateTime() || rate.EndTime != new DateTime()/*Tjek om raten drejer sig om arbejdstid*/)
                {
                    //Lastly it checks weither the entry does contain an amount of time within the timespan. Before applying the rate.
                    if (rate.StartTime <= entry.EndTime && rate.EndTime >= entry.StartTime)
                    {
                        ApplyHourlyRate(entry, rate);
                    }
                }
            }
        }

        private static void ApplyHourlyRate(TimesheetEntry entry, Rate rate)
        {
            //First a vismaEntry is created with values from the timesheetEntry and the rate.
            VismaEntry vismaEntry = new VismaEntry
            {
                VismaID = rate.VismaID,
                RateID = rate.Id,
                RateValue = rate.RateValue,
                TimesheetEntryID = entry.Id
            };
            //then it finds the amount of time within the hourly rate. by first checking which is larger, the start time of the rate or the entry
            DateTime startTime = entry.StartTime > rate.StartTime ? entry.StartTime : rate.StartTime;
            //then it checks which is smaller, the end time of the entry or the rate
            DateTime endTime = entry.EndTime < rate.EndTime ? entry.EndTime : rate.EndTime;
            //finally it calculates the timespan
            TimeSpan interval = endTime - startTime;

            vismaEntry.Value = interval.TotalHours;

            //Breaktime is applied to normal work hours (with visma ID = 1100).
            if (rate.VismaID == 1100)
            {
                vismaEntry.Value -= entry.BreakTime;
            }
            //finally for a failsafe a check for the value of the new vismaEntry is done, to check if any hours were in fact in the timespan. Before adding it to the timesheetEntry.
            if (vismaEntry.Value > 0)
            {
                entry.vismaEntries.Add(vismaEntry);
            }
        }

        private static void ApplyDailyRate(TimesheetEntry entry, Rate rate) 
        {
            VismaEntry vismaEntry = new VismaEntry
            {
                VismaID = rate.VismaID,
                RateID = rate.Id,
                RateValue = rate.RateValue,
                TimesheetEntryID = entry.Id,
                Value = 1
            };
            entry.vismaEntries.Add(vismaEntry);
        }

        private static void ApplyDriveRate(TimesheetEntry entry, Rate rate) 
        {
            VismaEntry vismaEntry = new VismaEntry
            {
                VismaID = rate.VismaID,
                RateID = rate.Id,
                RateValue = entry.DriveRate,
                TimesheetEntryID = entry.Id,
                Value = entry.DriveRate * entry.KmTextBox,
                Comment = "Kørsel " + entry.SelectedRouteComboBoxItem
            };
            entry.vismaEntries.Add(vismaEntry);
        }

        //to apply the correct values for the rates in which the vismaEntry needs to contain an amount of money instead of amount of hours
        //a check for the vismaID is done. After the check for the correct ID it calculates and saves the new value based on the ratevalue and the value in the vismaEntry.
        public static void ApplyRemainingRates(List<VismaEntry> vismaEntries)
        {
            foreach (VismaEntry vismaEntry in vismaEntries)
            {
                //if (vismaEntry.LinkedRate.SaveAsMoney)
                {
                    vismaEntry.Value *= vismaEntry.RateValue;
                }
            }
        }

}
}
