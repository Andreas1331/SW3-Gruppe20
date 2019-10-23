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
    public class AddAgreementViewModel : Conductor<object>
    {
        public ObservableCollection<object> RateEntries { get; set; } = new ObservableCollection<object>();
        
        public void RemoveRatesFromCA(ObservableCollection<AddRateViewModel> collection, AddRateViewModel instance)
        {
            collection.Remove(collection.Where(i => i.Id == instance.Id).Single());
        }

        public int rateId = 0;
        public void BtnAddRatesToCA()
        {
            RateEntries.Add(new AddRateViewModel() {Id = rateId++ });
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
