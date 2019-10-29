using Caliburn.Micro;
using SW3Projekt.DatabaseDir;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        //CONSTRUCTOR
        public AgreementsViewModel()
        {
            ////Load all agreements from database and categorize
            //Task.Run(async () =>
            //{
            //    CollectiveAgreements = await GetCollectiveAgreementsAsync(); 

            //    Categorize();

            //    //ActiveAgreements = new BindableCollection<CollectiveAgreement>(CollectiveAgreements);
            //});
            Categorize();
            InstatiateEntries();
        }

        //METHODS
        //Button methods
        public void BtnAddAgreement()
        {
            ActivateItem(new AddAgreementViewModel(this));
        }

        //Database methods
        private async Task<List<CollectiveAgreement>> GetCollectiveAgreementsAsync() //Get all employees from database
        {
            using (var ctx = new Database())
            {
                List<CollectiveAgreement> Data = await Task.Run(() => ctx.CollectiveAgreements.ToList());
                return Data;
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
            /*
            //Non archieved
            foreach (CollectiveAgreement collectiveAgreement in CollectiveAgreements)
                if (!collectiveAgreement.IsArchieved)
                NonArchievedCollectiveAgreements.Add(collectiveAgreement);

            //Archieved
            foreach (CollectiveAgreement collectiveAgreement in CollectiveAgreements)
            if (!collectiveAgreement.IsArchieved)
                NonArchievedCollectiveAgreements.Add(collectiveAgreement);
            */
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
