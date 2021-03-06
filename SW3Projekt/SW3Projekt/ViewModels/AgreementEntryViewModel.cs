﻿using Caliburn.Micro;
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
        // Properties
        public AgreementsViewModel agreementMasterPage;
        public CollectiveAgreement colAgreementEntry { get; set; }
        public ShellViewModel Svm;
        public bool isBtnActive { get; set; } = true;
        // Design properties
        public double ShadowRadius { get; set; } = GraphicalDesign.ShadowRadius;
        public double ShadowDirection { get; set; } = GraphicalDesign.ShadowDirection;
        public double ShadowDepth { get; set; } = GraphicalDesign.ShadowDepth;
        public double ShadowOpacity { get; set; } = GraphicalDesign.ShadowOpacity;

        // Constructor
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

        // Methods
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
            string caption = "Vil du slette denne overenskomst?";
            string message = "Alt data vil gå tabt.";
            System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.YesNo;
            System.Windows.Forms.DialogResult result;

            result = System.Windows.Forms.MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
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
}
