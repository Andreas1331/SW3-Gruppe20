using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.ViewModels
{
    public class AgreementEntryViewModel : Screen
    {
        public int Height { get; set; } = 100;

        public AgreementsViewModel agreementMasterPage;
        public CollectiveAgreement colAgreementEntry { get; set; }

        public AgreementEntryViewModel(AgreementsViewModel agreementVM, CollectiveAgreement col)
        {
            agreementMasterPage = agreementVM;
            colAgreementEntry = col;
        }

        public void BtnViewRatesInCol()
        {
            agreementMasterPage.ActivateItem(new AddAgreementViewModel(colAgreementEntry, agreementMasterPage, true));
        }
    }
}
