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

        public void BtnAddRatesToCA()
        {
            RateEntries.Add(new AddRateViewModel(this));
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
