using Caliburn.Micro;
using SW3Projekt.Models;
using System.Windows.Controls;

namespace SW3Projekt.ViewModels
{
    public class VismaEntryViewModel : Screen
    {
        public VismaEntry Entry { get; set; }

        public TimesheetEntryConfirmationViewModel TimesheetEntry { get; set; }


        public VismaEntryViewModel(VismaEntry entry, TimesheetEntryConfirmationViewModel timesheetEntry)
        {
            Entry = entry;
            TimesheetEntry = timesheetEntry;
        }
        
        public void BtnRemoveVismaEntry()
        {
            TimesheetEntry.RemoveEntry(this);
        }


    }
}
