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
    public static class Calculator
    {

        public static void AddVismaEntries(Timesheet timesheet)
        {
            foreach (TimesheetEntry tsentry in timesheet.TSEntries)
            {
            //For every rate it calls the method to check if the rate is applicable
                foreach (Rate rate in timesheet.rates)
                {
                    if (rate.Type != "Hidden")
                    {
                        IsRateApplicable(tsentry, rate);
                    }
                    if (rate.Name == "Normal")
                    {
                        if (tsentry.vismaEntries.Last().LinkedRate.Name == "Normal")
                        {
                            timesheet.TotalNormalHours += tsentry.vismaEntries.Last().Value;
                        }
                    }
                }
            }
        }

        // Rates are retrieved from the database by querying the active agreement's Rates list.
        public static List<Rate> GetRates()
        {
            List<Rate> returnList = new List<Rate>();

            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                var activeAgreement = ctx.CollectiveAgreements
                                         .Include("Rates")
                                         .FirstOrDefault(agreement => agreement.IsActive);

                returnList = activeAgreement.Rates;
            }

            return returnList;
        }



        private static void IsRateApplicable(TimesheetEntry entry, Rate rate)
        {
            // Check for match between rate type and selected item along with a days check.
            if (TypesCompatible(entry, rate) && DaysApply(rate.DaysPeriod,(int)entry.Date.DayOfWeek))
            {
                // Hourly or daily rate check.
                if (rate.StartTime != rate.EndTime)
                {
                    CheckAndApplyHourlyRate(entry, rate);
                }
                else
                {
                    ApplyDailyRate(entry, rate);              
                }
            }
            // Check if the entry has a route selected, and whether the rate is the drive rate.
            else if (!string.IsNullOrEmpty(entry.SelectedRouteComboBoxItem) && rate.Name == "Kørsel")
            {
                ApplyDriveRate(entry, rate);
            }
        }

        // Returns true when there is an overlap between the Days bit vector of the rate and the bit value of the entry day. 
        private static bool DaysApply(Days daysPeriod, int entryDay)
        {
            return (daysPeriod & ((Days)Math.Pow(2, entryDay))) > 0;
        }

        private static bool TypesCompatible(TimesheetEntry entry, Rate rate)
        {
            return entry.SelectedTypeComboBoxItem == rate.Type || (entry.SelectedTypeComboBoxItem == "Forskudttid" && rate.Name == "Normal");
        }


        private static void CheckAndApplyHourlyRate(TimesheetEntry entry, Rate rate)
        {
            if (rate.StartTime <= entry.EndTime && rate.EndTime >= entry.StartTime)
            {
                ApplyHourlyRate(entry, rate);
            }
        }

        private static void ApplyHourlyRate(TimesheetEntry entry, Rate rate)
        {
            //First a vismaEntry is created with values from the TimesheetEntry and the Rate.
            VismaEntry vismaEntry = new VismaEntry
            {
                VismaID = rate.VismaID,
                RateID = rate.Id,
                RateValue = rate.RateValue,
                TimesheetEntryID = entry.Id,
                LinkedRate = rate
            };
            
            // Then it finds the amount of time within the hourly rate by first checking which is larger, the start time of the rate or the entry.
            DateTime startTime = entry.StartTime > rate.StartTime ? entry.StartTime : rate.StartTime;
            
            // Then it checks which is smaller, the end time of the entry or the rate.
            DateTime endTime = entry.EndTime < rate.EndTime ? entry.EndTime : rate.EndTime;
           
            // Finally it calculates the timespan.
            TimeSpan interval = endTime - startTime;

            // Only the nearest quarter value is needed for precision.
            vismaEntry.Value = RoundToNearestQuarter(interval.TotalHours);

            //Breaktime is subtracted from normal work hours.
            if (rate.Name == "Normal")
            {
                vismaEntry.Value -= entry.BreakTime;
            }

            // Finally for a failsafe a check for the value of the new vismaEntry is done, to check if any hours were in fact in the timespan. Before adding it to the timesheetEntry.
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
                LinkedRate = rate,
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
                Value = entry.KrTextBox / entry.DriveRate,
                LinkedRate = rate,
                Comment = "Km. " + entry.SelectedRouteComboBoxItem
            };
            entry.vismaEntries.Add(vismaEntry);
        }

        //to apply the correct values for the rates in which the vismaEntry needs to contain an amount of money instead of amount of hours
        //a check for the vismaID is done. After the check for the correct ID it calculates and saves the new value based on the ratevalue and the value in the vismaEntry.
        public static void ApplyRemainingRates(TimesheetEntry TSentry)
        {
            for (int i = 0; i < TSentry.vismaEntries.Count; i++)
            {
                if (TSentry.vismaEntries[i].LinkedRate.Type == "Ferie")
                {
                    TSentry.vismaEntries.Add(new VismaEntry() { VismaID = TSentry.timesheet.rates.FirstOrDefault(x => x.Type == "Hidden").VismaID, 
                                                                Value = -1 * TSentry.vismaEntries[i].Value, 
                                                                TimesheetEntryID = TSentry.vismaEntries[i].TimesheetEntryID, 
                                                                LinkedRate = TSentry.timesheet.rates.FirstOrDefault(x => x.Type == "Hidden"), 
                                                                RateID = TSentry.timesheet.rates.FirstOrDefault(x => x.Type == "Hidden").Id});
                }
                if (TSentry.vismaEntries[i].LinkedRate.Name == "Afspadsering (ind)")
                {
                    TSentry.vismaEntries.Add(new VismaEntry() { VismaID = TSentry.timesheet.rates.FirstOrDefault(x => x.Name == "Normal").VismaID, 
                                                                Value = -1 * TSentry.vismaEntries[i].Value, 
                                                                TimesheetEntryID = TSentry.vismaEntries[i].TimesheetEntryID, 
                                                                LinkedRate = TSentry.timesheet.rates.FirstOrDefault(x => x.Name == "Normal"), 
                                                                RateID = TSentry.timesheet.rates.FirstOrDefault(x => x.Name == "Normal").Id });
                }
                if (TSentry.vismaEntries[i].LinkedRate.SaveAsMoney)
                {
                    TSentry.vismaEntries[i].Value *= TSentry.vismaEntries[i].RateValue;
                }
                TSentry.vismaEntries[i].Value = Math.Round(TSentry.vismaEntries[i].Value, 2);
            }
        }

        public static double RoundToNearestQuarter(double num)
        {
            double absNum = Math.Abs(num);
            double decimalPart = absNum - Math.Floor(absNum);
            double integralPart = Math.Floor(absNum);
            double modifier;

            if (decimalPart < 0.125)
            {
                modifier = 0.0;
            }
            else if (decimalPart < 0.375)
            {
                modifier = 0.25;
            }
            else if (decimalPart < 0.625)
            {
                modifier = 0.50;
            }
            else if (decimalPart < 0.875)
            {
                modifier = 0.75;
            }
            else
            {
                modifier = 1.0;
            }

            return (num < 0 ? -1 : 1) * (integralPart + modifier);
        }

}
}
