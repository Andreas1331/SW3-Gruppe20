﻿using Caliburn.Micro;
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

        const float Base60to100Constant = 5 / (3 * 100);


        //Send hele timesheetet over i stedet så vi kan finde alle timseheet entriesne.
        public static void AddVismaEntries(Timesheet timesheet)
        {
            List<Rate> rates = GetRates();

                foreach (TimesheetEntry tsentry in timesheet.TSEntries)
                {
                //Tilføj alle relevante vismaentries til den enkelte tsentry
                    foreach (Rate rate in rates)
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

            if ((rate.DaysPeriod & ((Days)Math.Pow(2, (int)entry.Date.DayOfWeek))) > 0) /*Tjek om dagen er gyldig for raten*/
            {
                if (rate.StartTime != 0 && rate.EndTime!= 0/*Tjek om raten drejer sig om arbejdstid*/)
                {
                    if (rate.StartTime <= entry.EndTime && rate.EndTime >= entry.StartTime) 
                    {
                        ApplyHourlyRate(entry, rate);
                    }
                }
                //else if kæde (ratetypelist.Contains(rate.densid) then apply(denhersensrate)
            }
        }

        private static void ApplyHourlyRate(TimesheetEntry entry, Rate rate)
        {
            VismaEntry vismaEntry = new VismaEntry();
            vismaEntry.VismaID = rate.VismaID;
            vismaEntry.RateID = rate.Id;
            vismaEntry.RateValue = (float)rate.RateValue;
            vismaEntry.TimesheetEntryID = entry.Id;
            float numberOfWholeHours =  (float)Math.Floor((double)(Math.Min(entry.EndTime, rate.EndTime) - Math.Max(entry.StartTime, rate.StartTime))/100);
            vismaEntry.Value = ((Math.Min(entry.EndTime, rate.EndTime) - Math.Max(entry.StartTime, rate.StartTime)) - (numberOfWholeHours * 100))*Base60to100Constant /* divides by 3 times 5 divided by 100 to get the decimal amount of hours*/ + numberOfWholeHours;
            entry.vismaEntries.Add(vismaEntry);
        }
    }
}
