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
        //CONSTRUCTOR
        public AgreementsViewModel()
        {
            CollectiveAgreements = GetCollectiveAgreementsAsync();

            foreach (CollectiveAgreement item in CollectiveAgreements)
            {
                Console.WriteLine("Navn Col: " + item.Name);
                foreach (Rate rateItem in item.Rates)
                {
                    Console.WriteLine("Navn Rate: " + rateItem.Name);
                }
            }
            //Load all agreements from database and categorize
            //Categorize();
        }

        //FIELDS

        //Contains all collective agreements
        private List<CollectiveAgreement> CollectiveAgreements { get; set; } = new List<CollectiveAgreement>();

        //Categorized collective agreements
        private List<CollectiveAgreement> ActiveCollectiveAgreements { get; set; } = new List<CollectiveAgreement>();
        private List<CollectiveAgreement> NonArchievedCollectiveAgreements { get; set; } = new List<CollectiveAgreement>();
        private List<CollectiveAgreement> ArchievedCollectiveAgreements { get; set; } = new List<CollectiveAgreement>();

        //Model Views
        public ObservableCollection<AgreementEntryViewModel> ActiveEntries { 
            get 
            {
                CollectiveAgreement col = CollectiveAgreements.FirstOrDefault(x => x.IsActive);

                List<AgreementEntryViewModel> lstAgreementActive = new List<AgreementEntryViewModel>() { new AgreementEntryViewModel(this, col) };
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
                    lstAgreementIdle.Add(new AgreementEntryViewModel(this, item));
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
                    lstAgreementArchived.Add(new AgreementEntryViewModel(this, item));
                }

                return new ObservableCollection<AgreementEntryViewModel>(lstAgreementArchived);
            }
        }

        //METHODS
        //Button methods
        public void BtnAddAgreement()
        {
            ActivateItem(new AddAgreementViewModel(this));
        }

        //Database methods
        //Get all Collective agreements from database
        private List<CollectiveAgreement> GetCollectiveAgreementsAsync()
        { 
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                //List<CollectiveAgreement> Data = await Task.Run(() => ctx.CollectiveAgreements.ToList());
                List<CollectiveAgreement> colAgreeList = new List<CollectiveAgreement>();

                colAgreeList = ctx.CollectiveAgreements.Include(x => x.Rates).ToList();
                return colAgreeList;
            }
        }

        //Categorize collective agreements in CollectiveAgreements into active, nonarchived and archived
        private void Categorize() 
        {
            //Add all the agreements to the different lists.
            foreach (CollectiveAgreement collectiveAgreement in CollectiveAgreements) //Find all active collective agreements
            {
                //Active
                if (collectiveAgreement.IsActive)
                {
                    ActiveCollectiveAgreements.Add(collectiveAgreement);
                }
                //Non Archieved
                if (!collectiveAgreement.IsArchived)
                {
                    NonArchievedCollectiveAgreements.Add(collectiveAgreement);
                }
                //Archieved
                if (collectiveAgreement.IsArchived)
                {
                    ArchievedCollectiveAgreements.Add(collectiveAgreement);
                }
            }
        }
    }
}
