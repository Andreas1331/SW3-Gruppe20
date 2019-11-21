using Caliburn.Micro;
using SW3Projekt.Models;
using SW3Projekt.Tools;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SW3Projekt.ViewModels
{
    public class TimesheetConfirmationViewModel : Conductor<object>
    {
        #region backingfield
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
        #endregion
        public TimesheetConfirmationViewModel(TimesheetTemplateViewModel timesheet)
        {
            Timesheet = timesheet;
            #region Add Timesheet entry view models to their collections
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
            #endregion
            PageTitle = "Bekræft Timeseddel - " + timesheet.EmployeeName;
            WeekBox = timesheet.Timesheet.WeekNumber.ToString();
            YearBox = timesheet.Timesheet.Year.ToString();
            SalaryIDBox = timesheet.Timesheet.EmployeeID.ToString();
            //Calls the sum event that updates the sum of all the visma entries.
            BtnSum();
        }
        
        public void BtnBack ()
        {
            RemoveTimesheetEntriesFromList();
            Timesheet.DeactivateItem(this, true);
        }

        public void RemoveTimesheetEntriesFromList()
        {
            //Checks every TimesheetEntry for each day and removes the vismaentries to prevent they show up twice on the next initialisation of the confirmation page.
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
            //Starts by updating the values of the rates that needs to contain a specific amount of money, like the diet which needs hours * cash pr. hour
            PrepareEntriesForDatabase();
            //adds timesheet entries and visma entries to the Database (Vismaentries are on the timesheet which is the implicit way they get added too)
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                ctx.TimesheetEntries.AddRange(Timesheet.Timesheet.TSEntries);

                // Removes references to LinkedRates in order to prevent duplication in the database.
                for (int i = 0; i < Timesheet.Timesheet.TSEntries.Count; i++)
                {
                    for (int j = 0; j < Timesheet.Timesheet.TSEntries[i].vismaEntries.Count; j++)
                    {
                        if (Timesheet.Timesheet.TSEntries[i].vismaEntries[j].LinkedRate != null)
                        {
                            ctx.Entry(Timesheet.Timesheet.TSEntries[i].vismaEntries[j].LinkedRate).State = EntityState.Detached;
                        }
                    }
                }

                ctx.SaveChanges();
            }

            string caption = "Succes";
            string message = "Timesedlen blev gemt.";
            MessageBoxButtons buttons = MessageBoxButtons.OK;

            System.Windows.Forms.MessageBox.Show(message, caption, buttons);
            //calls the method that changes the page to a new timesheet.
            Timesheet.ShellViewModel.BtnNewTimesheet();
        }

        private void PrepareEntriesForDatabase() 
        {
            //checks every timesheet entry in every day and calculates the new value and updates it
            foreach (BindableCollection<TimesheetEntryViewModel> day in Timesheet.WeekEntries)
            {
                foreach (TimesheetEntryViewModel tsentry in day)
                {
                    Calculator.ApplyRemainingRates(tsentry.TimesheetEntry.vismaEntries);

                    // Removes the references to LinkedRates to prevent duplication in the database.
                    // tsentry.TimesheetEntry.vismaEntries.ForEach(vsentry => vsentry.LinkedRate = null);
                }
            }
        }

        public void BtnSum()
        {
            //clears the collection to prevent that an entry appears multiple times in the calculations of the sum
            VismaSumEntries.Clear();
            //calls GetSumDic to get a sorted dictionary which contains the sum of values for each visma id.
            SortedDictionary<int, double> sumDic = GetSumDic();

            var tableDic = new Dictionary<int, double>();
            int i = 0;

            //calls a method to add textboxes which contains the pairs of visma ids and their sum values
            //and for every 6 unique visma ids it makes a lineshift.
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
            //checks every vismaentry for its value and id.
            foreach (BindableCollection<TimesheetEntryViewModel> day in Timesheet.WeekEntries)
            {
                foreach (TimesheetEntryViewModel tsentry in day)
                {
                    foreach (VismaEntry ventry in tsentry.TimesheetEntry.vismaEntries)
                    {
                        //if the dictionary already contains the key it adds the value to the key, else it adds a new entry to the dictionary with the id and value
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
