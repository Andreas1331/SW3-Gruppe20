using Caliburn.Micro;
using SW3Projekt.Models;
using SW3Projekt.Tools;
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
        public bool isBtnActive { get; set; } = true;
        public ShellViewModel Svm;
        public AgreementEntryViewModel(AgreementsViewModel agreementVM, CollectiveAgreement col, ShellViewModel Shell)
        {
            agreementMasterPage = agreementVM;
            colAgreementEntry = col;
            Svm = Shell;

            if(colAgreementEntry.IsActive == true)
            {
                isBtnActive = false;
            }

            if (colAgreementEntry.IsArchived == true)
            {
                isBtnActive = false;
            }
        }
        public double ShadowRadius { get; set; } = GraphicalDesign.ShadowRadius;
        public double ShadowDirection { get; set; } = GraphicalDesign.ShadowDirection;
        public double ShadowDepth { get; set; } = GraphicalDesign.ShadowDepth;
        public double ShadowOpacity { get; set; } = GraphicalDesign.ShadowOpacity;

        public void BtnActivateCol()
        {
            agreementMasterPage.SetCollectiveAgreementActive(colAgreementEntry);
            Svm.BtnAgreements();
        }

        public void BtnViewRatesInCol()
        {
            agreementMasterPage.ActivateItem(new AddAgreementViewModel(colAgreementEntry, agreementMasterPage));
        }

        public void BtnArchiveCol()
        {
            agreementMasterPage.SetCollectiveAgreementArchived(colAgreementEntry);
            Svm.BtnAgreements();
        }
        
        public void BtnRemoveCol()
        {
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                ctx.CollectiveAgreements.Attach(colAgreementEntry);
                ctx.CollectiveAgreements.Remove(colAgreementEntry);
                ctx.SaveChanges();
            }

            Svm.BtnAgreements();
        }
    }
}
