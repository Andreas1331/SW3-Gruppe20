using Caliburn.Micro;
using SW3Projekt.Models;
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
            BtnCheckWorkDays();
        }

        public Rate Rate { get; set; } = new Rate();

        //Do not look at the scheisse - it's all hardcoded. 
        //But it works :)

        private bool _isCheckedMon;
        private bool _isCheckedTir;
        private bool _isCheckedOns;
        private bool _isCheckedTor;
        private bool _isCheckedFre;
        private bool _isCheckedLor;
        private bool _isCheckedSon;

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
                Rate.DaysPeriod = value ? (Rate.DaysPeriod ^= Days.Monday) : Rate.DaysPeriod;
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
                Rate.DaysPeriod = value ? (Rate.DaysPeriod ^= Days.Tuesday) : Rate.DaysPeriod;
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
                Rate.DaysPeriod = value ? (Rate.DaysPeriod ^= Days.Wednesday) : Rate.DaysPeriod;
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
                Rate.DaysPeriod = value ? (Rate.DaysPeriod ^= Days.Thursday) : Rate.DaysPeriod;
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
                Rate.DaysPeriod = value ? (Rate.DaysPeriod ^= Days.Friday) : Rate.DaysPeriod;
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
                Rate.DaysPeriod = value ? (Rate.DaysPeriod ^= Days.Saturday) : Rate.DaysPeriod;
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
                Rate.DaysPeriod = value ? (Rate.DaysPeriod ^= Days.Sunday) : Rate.DaysPeriod;
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
