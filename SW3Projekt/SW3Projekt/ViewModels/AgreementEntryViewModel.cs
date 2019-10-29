using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.ViewModels
{
    public class AgreementEntryViewModel : Screen
    {
        public int Height { get; set; } = 100;

        public CollectiveAgreement colAgreementEntry{ get; set;}
    
        public AgreementEntryViewModel(CollectiveAgreement col) 
        {
            colAgreementEntry = col;
        }
    }

}
