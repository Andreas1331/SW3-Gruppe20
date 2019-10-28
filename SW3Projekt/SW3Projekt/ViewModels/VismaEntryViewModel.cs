using Caliburn.Micro;
using SW3Projekt.Models;

namespace SW3Projekt.ViewModels
{
    public class VismaEntryViewModel : Screen
    {
        public VismaEntry Entry { get; set; }

        public VismaEntryViewModel(VismaEntry entry)
        {
            Entry = entry;
        }
        
        public void BtnRemoveVismaEntry()
        {

        }
    }
}
