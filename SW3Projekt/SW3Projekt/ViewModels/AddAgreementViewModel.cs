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
        public int PreDefinedRateGridMaxHeight { get; set; } = 700;
        public string RateListHeader { get; set; } = "Tillægsrater";

        // PRE-DEFINED RATES
        public Rate ChildIllnessRate { get; set; }
        public Rate DisplacedTimeRate { get; set; }
        public Rate PaidLeaveInRate { get; set; }
        public Rate PaidLeaveOutRate { get; set; }
        public Rate HolidayRate { get; set; }
        public Rate HolidayFreeRate { get; set; } 
        public Rate ShDayRate { get; set; } 
        public Rate IllnessRate { get; set; }
        public Rate DietRate { get; set; } 
        public Rate LogiRate { get; set; }
        public Rate KørselRate { get; set; }
        public Rate NormRate { get; set; }

        //CONSTRUCTORS
        // CREATING AN AGREEMENT
        public AddAgreementViewModel(AgreementsViewModel agreementViewModelInstanceThatWeCanGetBackTo)
        {
            IsViewingAgreement = false;
            IsViewingAgreementNeg   = !IsViewingAgreement;
            IsTopInformationEditble = !IsViewingAgreement;
            _agreementViewModel = agreementViewModelInstanceThatWeCanGetBackTo;
            initPredefinedRates();
        }

        // VIEWING AN AGREEMENT
        public AddAgreementViewModel(CollectiveAgreement col, AgreementsViewModel agreementViewModelInstanceThatWeCanGetBackTo2)
        {
            IsViewingAgreement = true;
            IsViewingAgreementNeg = !IsViewingAgreement;
            IsTopInformationEditble = !IsViewingAgreement;
            ColAgreement = col;
            List<AddRateViewModel> rates = new List<AddRateViewModel>();
            ColAgreement.Rates.ForEach(x => rates.Add(new AddRateViewModel(x, true, false, false, false, false, false, false)));
            RateEntries = new ObservableCollection<AddRateViewModel>(rates);
            _agreementViewModel = agreementViewModelInstanceThatWeCanGetBackTo2;
            RateListHeader = "Liste over rater";
            PreDefinedRateGridMaxHeight = 0;
        }

        //METHODS
        public void initPredefinedRates()
        {
            // DATA FOR EACH CARD AND PREDEFINED RATE
            ChildIllnessRate = new Rate() { Name = "Barn syg", Type = "Barn syg", DaysPeriod = GetAllDays(), SaveAsMoney = false };
            DisplacedTimeRate = new Rate() { Name = "Forskudttid", Type = "Forskudttid", DaysPeriod = GetAllDays(), SaveAsMoney = true };
            PaidLeaveInRate = new Rate() { Name = "Afspadsering Ind", Type = "Afspadsering", DaysPeriod = GetAllDays(), SaveAsMoney = false };
            PaidLeaveOutRate = new Rate() { Name = "Afspadsering Ud", Type = "Afspadsering", DaysPeriod = GetAllDays(), SaveAsMoney = false };
            HolidayRate = new Rate() { Name = "Ferie", Type = "Ferie", DaysPeriod = GetAllDays(), SaveAsMoney = false };
            HolidayFreeRate = new Rate() { Name = "Feriefri", Type = "Feriefri", DaysPeriod = GetAllDays(), SaveAsMoney = false };
            ShDayRate = new Rate() { Name = "SH-dage", Type = "SH-dage", DaysPeriod = GetAllDays(), SaveAsMoney = true };
            IllnessRate = new Rate() { Name = "Sygdom", Type = "Sygdom", DaysPeriod = GetAllDays(), SaveAsMoney = false };
            DietRate = new Rate() { Name = "Diæt", Type = "Diæt", DaysPeriod = GetAllDays(), SaveAsMoney = true };
            LogiRate = new Rate() { Name = "Logi", Type = "Logi", DaysPeriod = GetAllDays(), SaveAsMoney = true };
            KørselRate = new Rate() { Name = "Kørsel", Type = "Kørsel", DaysPeriod = GetAllDays(), SaveAsMoney = true };
            NormRate = new Rate() { Name = "Normal", Type = "Arbejde", DaysPeriod = GetWorkDays(), SaveAsMoney = false, RateValue = 1, StartTime = new DateTime(1, 1, 1, 0, 0, 0), EndTime = new DateTime(1, 1, 1, 23, 59, 0) };
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
            bool valid = true;

            if (ColAgreement.IsValidate())
            {
                foreach (var rateVM in RateEntries)
                {
                    if (!rateVM.Rate.IsValidate())
                    {
                        valid = false;
                    }
                }

                if (valid)
                {
                    SaveCollectiveAgreement();
                }
                else
                {
                    //Rate not valid
                    new Notification(Notification.NotificationType.Error, "Fejl i en rate. Kontroller: Navn, VismaID, Sats, Start og Slut tid");
                }
            } 
            else 
            {
                //Col agreement not valid
                new Notification(Notification.NotificationType.Error, "Fejl i overenskomst. Kontroller: Navn, Start og Slut tid");
            }
        }


        private void SaveCollectiveAgreement()
        {
            // Add the predefined rates to the collective agreements rate list
            ColAgreement.Rates.Add(ChildIllnessRate);
            ColAgreement.Rates.Add(DisplacedTimeRate);
            ColAgreement.Rates.Add(PaidLeaveInRate);
            ColAgreement.Rates.Add(PaidLeaveOutRate);
            ColAgreement.Rates.Add(HolidayRate);
            ColAgreement.Rates.Add(HolidayFreeRate);
            ColAgreement.Rates.Add(ShDayRate);
            ColAgreement.Rates.Add(IllnessRate);
            ColAgreement.Rates.Add(DietRate);
            ColAgreement.Rates.Add(LogiRate);
            ColAgreement.Rates.Add(KørselRate);
            ColAgreement.Rates.Add(NormRate);

            // Open db connection and add rates to db
            using (var ctx = new Database())
            {
                foreach (AddRateViewModel rate in RateEntries.ToList())
                {

                    if (rate.Rate.EndTime < rate.Rate.StartTime && rate.Rate.EndTime != new DateTime())
                    {
                        // Creation and adding of extra rates
                        Rate Rate = rate.Rate;
                        Rate extraRate = new Rate
                        {
                            Name = Rate.Name,
                            VismaID = Rate.VismaID,
                            StartTime = new DateTime(),
                            EndTime = Rate.EndTime,
                            RateValue = Rate.RateValue,
                            CollectiveAgreementID = Rate.CollectiveAgreementID,
                            DaysPeriod = Rate.DaysPeriod,
                            SaveAsMoney = Rate.SaveAsMoney,
                            Type = Rate.Type
                        };
                        ColAgreement.Rates.Add(extraRate);

                        // Adding the original
                        Rate.EndTime = new DateTime(1, 1, 1, 23, 59, 0);
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
    }
}
