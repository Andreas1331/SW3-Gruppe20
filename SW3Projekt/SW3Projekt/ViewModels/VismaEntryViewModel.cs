using Caliburn.Micro;
using SW3Projekt.Models;
using System.Collections.Generic;
using System.Windows.Controls;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using SW3Projekt.Tools;
using System.Collections.ObjectModel;
using System.Windows;

namespace SW3Projekt.ViewModels
{
    public class VismaEntryViewModel : Screen
    {
        #region Properties
        public VismaEntry Entry { get; set; }
        public int VismaIdBox
        {
            get
            {
                return Entry.VismaID;
            }
            set
            {
                Entry.VismaID = value;
                NotifyOfPropertyChange(() => VismaIdBox);
            }
        }  
        public float ValueBox
        {
            get
            {
                return (float)Entry.Value;
            }
            set
            {
                Entry.Value = value;
                TimesheetConfirmationViewModel.BtnSum();
                NotifyOfPropertyChange(() => ValueBox);
            }
        }
        public float RateValueBox
        {
            get
            {
                return (float)Entry.RateValue;
            }
            set
            {
                Entry.RateValue = value;
                NotifyOfPropertyChange(() => RateValueBox);
            }
        }
        public string CommentBox
        {
            get
            {
                return Entry.Comment;
            }
            set
            {
                Entry.Comment = value;
                NotifyOfPropertyChange(() => CommentBox);
            }
        }

        public TimesheetEntryConfirmationViewModel TimesheetEntry { get; set; }
        public TimesheetConfirmationViewModel TimesheetConfirmationViewModel { get; set; }

        public List<string> rateNames = new List<string>();
        public ObservableCollection<ComboBoxItem> RateNamesCombobox { get; set; } = new ObservableCollection<ComboBoxItem>();
        public ComboBoxItem SelectedRate { get; set; }
        #endregion

        public VismaEntryViewModel(VismaEntry entry, TimesheetEntryConfirmationViewModel timesheetEntry, TimesheetConfirmationViewModel TsConfirmationViewModel)
        {
            TimesheetConfirmationViewModel = TsConfirmationViewModel;
            Entry = entry;
            TimesheetEntry = timesheetEntry;
            //Saves all the rates' names to a list and then adds them as items in the combobox.
            rateNames = TimesheetEntry.Tsentry.timesheet.rates.Select(rate => rate.Name).ToList();
            foreach (string name in rateNames) {
                RateNamesCombobox.Add(new ComboBoxItem() { Content = name });
            }
            //then it finds the rate which was added to then select the correct rate to show.
            string raten = TimesheetEntry.Tsentry.timesheet.rates
                            .Where(rate => rate.Id == Entry.RateID)
                            .Select(rate => rate.Name).FirstOrDefault();

            SelectedRate = RateNamesCombobox.Where(name => (string)name.Content == raten).FirstOrDefault();
        }

        //when a rate is selected in the combobox it first finds the rate matching the name, then adds the rate's id to the idbox and 
        //the rateId to the vismaEntry (for the databse) itself and finally it adds the rate's ratevalue to the RateValueTextbox
        public void OnSelected(object sender, SelectionChangedEventArgs e) { 
            ComboBoxItem selecteditem = sender as ComboBoxItem;

            Rate rate = TimesheetEntry.Tsentry.timesheet.rates
                            .Where(r => r.Name == (string)selecteditem.Content).FirstOrDefault();

            VismaIdBox = rate.VismaID;
            Entry.RateID = rate.Id;
            Entry.LinkedRate = rate;
            RateValueBox = (float) rate.RateValue;
        }

        public void BtnRemoveVismaEntry()
        {
            //To remove the vismaEntry it first needs to remove itself from the timesheetEntry (that gets added to the databse)
            TimesheetEntry.Tsentry.vismaEntries.Remove(Entry);
            //Then update the sums by calling the sum method
            TimesheetConfirmationViewModel.BtnSum();
            //and finally calling the remove method from the timesheetViewModel to remove itself from the page
            TimesheetEntry.RemoveEntry(this);
        }
    }
}
