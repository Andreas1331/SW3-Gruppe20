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


        //Send hele timesheetet over i stedet så vi kan finde alle timseheet entriesne.
        public static void AddVismaEntries(Timesheet timesheet)
        {
            foreach (TimesheetEntry tsentry in timesheet.TSEntries)
            {
            //Tilføj alle relevante vismaentries til den enkelte tsentry
                foreach (Rate rate in timesheet.rates)
                {
                //Apply rate
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


            //Sygdom
            if (rate.Name.ToLower().Contains("syg"))
            {
                if (entry.SelectedTypeComboBoxItem == rate.Name)
                {
                    ApplyHourlyRate(entry, rate);
                }
            }
            //Ferie
            else if (rate.Name.ToLower().Contains("ferie"))
            {
                if (entry.SelectedTypeComboBoxItem == rate.Name)
                {
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
            //SH-Dage
            else if (rate.Name.ToLower().Contains("sh-dage"))
            {
                if (entry.SelectedTypeComboBoxItem == rate.Name)
                {
                    if (rate.VismaID == 6510)
                    {
                        ApplyDailyRate(entry, rate);
                    }
                }
            }
            else if (rate.Name.ToLower().Contains("afspadsering")) {
                return;
            }
            //Arbejde
            else if ((rate.DaysPeriod & ((Days)Math.Pow(2, (int)entry.Date.DayOfWeek))) > 0) /*Tjek om dagen er gyldig for raten*/
            {
                if (entry.SelectedTypeComboBoxItem.ToLower().Contains("syg") || entry.SelectedTypeComboBoxItem.ToLower().Contains("ferie") || entry.SelectedTypeComboBoxItem.ToLower().Contains("sh"))
                    return;

                if (rate.StartTime != new DateTime() || rate.EndTime != new DateTime()/*Tjek om raten drejer sig om arbejdstid*/)
                {
                    if (rate.StartTime <= entry.EndTime && rate.EndTime >= entry.StartTime)
                    {
                        ApplyHourlyRate(entry, rate);
                    }
                }
            }
        }

        private static void ApplyHourlyRate(TimesheetEntry entry, Rate rate)
        {
            VismaEntry vismaEntry = new VismaEntry();
            vismaEntry.VismaID = rate.VismaID;
            vismaEntry.RateID = rate.Id;
            vismaEntry.RateValue = rate.RateValue;
            vismaEntry.TimesheetEntryID = entry.Id;

            DateTime startTime = entry.StartTime > rate.StartTime ? entry.StartTime : rate.StartTime;
            DateTime endTime = entry.EndTime < rate.EndTime ? entry.EndTime : rate.EndTime;
            TimeSpan interval = endTime - startTime;

            vismaEntry.Value = interval.TotalHours;

            //Breaktime is applied to normal work hours (with visma ID = 1100).
            if (rate.VismaID == 1100)
            {
                vismaEntry.Value -= entry.BreakTime;
            }
            if (vismaEntry.Value > 0)
            {
                entry.vismaEntries.Add(vismaEntry);
            }
        }

        private static void ApplyDailyRate(TimesheetEntry entry, Rate rate) 
        {
            VismaEntry vismaEntry = new VismaEntry();
            vismaEntry.VismaID = rate.VismaID;
            vismaEntry.RateID = rate.Id;
            vismaEntry.RateValue = (float)rate.RateValue;
            vismaEntry.TimesheetEntryID = entry.Id;
            vismaEntry.Value = 1;
            entry.vismaEntries.Add(vismaEntry);
        }

    }
}
