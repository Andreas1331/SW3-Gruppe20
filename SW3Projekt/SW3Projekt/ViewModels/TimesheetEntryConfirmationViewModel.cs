using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.ViewModels
{
    public class TimesheetEntryConfirmationViewModel : Conductor<object>
    {
        public string TypeText { get; set; }
        public string ProjectIdText { get; set; }
        public string StartTimeText { get; set; }
        public string EndTimeText { get; set; }
        public string DrivingText { get; set; }

        public BindableCollection<VismaEntryViewModel> VismaEntries { get; set; } = new BindableCollection<VismaEntryViewModel>();


        public TimesheetEntryConfirmationViewModel(TimesheetEntry entry)
        {
            TypeText = "Type: " + entry.SelectedTypeComboBoxItem;
            ProjectIdText = "Projekt-ID: " + entry.ProjectID;
            StartTimeText = "Start: " + entry.StartTime.ToString();
            EndTimeText = "Slut: "  + entry.EndTime.ToString();
            DrivingText = "Kørsel: " + entry.SelectedRouteComboBoxItem;

            foreach (VismaEntry visma in entry.vismaEntries)
            {
                VismaEntries.Add(new VismaEntryViewModel(visma));
            }
        }
    }
}
