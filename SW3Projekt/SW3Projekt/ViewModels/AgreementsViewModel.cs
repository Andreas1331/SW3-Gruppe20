using Caliburn.Micro;
using SW3Projekt.DatabaseDir;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SW3Projekt.ViewModels
{
    public class AgreementsViewModel : Conductor<object>
    {
        //FIELDS

        //Contains all collective agreements
        private List<CollectiveAgreement> CollectiveAgreements { get; set; } = new List<CollectiveAgreement>();

        //Categorized collective agreements
        private List<CollectiveAgreement> ActiveCollectiveAgreements { get; set; } = new List<CollectiveAgreement>();
        private List<CollectiveAgreement> NonArchievedCollectiveAgreements { get; set; } = new List<CollectiveAgreement>();
        private List<CollectiveAgreement> ArchievedCollectiveAgreements { get; set; } = new List<CollectiveAgreement>();

        //Model Views
        public BindableCollection<AgreementEntryViewModel> ActiveEntries { get; set; } = new BindableCollection<AgreementEntryViewModel>();
        public BindableCollection<AgreementEntryViewModel> NonArchievedEntries { get; set; } = new BindableCollection<AgreementEntryViewModel>();
        public BindableCollection<AgreementEntryViewModel> ArchievedEntries { get; set; } = new BindableCollection<AgreementEntryViewModel>();

        //Lav logik i getters til at få den aktuelle agreement
        //fjern setters


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
            Task.Run(async () =>
            {

                //Categorize();

                //ActiveEntries = new BindableCollection<CollectiveAgreement>(CollectiveAgreements);
            });
        }

        //METHODS
        //Button methods
        public void BtnAddAgreement()
        {
            ActivateItem(new AddAgreementViewModel(this));
        }

        //Database methods
        private List<CollectiveAgreement> GetCollectiveAgreementsAsync() //Get all employees from database
        {
            using (var ctx = new SW3Projekt.DatabaseDir.Database())
            {
                //List<CollectiveAgreement> Data = await Task.Run(() => ctx.CollectiveAgreements.ToList());
                List<CollectiveAgreement> colAgreeList = new List<CollectiveAgreement>();

                colAgreeList = ctx.CollectiveAgreements.Include(x => x.Rates).ToList();
                return colAgreeList;
            }
        }

        private void Categorize() //Categorize collective agreements in CollectiveAgreements into active, nonarchived and archived
        {
            //Active collective agreement
            foreach (CollectiveAgreement collectiveAgreement in CollectiveAgreements) //Find all active collective agreements
            {
                if (collectiveAgreement.IsActive)
                    ActiveCollectiveAgreements.Add(collectiveAgreement);

            }

            //Check if more than one collective agreement is active. Should give an error
            if (ActiveCollectiveAgreements.Count > 1)
                Console.WriteLine("More than one collection agreement is active"); //Console writeline for now
            
            ////Non archieved
            //foreach (CollectiveAgreement collectiveAgreement in CollectiveAgreements)
            //    if (!collectiveAgreement.IsArchived)
            //    NonArchievedCollectiveAgreements.Add(collectiveAgreement);

            ////Archieved
            //foreach (CollectiveAgreement collectiveAgreement in CollectiveAgreements)
            //if (collectiveAgreement.IsArchived)
            //    NonArchievedCollectiveAgreements.Add(collectiveAgreement);
            
        }

        private void InstatiateEntries(){
            //Test data
            //TODO instantiate agreemententries corresponding with the time sheets and bind their data
            ActiveEntries.Add(new AgreementEntryViewModel());

            NonArchievedEntries.Add(new AgreementEntryViewModel());
            NonArchievedEntries.Add(new AgreementEntryViewModel());
            NonArchievedEntries.Add(new AgreementEntryViewModel());
            NonArchievedEntries.Add(new AgreementEntryViewModel());

            ArchievedEntries.Add(new AgreementEntryViewModel());
            ArchievedEntries.Add(new AgreementEntryViewModel());
            ArchievedEntries.Add(new AgreementEntryViewModel());
            ArchievedEntries.Add(new AgreementEntryViewModel());
            ArchievedEntries.Add(new AgreementEntryViewModel());
            ArchievedEntries.Add(new AgreementEntryViewModel());
        }
    }
}
