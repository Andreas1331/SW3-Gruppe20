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
        public AgreementsViewModel agreementMasterPage;
        public CollectiveAgreement colAgreementEntry { get; set; }
        public bool isRemoveBtnActive { get; set; } = true;
        public AgreementEntryViewModel(AgreementsViewModel agreementVM, CollectiveAgreement col)
        {
            agreementMasterPage = agreementVM;
            colAgreementEntry = col;

            if(colAgreementEntry.IsActive == true)
            {
                isRemoveBtnActive = false;
            }
        }

        public void BtnViewRatesInCol()
        {
            agreementMasterPage.ActivateItem(new AddAgreementViewModel(colAgreementEntry, agreementMasterPage, true));
        }   
        public void BtnRemoveCol()
        {
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            { 
                ctx.CollectiveAgreements.Remove(colAgreementEntry);
                ctx.SaveChanges();
            }
        }
    }
}
