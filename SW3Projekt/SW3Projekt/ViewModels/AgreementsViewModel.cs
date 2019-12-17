using Caliburn.Micro;
using SW3Projekt.DatabaseDir;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.ObjectModel;

namespace SW3Projekt.ViewModels
{
    public class AgreementsViewModel : Conductor<object>
    {
        // Properties
        public ShellViewModel Svm;

        

        //Collection properties 
        // Contains all collective agreements
        private List<CollectiveAgreement> CollectiveAgreements { get; set; } = new List<CollectiveAgreement>();
        // Each type of agreement state
        public ObservableCollection<AgreementEntryViewModel> ActiveEntries { 
            get 
            {
                CollectiveAgreement col = CollectiveAgreements.FirstOrDefault(x => x.IsActive);

                List<AgreementEntryViewModel> lstAgreementActive = new List<AgreementEntryViewModel>() { new AgreementEntryViewModel(this, col, Svm) };
                return new ObservableCollection<AgreementEntryViewModel>(lstAgreementActive);
            } 
        }
        public ObservableCollection<AgreementEntryViewModel> NonArchievedEntries {
            get
            {
                List<CollectiveAgreement> col = CollectiveAgreements.FindAll(x => !(x.IsActive) && !(x.IsArchived));

                List<AgreementEntryViewModel> lstAgreementIdle = new List<AgreementEntryViewModel>();
                foreach (CollectiveAgreement item in col)
                {
                    lstAgreementIdle.Add(new AgreementEntryViewModel(this, item, Svm));
                }

                return new ObservableCollection<AgreementEntryViewModel>(lstAgreementIdle);
            }
        }
        public ObservableCollection<AgreementEntryViewModel> ArchievedEntries
        {
            get
            {
                List<CollectiveAgreement> col = CollectiveAgreements.FindAll(x => x.IsArchived);

                List<AgreementEntryViewModel> lstAgreementArchived = new List<AgreementEntryViewModel>();
                foreach (CollectiveAgreement item in col)
                {
                    lstAgreementArchived.Add(new AgreementEntryViewModel(this, item, Svm));
                }

                return new ObservableCollection<AgreementEntryViewModel>(lstAgreementArchived);
            }
        }
      
        // Constructor 1
        public AgreementsViewModel(ShellViewModel Shell)
        {
            Svm = Shell;
            CollectiveAgreements = GetCollectiveAgreementsAsync();
        }

        // Constructor 2
        public AgreementsViewModel()
        {
            CollectiveAgreements = GetCollectiveAgreementsAsync();
        }

        //METHODS
        // Button methods
        public void BtnAddAgreement()
        {
            ActivateItem(new AddAgreementViewModel(this));
        }

        // Database methods
        // Get all Collective agreements from database
        private List<CollectiveAgreement> GetCollectiveAgreementsAsync()
        { 
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                List<CollectiveAgreement> colAgreeList = new List<CollectiveAgreement>();

                colAgreeList = ctx.CollectiveAgreements.Include(x => x.Rates).ToList();
                colAgreeList = SortAgreements(colAgreeList);
                return colAgreeList;
            }
        }

        private List<CollectiveAgreement> SortAgreements(List<CollectiveAgreement> colAgreeList) 
        {
            foreach (CollectiveAgreement collectiveAgreement in colAgreeList) 
            {
                collectiveAgreement.Rates = collectiveAgreement.Rates.OrderBy(x=> x.Name).ToList();
            }
            return colAgreeList;
        }

        public void SetCollectiveAgreementActive(CollectiveAgreement colAgr)
        {
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                foreach (CollectiveAgreement item in ctx.CollectiveAgreements.ToList())
                {
                    item.IsActive = false;
                }

                CollectiveAgreement col = ctx.CollectiveAgreements.FirstOrDefault(x => x.Id == colAgr.Id);
                col.IsActive = true;

                ctx.CollectiveAgreements.Attach(col);
                ctx.Entry(col).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public void SetCollectiveAgreementArchived(CollectiveAgreement colAgr)
        {
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                CollectiveAgreement col = ctx.CollectiveAgreements.FirstOrDefault(x => x.Id == colAgr.Id);
                col.IsArchived = true;

                ctx.CollectiveAgreements.Attach(col);
                ctx.Entry(col).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }
    }
}
