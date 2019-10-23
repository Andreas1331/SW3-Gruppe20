using Caliburn.Micro;
using SW3Projekt.Models;

namespace SW3Projekt.ViewModels
{
    public class VismaEntryViewModel : Screen
    {
        private VismaEntry _entry;
        public VismaEntry Entry
        {
            get { return _entry; }
            set { _entry = value; }
        }

        private float _value;
        public float Value
        {
            get { return _value; }
            set { _value = value;  }
        }

        public void BtnRemoveVismaEntry()
        {

        }
    }
}
