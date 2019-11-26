using Caliburn.Micro;
using SW3Projekt.DatabaseDir;
using SW3Projekt.Models;
using SW3Projekt.Tools;
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
        public ObservableCollection<AddRateViewModel> RateEntries { get; set; } = new ObservableCollection<AddRateViewModel>();
        public CollectiveAgreement ColAgreement { get; set; } = new CollectiveAgreement();
        public string SaveAsMoneyCheckBoxText { get; set; } = "Gem beløb i kroner";
        public string TooltipSaveAsMoney { get; set; } = "Vælg om værdien skal gemmes som kroner, til Visma";
        public double ShadowRadius { get; set; } = GraphicalDesign.ShadowRadius;
        public double ShadowDirection { get; set; } = GraphicalDesign.ShadowDirection;
        public double ShadowDepth { get; set; } = GraphicalDesign.ShadowDepth;
        public double ShadowOpacity { get; set; } = GraphicalDesign.ShadowOpacity;

        // STATES
        public bool IsViewingAgreement { get; set; }
        public bool IsViewingAgreementNeg { get; set; }
        public bool IsTopInformationEditble { get; set; }
        public string visibilityState { get; set; } = "Visible";
        public int PreDefinedRateGridMaxHeight { get; set; } = 300;
        public string RateListHeader { get; set; } = "Tillægsrater";

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
        public Rate KørselRate { get; set; }
        public Rate NormRate { get; set; }

        //CONSTRUCTORS
        // CREATING A AGREEMENT
        public AddAgreementViewModel(AgreementsViewModel agreementViewModelInstanceThatWeCanGetBackTo)
        {
            IsViewingAgreement = false;
            IsViewingAgreementNeg   = !IsViewingAgreement;
            IsTopInformationEditble = !IsViewingAgreement;
            _agreementViewModel = agreementViewModelInstanceThatWeCanGetBackTo;
            initPredefinedRates();
        }

        // VIEWING A AGREEMENT
        public AddAgreementViewModel(CollectiveAgreement col, AgreementsViewModel agreementViewModelInstanceThatWeCanGetBackTo2, bool state)
        {
            IsViewingAgreement = true;
            IsViewingAgreementNeg   = !IsViewingAgreement;
            IsTopInformationEditble = !IsViewingAgreement;
            ColAgreement = col;
            List<AddRateViewModel> rates = new List<AddRateViewModel>();
            ColAgreement.Rates.ForEach(x => rates.Add(new AddRateViewModel(x, true, false, false, false, false, false, false)));
            RateEntries = new ObservableCollection<AddRateViewModel>(rates);
            _agreementViewModel = agreementViewModelInstanceThatWeCanGetBackTo2;
            visibilityState = "Hidden";
            RateListHeader = "Liste over rater";
            PreDefinedRateGridMaxHeight = 0;
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
            KørselRate          = new Rate() { Name = "Kørsel",         Type = "Kørsel",        DaysPeriod = GetAllDays(),  SaveAsMoney = true };
            NormRate            = new Rate() { Name = "Normal",         Type = "Arbejde",       DaysPeriod = GetWorkDays(), SaveAsMoney = false , StartTime = new DateTime(1, 1, 1, 7, 0, 0), EndTime = new DateTime(1,1,1,15,0,0)};
            // ADD AGREEMENTS WITH NAME IN READONLY MODE
            AddRateViewModel KørselRVM = new AddRateViewModel(KørselRate, true, true, false, false, true, false, false );
            AddRateViewModel normRVM = new AddRateViewModel(NormRate, true, true, true, true, true, false, false );
            RateEntries.Add(KørselRVM);
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
            RateEntries.Add(new AddRateViewModel(this, false, true, true, true, true, true, true));
        }

        // NEEDS PREDIFENED "Normal" RATE

        public void BtnBackToCaOverview()
        {
            if (IsViewingAgreement)
            {
                _agreementViewModel.DeactivateItem(this, true);
            }
            else
            {
                string caption = "Sikker på du vil gå tilbage?";
                string message = "Alt input i den nye overenskomst vil gå tabt.";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                result = MessageBox.Show(message, caption, buttons);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    _agreementViewModel.DeactivateItem(this, true);
                }
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
                //RateEntries.ToList().ForEach(x => ColAgreement.Rates.Add(x.Rate));

                foreach (AddRateViewModel rate in RateEntries.ToList()) {
                    if (rate.Rate.EndTime < rate.Rate.StartTime && rate.Rate.EndTime != new DateTime())
                    {
                        //skabelse og tilføjelse af extrarate
                        Rate Rate = rate.Rate;
                        Rate extraRate = new Rate
                        {
                            
                            Name = Rate.Name ?? "",
                            VismaID = Rate.VismaID,
                            StartTime = new DateTime(),
                            EndTime = Rate.EndTime,
                            RateValue = Rate.RateValue,
                            CollectiveAgreementID = Rate.CollectiveAgreementID,
                            DaysPeriod = Rate.DaysPeriod,
                            SaveAsMoney = Rate.SaveAsMoney,
                            Type = Rate.Type ?? ""
                        };
                        ColAgreement.Rates.Add(extraRate);

                        //tilføjelse af den originale
                        Rate.EndTime = new DateTime(1,1,1,23,59,0);
                        ColAgreement.Rates.Add(Rate);
                    }
                    else 
                    {
                        if (rate.Rate.StartTime != new DateTime() && rate.Rate.EndTime == new DateTime())
                            rate.Rate.EndTime = new DateTime(1, 1, 1, 23, 59, 0);
                        ColAgreement.Rates.Add(rate.Rate);
                    }
                }
                ctx.CollectiveAgreements.Add(ColAgreement);
                ctx.SaveChanges();

            }

            _agreementViewModel.Svm.BtnAgreements(); 
        }

        public void BtnEditAgreement()
        {
            //ColAgreement.Rates.ForEach(x => rates.Add(new AddRateViewModel(x, true, false, false, false, false, false, false)));
            //RateEntries = new ObservableCollection<AddRateViewModel>(rates);
        }


        public void SetPredefinedRatesToReadOnly()
        {
            foreach (var rate in ColAgreement.Rates)
            {
                switch (rate.Name)
                {
                    case "Barn Syg":
                    case "Ferie":
                    case "Sygdom":
                    case "Forskudttid":
                    case "Feriefri":
                    case "Diæt":
                    case "Afspadsering":
                    case "SH-dage":
                    case "Logi":
                        break;
                            
                    default:
                        break;
                }

            }
        }
    }
}
