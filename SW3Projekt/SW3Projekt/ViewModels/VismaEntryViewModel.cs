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

        public List<string> rateNames = new List<string>();
        public ObservableCollection<ComboBoxItem> RateNamesCombobox { get; set; } = new ObservableCollection<ComboBoxItem>();
        public ComboBoxItem SelectedRate { get; set; }


        public VismaEntryViewModel(VismaEntry entry, TimesheetEntryConfirmationViewModel timesheetEntry)
        {
            Entry = entry;
            TimesheetEntry = timesheetEntry;

            rateNames = TimesheetEntry.Tsentry.timesheet.rates.Select(rate => rate.Name).ToList();
            foreach (string name in rateNames) {
                RateNamesCombobox.Add(new ComboBoxItem() { Content = name });
            }
            string raten = TimesheetEntry.Tsentry.timesheet.rates
                            .Where(rate => rate.Id == Entry.RateID)
                            .Select(rate => rate.Name).FirstOrDefault();

            SelectedRate = RateNamesCombobox.Where(name => (string)name.Content == raten).FirstOrDefault();
            
        }

        public void OnSelected(object sender, SelectionChangedEventArgs e) { 
            ComboBoxItem selecteditem = sender as ComboBoxItem;

            Rate rate = TimesheetEntry.Tsentry.timesheet.rates
                            .Where(r => r.Name == (string)selecteditem.Content).FirstOrDefault();

            VismaIdBox = rate.VismaID;
            Entry.RateID = rate.Id;
            RateValueBox = (float) rate.RateValue;
        }

        public void BtnRemoveVismaEntry()
        {
            TimesheetEntry.Tsentry.vismaEntries.Remove(Entry);
            TimesheetEntry.RemoveEntry(this);
        }


    }
}
