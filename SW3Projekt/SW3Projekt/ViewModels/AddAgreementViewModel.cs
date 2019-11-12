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
    // LOT OF STUFF IN THIS CLASS IS HARDCODED TO MAKE A LOT OF OTHER STUFF DYNAMIC. 
    // THESE RATES ARE RULES THAT EVERY EMPLOYEE IN DENMARK HAS RIGHTS TO.
    public class AddAgreementViewModel : Conductor<object>
    {
        // PROPERTIES
        private AgreementsViewModel _agreementViewModel = new AgreementsViewModel();
        public bool IsReadOnly { get; set; } = false;
        public bool IsItemActive { get; set; } = true;
        public string visibilityState { get; set; } = "Visible";
        public string HeaderText { get; set; } = "Tilføj Overenskomst";
        public int PreDefinedRateGridMaxHeight { get; set; } = 300;
        public CollectiveAgreement ColAgreement { get; set; } = new CollectiveAgreement();
        public ObservableCollection<AddRateViewModel> RateEntries { get; set; } = new ObservableCollection<AddRateViewModel>();
        // PRE-DEFINED RATES
        public Rate ChildIllnessRate { get; set; }
        public Rate DisplacedTimeRate { get; set; }
        public Rate PaidLeaveRate { get; set; }
        public Rate HolidayRate { get; set; }
        public Rate HolidayFreeRate { get; set; } 
        public Rate ShDayRate { get; set; } 
        public Rate IllnessRate { get; set; }
        public Rate DietRate { get; set; } 
        public Rate LogiRate { get; set; }
        public Rate NormRate { get; set; }


        //CONSTRUCTORS
        // CREATING A AGREEMENT
        public AddAgreementViewModel(AgreementsViewModel agreementViewModelInstanceThatWeCanGetBackTo)
        {
            _agreementViewModel = agreementViewModelInstanceThatWeCanGetBackTo;
            initPredefinedRates();
        }

        // VIEWING A AGREEMENT
        public AddAgreementViewModel(CollectiveAgreement col, AgreementsViewModel agreementViewModelInstanceThatWeCanGetBackTo2, bool state)
        {
            ColAgreement = col;
            List<AddRateViewModel> rates = new List<AddRateViewModel>();
            ColAgreement.Rates.ForEach(x => rates.Add(new AddRateViewModel(x, state)));
            RateEntries = new ObservableCollection<AddRateViewModel>(rates);
            _agreementViewModel = agreementViewModelInstanceThatWeCanGetBackTo2;
            IsReadOnly = state;
            IsItemActive = !state;
            visibilityState = "Hidden";
            PreDefinedRateGridMaxHeight = 0;

            if (IsReadOnly == true)
            {
                HeaderText = "Overenskomst oversigt";
            }
        }


        //METHODS
        public void initPredefinedRates()
        { 
            // DATA FOR EACH CARD AND PREDEFINED RATE
            ChildIllnessRate    = new Rate() { Name = "Barn syg",       Type = "Barn syg",      DaysPeriod = GetAllDays(),  SaveAsMoney = false};
            DisplacedTimeRate   = new Rate() { Name = "Forskudttid",    Type = "Forskudttid",   DaysPeriod = GetAllDays(),  SaveAsMoney = true };
            PaidLeaveRate       = new Rate() { Name = "Afspadsering",   Type = "Afspadsering",  DaysPeriod = GetAllDays(),  SaveAsMoney = false };
            HolidayRate         = new Rate() { Name = "Ferie",          Type = "Ferie",         DaysPeriod = GetAllDays(),  SaveAsMoney = false };
            HolidayFreeRate     = new Rate() { Name = "Feriefri",       Type = "Feriefri",      DaysPeriod = GetAllDays(),  SaveAsMoney = false };
            ShDayRate           = new Rate() { Name = "SH-dage",        Type = "SH-dage",       DaysPeriod = GetAllDays(),  SaveAsMoney = true };
            IllnessRate         = new Rate() { Name = "Sygdom",         Type = "Sygdom",        DaysPeriod = GetAllDays(),  SaveAsMoney = false };
            DietRate            = new Rate() { Name = "Diæt",           Type = "Diæt",          DaysPeriod = GetAllDays(),  SaveAsMoney = true };
            LogiRate            = new Rate() { Name = "Logi",           Type = "Logi",          DaysPeriod = GetAllDays(),  SaveAsMoney = true };
            NormRate            = new Rate() { Name = "Normal",         Type = "Arbejde",       DaysPeriod = GetWorkDays(), SaveAsMoney = false };
            AddRateViewModel normRVM = new AddRateViewModel(NormRate, false, true);
            RateEntries.Add(normRVM);
        }

        public Days GetAllDays()
        {
            return (Days.Monday | Days.Tuesday | Days.Wednesday | Days.Thursday | Days.Friday | Days.Saturday | Days.Sunday);
        }
        public Days GetWorkDays()
        {
            return (Days.Monday | Days.Tuesday | Days.Wednesday | Days.Thursday | Days.Friday);
        }

        public void BtnAddRatesToCA()
        {
            RateEntries.Add(new AddRateViewModel(this, IsReadOnly));
        }

        // NEEDS PREDIFENED "Normal" RATE

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
            ColAgreement.Rates.Add(ChildIllnessRate);
            ColAgreement.Rates.Add(DisplacedTimeRate);
            ColAgreement.Rates.Add(PaidLeaveRate);
            ColAgreement.Rates.Add(HolidayRate);
            ColAgreement.Rates.Add(HolidayFreeRate);
            ColAgreement.Rates.Add(ShDayRate);
            ColAgreement.Rates.Add(IllnessRate);
            ColAgreement.Rates.Add(DietRate);
            ColAgreement.Rates.Add(LogiRate);

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
