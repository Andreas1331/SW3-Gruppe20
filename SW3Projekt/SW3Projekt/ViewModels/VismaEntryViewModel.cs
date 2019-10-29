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
                return Entry.Value;
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
                return Entry.RateValue;
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
        public ComboBoxItem selectedRate { get; set; }


        public VismaEntryViewModel(VismaEntry entry, TimesheetEntryConfirmationViewModel timesheetEntry)
        {
            Entry = entry;
            TimesheetEntry = timesheetEntry;
            rateNames = TimesheetEntry.tsentry.timesheet.rates.Select(rate => rate.Name).ToList();
            foreach (string name in rateNames) {
                RateNamesCombobox.Add(new ComboBoxItem() { Content = name });
            }
            string raten = TimesheetEntry.tsentry.timesheet.rates
                            .Where(rate => rate.VismaID == Entry.VismaID)
                            .Select(rate => rate.Name).FirstOrDefault();

            selectedRate = RateNamesCombobox.Where(name => (string)name.Content == raten).FirstOrDefault();
            
        }

        public void OnSelected(object sender, SelectionChangedEventArgs e) { 
            ComboBoxItem selecteditem = sender as ComboBoxItem;
            VismaIdBox = TimesheetEntry.tsentry.timesheet.rates
                            .Where(rate => rate.Name == (string)selecteditem.Content)
                            .Select(rate => rate.VismaID).FirstOrDefault();
            Console.WriteLine(VismaIdBox);
        }

        public void BtnRemoveVismaEntry()
        {
            TimesheetEntry.RemoveEntry(this);
        }


    }
}
