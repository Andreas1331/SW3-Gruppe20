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
        // PROPERTIES
        private AgreementsViewModel _agreementViewModel = new AgreementsViewModel();
        public bool IsReadOnly { get; set; } = false;
        public bool IsItemActive { get; set; } = true;
        public string HeaderText { get; set; } = "Tilføj Overenskomst";
        public float DietValueBox
        {
            get
            {
                return ColAgreement.DietValue;
            }
            set
            {
                ColAgreement.DietValue = value;
                NotifyOfPropertyChange(() => DietValueBox);
            }
        }
        public float LogiValueBox
        {
            get
            {
                return ColAgreement.LogiValue;
            }
            set
            {
                ColAgreement.LogiValue = value;
                NotifyOfPropertyChange(() => LogiValueBox);
            }
        }
        public float MileageValueBox
        {
            get
            {
                return ColAgreement.MileageValue;
            }
            set
            {
                ColAgreement.MileageValue = value;
                NotifyOfPropertyChange(() => MileageValueBox);
            }
        }
        public CollectiveAgreement ColAgreement { get; set; } = new CollectiveAgreement();
        public ObservableCollection<AddRateViewModel> RateEntries { get; set; } = new ObservableCollection<AddRateViewModel>();


        //CONSTRUCTOR
        public AddAgreementViewModel(AgreementsViewModel agreementViewModelInstanceThatWeCanGetBackTo)
        {
            _agreementViewModel = agreementViewModelInstanceThatWeCanGetBackTo;
        }
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

        // DATA FOR EACH CARD
        public Rate ChildIllnessRate { get; set; } = new Rate() { Name = "Barn syg", Type = "Barn syg" };
        public Rate DisplacedTimeRate { get; set; } = new Rate() { Name = "Forskudttid", Type = "Forskudttid" };
        public Rate PaidLeaveRate { get; set; } = new Rate() { Name = "Afspadsering", Type = "Afspadsering" };
        public Rate HolidayRate { get; set; } = new Rate() { Name = "Ferie", Type = "Ferie" };
        public Rate HolidayFreeRate { get; set; } = new Rate() { Name = "Feriefri", Type = "Feriefri" };
        public Rate ShDayRate { get; set; } = new Rate() { Name = "SH-dage", Type = "SH-dage" };
        public Rate IllnessRate { get; set; } = new Rate() { Name = "Sygdom", Type = "Sygdom" };
        public Rate DietRate { get; set; } = new Rate() { Name = "Diæt", Type = "Diæt" };
        public Rate LogiRate { get; set; } = new Rate() { Name = "Logi", Type = "Logi" };



        //METHODS
        public void initRates()
        {
        }
       
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
            // Add the predefined rates to the collective agreements rate list
            ColAgreement.Rates.Add(DietRate);

            // Open db connection and add rates to db
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
