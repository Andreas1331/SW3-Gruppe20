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

        public bool IsCheckedMon
        {
            get
            {
                return Rate.CheckFlag(Days.Monday);
            }
            set
            {
                NotifyOfPropertyChange(() => IsCheckedMon);
                Rate.DaysPeriod ^= Days.Monday;
            }
        }
        public bool IsCheckedTue
        {
            get
            {
                return Rate.CheckFlag(Days.Tuesday);
            }
            set
            {
                NotifyOfPropertyChange(() => IsCheckedTue);
                Rate.DaysPeriod ^= Days.Tuesday;
            }
        }
        public bool IsCheckedWed
        {
            get
            {
                return Rate.CheckFlag(Days.Wednesday);
            }
            set
            {
                NotifyOfPropertyChange(() => IsCheckedWed);
                Rate.DaysPeriod ^= Days.Wednesday;
            }
        }
        public bool IsCheckedThu
        {
            get
            {
                return Rate.CheckFlag(Days.Thursday);
            }
            set
            {
                NotifyOfPropertyChange(() => IsCheckedThu);
                Rate.DaysPeriod ^= Days.Thursday;
            }
        }
        public bool IsCheckedFri
        {
            get
            {
                return Rate.CheckFlag(Days.Friday);
            }
            set
            {
                NotifyOfPropertyChange(() => IsCheckedFri);
                Rate.DaysPeriod ^= Days.Friday;
            }
        }
        public bool IsCheckedSat
        {
            get
            {
                return Rate.CheckFlag(Days.Saturday);
            }
            set
            {
                NotifyOfPropertyChange(() => IsCheckedSat);
                Rate.DaysPeriod ^= Days.Saturday;
            }
        }
        public bool IsCheckedSun
        {
            get
            {
                return Rate.CheckFlag(Days.Sunday);
            }
            set
            {
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
