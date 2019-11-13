using Caliburn.Micro;
using SW3Projekt.Models;
using SW3Projekt.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.ViewModels
{
    public class TimesheetEntryConfirmationViewModel : Conductor<object>
    {
        #region backingfield
        public string TypeText { get; set; }
        public string ProjectIdText { get; set; }
        public string StartTimeText { get; set; }
        public string EndTimeText { get; set; }
        public string DrivingText { get; set; }

        public BindableCollection<VismaEntryViewModel> VismaEntries { get; set; } = new BindableCollection<VismaEntryViewModel>();

        public List<Rate> Rates = Calculator.GetRates();
        public TimesheetEntry Tsentry { get; set; }
        TimesheetConfirmationViewModel TsConfirmationViewModel;
        #endregion
        public TimesheetEntryConfirmationViewModel(TimesheetEntry entry, TimesheetConfirmationViewModel TsConfirmationViewModel)
        {
            this.TsConfirmationViewModel = TsConfirmationViewModel;
            TypeText = "Type: " + entry.SelectedTypeComboBoxItem;
            ProjectIdText = "Projekt-ID: " + entry.ProjectID;
            //to prevent the start and end times to contain seconds we restrict the substring to 5 symbols (2 for hours, 1 for the dot, and 2 for the minutes)
            StartTimeText = "Start: " + entry.StartTime.TimeOfDay.ToString().Substring(0, 5);
            EndTimeText = "Slut: "  + entry.EndTime.TimeOfDay.ToString().Substring(0, 5);
            DrivingText = "Kørsel: " + entry.SelectedRouteComboBoxItem;
            Tsentry = entry;

            foreach (VismaEntry visma in entry.vismaEntries)
            {
                VismaEntries.Add(new VismaEntryViewModel(visma, this, TsConfirmationViewModel));
            }
        }

        public void BtnAddVismaEntry()
        {
            //first it adds a new Vismaentry Viewmodel (and thereby its view) to the list where it contains what to show on the page
            VismaEntries.Add(new VismaEntryViewModel(new VismaEntry(), this, TsConfirmationViewModel));
            //then it adds the the specific vismaentry to the list of vismaentries it contains on the timesheetentry instance itself.
            Tsentry.vismaEntries.Add(VismaEntries.Last().Entry);
            //and finally it adds a reference to the timesheetEntry the vismaEntry is on to the vismaEntry
            VismaEntries.Last().Entry.TimesheetEntryID = Tsentry.Id;
        }

        //this method is for the visma entries to call, so that they can remove themselves from the TimesheetEntry
        public void RemoveEntry(VismaEntryViewModel entry)
        {
            VismaEntries.Remove(entry);
        }
    }
}
