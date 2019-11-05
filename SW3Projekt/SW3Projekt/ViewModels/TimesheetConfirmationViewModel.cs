using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.ViewModels
{
    public class TimesheetConfirmationViewModel : Conductor<object>
    {



        public TimesheetTemplateViewModel Timesheet { get; set; }



        public BindableCollection<VismaEntrySumViewModel> VismaSumEntries { get; set; } = new BindableCollection<VismaEntrySumViewModel>();


        public BindableCollection<TimesheetEntryConfirmationViewModel> MondayEntries { get; set; } = new BindableCollection<TimesheetEntryConfirmationViewModel>();
        public BindableCollection<TimesheetEntryConfirmationViewModel> TuesdayEntries { get; set; } = new BindableCollection<TimesheetEntryConfirmationViewModel>();
        public BindableCollection<TimesheetEntryConfirmationViewModel> WednesdayEntries { get; set; } = new BindableCollection<TimesheetEntryConfirmationViewModel>();
        public BindableCollection<TimesheetEntryConfirmationViewModel> ThursdayEntries { get; set; } = new BindableCollection<TimesheetEntryConfirmationViewModel>();
        public BindableCollection<TimesheetEntryConfirmationViewModel> FridayEntries { get; set; } = new BindableCollection<TimesheetEntryConfirmationViewModel>();
        public BindableCollection<TimesheetEntryConfirmationViewModel> SaturdayEntries { get; set; } = new BindableCollection<TimesheetEntryConfirmationViewModel>();
        public BindableCollection<TimesheetEntryConfirmationViewModel> SundayEntries { get; set; } = new BindableCollection<TimesheetEntryConfirmationViewModel>();
        public string WeekBox { get; set; }
        public string YearBox { get; set; }
        public string SalaryIDBox { get; set; }


        public TimesheetConfirmationViewModel(TimesheetTemplateViewModel timesheet)
        {
            Timesheet = timesheet;

            foreach (TimesheetEntryViewModel entry in Timesheet.MondayEntries) 
            {
                MondayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry));
            }      
            
            foreach (TimesheetEntryViewModel entry in Timesheet.TuesdayEntries) 
            {
                TuesdayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry));
            }      
            
            foreach (TimesheetEntryViewModel entry in Timesheet.WednesdayEntries) 
            {
                WednesdayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry));
            }      
            
            foreach (TimesheetEntryViewModel entry in Timesheet.ThursdayEntries) 
            {
                ThursdayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry));
            }

            foreach (TimesheetEntryViewModel entry in Timesheet.FridayEntries)
            {
                FridayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry));
            }

            foreach (TimesheetEntryViewModel entry in Timesheet.SaturdayEntries) 
            {
                SaturdayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry));
            }
            
            foreach (TimesheetEntryViewModel entry in Timesheet.SundayEntries) 
            {
                SundayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry));
            }
            WeekBox = timesheet.Timesheet.WeekNumber.ToString();
            YearBox = timesheet.Timesheet.Year.ToString();
            SalaryIDBox = timesheet.Timesheet.EmployeeID.ToString();
        }

        public void BtnBack ()
        {
            removeTimesheetEntriesFromList();

            Timesheet.DeactivateItem(this, true);
        }

        public void removeTimesheetEntriesFromList()
        {
            foreach (BindableCollection<TimesheetEntryViewModel> day in Timesheet.WeekEntries)
            {
                foreach (TimesheetEntryViewModel tsentry in day)
                {
                    tsentry.TimesheetEntry.vismaEntries.Clear();
                }
            }
        }


        public void BtnConfirm()
        {
            applyRemainingRates();

            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                ctx.TimesheetEntries.AddRange(Timesheet.Timesheet.TSEntries);
                ctx.SaveChanges();
            }
            Timesheet.ShellViewModel.BtnNewTimesheet();
        }
        //diet and logi
        private void applyRemainingRates() {
            foreach (BindableCollection<TimesheetEntryViewModel> day in Timesheet.WeekEntries)
            {
                foreach (TimesheetEntryViewModel tsentry in day)
                {
                    foreach (VismaEntry vismaEntry in tsentry.TimesheetEntry.vismaEntries)
                    {
                        switch (vismaEntry.VismaID) 
                        {
                            case 1371:
                            case 1372:
                            case 1373:
                            case 1181:
                            case 9020:
                            case 9031:
                                vismaEntry.Value = vismaEntry.Value * vismaEntry.RateValue;
                                break;
                        }
                    }
                }
            }
        }

        public void BtnSum()
        {
            VismaSumEntries.Clear();

            Dictionary<int, double> sumDic = getSumDic();
            int numberOfEntries = sumDic.Count;

            var tableDic = new Dictionary<int, double>();
            int i = 0;

            foreach (KeyValuePair<int, double> pair in sumDic)
            {
                i++;
                tableDic.Add(pair.Key, pair.Value);

                if (i == 10 || pair.Key == sumDic.Last().Key)
                {
                    VismaSumEntries.Add(new VismaEntrySumViewModel(tableDic));
                    i = 0;
                    tableDic.Clear();
                } 
            }

        }


        public Dictionary<int, double> getSumDic()
        {
            var sumDic = new Dictionary<int, double>();

            foreach (BindableCollection<TimesheetEntryViewModel> day in Timesheet.WeekEntries)
            {
                foreach (TimesheetEntryViewModel tsentry in day)
                {
                    foreach (VismaEntry ventry in tsentry.TimesheetEntry.vismaEntries)
                    {
                        if (sumDic.ContainsKey(ventry.VismaID))
                        {
                            sumDic[ventry.VismaID] += ventry.Value;
                        }
                        else
                            sumDic.Add(ventry.VismaID, ventry.Value);
                    }
                }
            }

            return sumDic;
        }

    }
}
