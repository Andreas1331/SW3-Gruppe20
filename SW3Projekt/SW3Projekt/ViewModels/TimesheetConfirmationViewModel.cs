using Caliburn.Micro;
using SW3Projekt.Models;
using SW3Projekt.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private string _pagetitle;
        public string PageTitle
        {
            get
            {
                return _pagetitle;
            }
            set
            {
                _pagetitle = value;
                NotifyOfPropertyChange(() => PageTitle);
            }
        }

        public TimesheetConfirmationViewModel(TimesheetTemplateViewModel timesheet)
        {
            Timesheet = timesheet;

            foreach (TimesheetEntryViewModel entry in Timesheet.MondayEntries) 
            {
                MondayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry, this));
            }      
            
            foreach (TimesheetEntryViewModel entry in Timesheet.TuesdayEntries) 
            {
                TuesdayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry, this));
            }      
            
            foreach (TimesheetEntryViewModel entry in Timesheet.WednesdayEntries) 
            {
                WednesdayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry, this));
            }      
            
            foreach (TimesheetEntryViewModel entry in Timesheet.ThursdayEntries) 
            {
                ThursdayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry, this));
            }

            foreach (TimesheetEntryViewModel entry in Timesheet.FridayEntries)
            {
                FridayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry, this));
            }

            foreach (TimesheetEntryViewModel entry in Timesheet.SaturdayEntries) 
            {
                SaturdayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry, this));
            }
            
            foreach (TimesheetEntryViewModel entry in Timesheet.SundayEntries) 
            {
                SundayEntries.Add(new TimesheetEntryConfirmationViewModel(entry.TimesheetEntry, this));
            }
            PageTitle = "Bekræft Timeseddel - " + timesheet.EmployeeName;
            WeekBox = timesheet.Timesheet.WeekNumber.ToString();
            YearBox = timesheet.Timesheet.Year.ToString();
            SalaryIDBox = timesheet.Timesheet.EmployeeID.ToString();
            BtnSum();
        }

        public void BtnBack ()
        {
            RemoveTimesheetEntriesFromList();

            Timesheet.DeactivateItem(this, true);
        }

        public void RemoveTimesheetEntriesFromList()
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
            ApplyRemainingRates();

            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                ctx.TimesheetEntries.AddRange(Timesheet.Timesheet.TSEntries);
                ctx.SaveChanges();
            }

            string caption = "Succes";
            string message = "Timesedlen blev gemt.";
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            System.Windows.Forms.MessageBox.Show(message, caption, buttons);

            Timesheet.ShellViewModel.BtnNewTimesheet();
        }
        //diet and logi
        private void ApplyRemainingRates() 
        {
            foreach (BindableCollection<TimesheetEntryViewModel> day in Timesheet.WeekEntries)
            {
                foreach (TimesheetEntryViewModel tsentry in day)
                {
                    Calculator.ApplyRemainingRates(tsentry.TimesheetEntry.vismaEntries);
                }
            }
        }

        public void BtnSum()
        {
            VismaSumEntries.Clear();

            SortedDictionary<int, double> sumDic = GetSumDic();

            var tableDic = new Dictionary<int, double>();
            int i = 0;

            foreach (KeyValuePair<int, double> pair in sumDic)
            {
                i++;
                tableDic.Add(pair.Key, pair.Value);

                if (i == 6 || pair.Key == sumDic.Last().Key)
                {
                    VismaSumEntries.Add(new VismaEntrySumViewModel(tableDic));
                    i = 0;
                    tableDic.Clear();
                } 
            }

        }

        public SortedDictionary<int, double> GetSumDic()
        {
            SortedDictionary<int, double> sortedSumDic = new SortedDictionary<int, double>();

            foreach (BindableCollection<TimesheetEntryViewModel> day in Timesheet.WeekEntries)
            {
                foreach (TimesheetEntryViewModel tsentry in day)
                {
                    foreach (VismaEntry ventry in tsentry.TimesheetEntry.vismaEntries)
                    {
                        if (sortedSumDic.ContainsKey(ventry.VismaID))
                        {
                            sortedSumDic[ventry.VismaID] += ventry.Value;
                        }
                        else
                            sortedSumDic.Add(ventry.VismaID, ventry.Value);
                    }
                }
            }
            return sortedSumDic;
        }
    }
}
