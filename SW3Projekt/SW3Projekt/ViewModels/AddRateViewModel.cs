using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.ViewModels
{
    public class AddRateViewModel : Screen
    {
        private AddAgreementViewModel _agreementVievModel;
        public AddRateViewModel(AddAgreementViewModel agvm)
        {
            _agreementVievModel = agvm;
        }

        //Do not look at the scheisse - it's all hardcoded. 
        //But it works :)

        private bool _isCheckedMon = true;
        private bool _isCheckedTir = true;
        private bool _isCheckedOns = true;
        private bool _isCheckedTor = true;
        private bool _isCheckedFre = true;
        private bool _isCheckedLor = false;
        private bool _isCheckedSon = false;

        public bool IsCheckedMon
        {
            get
            {
                return _isCheckedMon;
            }
            set
            {
                _isCheckedMon = value;
                NotifyOfPropertyChange(() => IsCheckedMon);
            }
        }
        public bool IsCheckedTir
        {
            get
            {
                return _isCheckedTir;
            }
            set
            {
                _isCheckedTir = value;
                NotifyOfPropertyChange(() => IsCheckedTir);
            }
        }
        public bool IsCheckedOns
        {
            get
            {
                return _isCheckedOns;
            }
            set
            {
                _isCheckedOns = value;
                NotifyOfPropertyChange(() => IsCheckedOns);
            }
        }
        public bool IsCheckedTor
        {
            get
            {
                return _isCheckedTor;
            }
            set
            {
                _isCheckedTor = value;
                NotifyOfPropertyChange(() => IsCheckedTor);
            }
        }
        public bool IsCheckedFre
        {
            get
            {
                return _isCheckedFre;
            }
            set
            {
                _isCheckedFre = value;
                NotifyOfPropertyChange(() => IsCheckedFre);
            }
        }
        public bool IsCheckedLor
        {
            get
            {
                return _isCheckedLor;
            }
            set
            {
                _isCheckedLor = value;
                NotifyOfPropertyChange(() => IsCheckedLor);
            }
        }
        public bool IsCheckedSon
        {
            get
            {
                return _isCheckedSon;
            }
            set
            {
                _isCheckedSon = value;
                NotifyOfPropertyChange(() => IsCheckedSon);
            }
        }

        public void BtnCheckWorkDays()
        {
            IsCheckedMon = true;
            IsCheckedTir = true;
            IsCheckedOns = true;
            IsCheckedTor = true;
            IsCheckedFre = true;
            IsCheckedLor = false;
            IsCheckedSon = false;
        }

        public void BtnCheckAll()
        {
            IsCheckedMon = true;
            IsCheckedTir = true;
            IsCheckedOns = true;
            IsCheckedTor = true;
            IsCheckedFre = true;
            IsCheckedLor = true;
            IsCheckedSon = true;
        }
        public void BtnUnCheckAll()
        {
            IsCheckedMon = false;
            IsCheckedTir = false;
            IsCheckedOns = false;
            IsCheckedTor = false;
            IsCheckedFre = false;
            IsCheckedLor = false;
            IsCheckedSon = false;
        }

        public void BtnRemoveRateEntry()
        {
            _agreementVievModel.RateEntries.Remove(this);
        }

    }
}
