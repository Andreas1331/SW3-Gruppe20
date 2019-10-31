using Caliburn.Micro;
using SW3Projekt.DatabaseDir;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SW3Projekt.ViewModels
{
    public class AddAgreementViewModel : Conductor<object>
    {
        //CONSTRUCTOR
        private AgreementsViewModel _agreementViewModel = new AgreementsViewModel();
        public AddAgreementViewModel(AgreementsViewModel agreementViewModelInstanceThatWeCanGetBackTo)
        {
            _agreementViewModel = agreementViewModelInstanceThatWeCanGetBackTo;
        }
        public bool IsReadOnly { get; set; } = false;
        public bool IsItemActive { get; set; } = true;
        public string HeaderText { get; set; } = "Tilføj Overenskomst";
        public AddAgreementViewModel(CollectiveAgreement col, AgreementsViewModel agreementViewModelInstanceThatWeCanGetBackTo2, bool state)
        {
            ColAgreement = col;
            List<AddRateViewModel> rates = new List<AddRateViewModel>();
            ColAgreement.Rates.ForEach(x => rates.Add(new AddRateViewModel(x, state)));
            RateEntries = new ObservableCollection<AddRateViewModel>(rates);
            _agreementViewModel = agreementViewModelInstanceThatWeCanGetBackTo2;
            IsReadOnly = state;
            IsItemActive = !state;
            if (IsReadOnly == true)
            {
                HeaderText = "Overenskomst oversigt";
            }
        }

        //PROPERTIES
        public CollectiveAgreement ColAgreement { get; set; } = new CollectiveAgreement();
        public ObservableCollection<AddRateViewModel> RateEntries { get; set; } = new ObservableCollection<AddRateViewModel>();        

        //METHODS
        public void BtnAddRatesToCA()
        {
            RateEntries.Add(new AddRateViewModel(this, IsReadOnly));
        }
        public void BtnBackToCaOverview()
        {
            string caption = "Sikker på du vil gå tilbage?";
            string message = "Alt input i den nye overenskomst vil gå tabt.";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show(message, caption, buttons);

            if(result == System.Windows.Forms.DialogResult.Yes)
            {
                _agreementViewModel.DeactivateItem(this, true);
            }
        }
        public void BtnSaveCA()
        {
            using (var ctx = new Database())
            {
                RateEntries.ToList().ForEach(x => ColAgreement.Rates.Add(x.Rate));
                ctx.CollectiveAgreements.Add(ColAgreement);
                ctx.SaveChanges();

            }

            _agreementViewModel.Svm.BtnAgreements(); 
            
        }
    }
}
