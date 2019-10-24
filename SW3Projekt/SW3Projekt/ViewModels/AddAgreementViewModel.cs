using Caliburn.Micro;
using SW3Projekt.DatabaseDir;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.ViewModels
{
    public class AddAgreementViewModel : Conductor<object>
    {
        //CONSTRUCTOR
        private AgreementsViewModel _agreementViewModel = new AgreementsViewModel();
        public AddAgreementViewModel(AgreementsViewModel agreementViewModelInstanceThatWeCanGetBackToo)
        {
            _agreementViewModel = agreementViewModelInstanceThatWeCanGetBackToo;
        }

        public CollectiveAgreement ColAgreement = new CollectiveAgreement();
        
        public ObservableCollection<AddRateViewModel> RateEntries { get; set; } = new ObservableCollection<AddRateViewModel>();        



        public void BtnAddRatesToCA()
        {
            RateEntries.Add(new AddRateViewModel(this));
        }

        public void BtnBackToCaOverview()
        {
            _agreementViewModel.DeactivateItem(this, true);
        }

        public void BtnSaveCA()
        {
            using (var ctx = new Database())
            {
                ctx.CollectiveAgreements.Add(ColAgreement);
                ctx.SaveChanges();

                RateEntries.ToList().ForEach(x => x.Rate.CollectiveAgreementID = ColAgreement.Id);

            }
        }




        public void CreateNewAgreement()
        {
            //CollectiveAgreement ca = new CollectiveAgreement()
            //{
            //    Name =,
            //    Start,
            //    End,
            //    Rates,
            //    IsActive
            //}

            //using (var ctx = new SW3Projekt.DatabaseDir.Database())
            //{
            //    ctx.CollectiveAgreements.Add(ca);
            //    ctx.SaveChanges();
            //}

        }
    }
}
