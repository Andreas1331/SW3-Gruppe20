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
        public bool IsReadOnly { get; set; }
        public bool IsItemActive { get; set; }
        private AddAgreementViewModel _agreementVievModel;
        public AddRateViewModel(AddAgreementViewModel agvm, bool isReadOnly)
        {
            _agreementVievModel = agvm;
            IsReadOnly = isReadOnly;
            IsItemActive = !isReadOnly;

            BtnCheckWorkDays();
        }

        public AddRateViewModel(Rate rate, bool isReadOnly)
        {
            Rate = rate;
            IsReadOnly = isReadOnly;
            IsItemActive = !isReadOnly;
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
                Rate.DaysPeriod ^= Days.Monday;
            }
        }
        public bool IsCheckedTue
        {
            get
            {
                return _isCheckedTir;
            }
            set
            {
                _isCheckedTir = value;
                NotifyOfPropertyChange(() => IsCheckedTue);
                Rate.DaysPeriod ^= Days.Tuesday;
            }
        }
        public bool IsCheckedWed
        {
            get
            {
                return _isCheckedOns;
            }
            set
            {
                _isCheckedOns = value;
                NotifyOfPropertyChange(() => IsCheckedWed);
                Rate.DaysPeriod ^= Days.Wednesday;
            }
        }
        public bool IsCheckedThu
        {
            get
            {
                return _isCheckedTor;
            }
            set
            {
                _isCheckedTor = value;
                NotifyOfPropertyChange(() => IsCheckedThu);
                Rate.DaysPeriod ^= Days.Thursday;
            }
        }
        public bool IsCheckedFri
        {
            get
            {
                return _isCheckedFre;
            }
            set
            {
                _isCheckedFre = value;
                NotifyOfPropertyChange(() => IsCheckedFri);
                Rate.DaysPeriod ^= Days.Friday;
            }
        }
        public bool IsCheckedSat
        {
            get
            {
                return _isCheckedLor;
            }
            set
            {
                _isCheckedLor = value;
                NotifyOfPropertyChange(() => IsCheckedSat);
                Rate.DaysPeriod ^= Days.Saturday;
            }
        }
        public bool IsCheckedSun
        {
            get
            {
                return _isCheckedSon;
            }
            set
            {
                _isCheckedSon = value;
                NotifyOfPropertyChange(() => IsCheckedSun);
                Rate.DaysPeriod ^= Days.Sunday;
            }
        }

        public void BtnCheckWorkDays()
        {
            IsCheckedMon = true;
            IsCheckedTue = true;
            IsCheckedWed = true;
            IsCheckedThu = true;
            IsCheckedFri = true;
            IsCheckedSat = false;
            IsCheckedSun = false;
        }
        public void BtnCheckAll()
        {
            IsCheckedMon = true;
            IsCheckedTue = true;
            IsCheckedWed = true;
            IsCheckedThu = true;
            IsCheckedFri = true;
            IsCheckedSat = true;
            IsCheckedSun = true;
        }
        public void BtnUnCheckAll()
        {
            IsCheckedMon = false;
            IsCheckedTue = false;
            IsCheckedWed = false;
            IsCheckedThu = false;
            IsCheckedFri = false;
            IsCheckedSat = false;
            IsCheckedSun = false;
        }

        public void BtnRemoveRateEntry()
        {
            _agreementVievModel.RateEntries.Remove(this);
        }

    }
}
