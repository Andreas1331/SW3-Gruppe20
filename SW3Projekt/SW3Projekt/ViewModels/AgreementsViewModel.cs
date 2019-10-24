using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.ViewModels
{
    public class AgreementsViewModel : Conductor<object>
    {
        public void BtnAddAgreement()
        {
            ActivateItem(new AddAgreementViewModel(this));
        }
    }

}
