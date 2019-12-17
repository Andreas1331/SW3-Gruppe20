using Caliburn.Micro;
using SW3Projekt.Views;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SW3Projekt.ViewModels
{
    public class OverviewViewModel : Conductor<object>
    {
        // Properties
        public string BtnBackgroundDefault = "#333333";
        public string BtnBackgroundChosen = "#1565c0";
        private string _btnSaldoBackground = "#333333";
        private string _btnYearlyBackground = "#333333";
        public string BtnSaldoBackground {
            get 
            {
                return _btnSaldoBackground;
            }
            set
            {
                _btnSaldoBackground = value;
                NotifyOfPropertyChange(() => BtnSaldoBackground);
            }
        }
        public string BtnYearlyBackground 
        {
            get
            {
                return _btnYearlyBackground;
            }
            set
            {
                _btnYearlyBackground = value;
                NotifyOfPropertyChange(() => BtnYearlyBackground);
            }
        }

        // Constructor
        public OverviewViewModel()
        {
            BtnYearlyOverview();
        }

        // Methods
        public void BtnYearlyOverview()
        {
            BtnYearlyBackground = BtnBackgroundChosen;
            BtnSaldoBackground = BtnBackgroundDefault;
            ActivateItem(new YearCountViewModel());
        }
        public void BtnSaldoOverview()
        {
            BtnYearlyBackground = BtnBackgroundDefault;
            BtnSaldoBackground = BtnBackgroundChosen;
            ActivateItem(new SaldoOverviewViewModel());
        }
    }
}
