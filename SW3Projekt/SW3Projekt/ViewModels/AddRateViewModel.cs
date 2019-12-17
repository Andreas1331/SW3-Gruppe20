using Caliburn.Micro;
using SW3Projekt.Models;
using SW3Projekt.Tools;
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
        // Propterties
        public Rate Rate { get; set; } = new Rate();
        private AddAgreementViewModel _agreementVievModel;
        // Properties for the view
        public bool IsNameReadOnly { get; set; } = false;
        public bool IsVismaIdActive{ get; set; } = false;
        public bool IsStartTimeActive { get; set; } = true;
        public bool IsEndTimeActive { get; set; } = true;
        public string IsToolTipVisible { get; set; } = "Visible";
        public string TooltipText { get; set; } = "Skal en rate gælde en hel dag, skal både start- og sluttid sættes til 00:00";
        public bool IsValueActive { get; set; } = true;
        public bool IsDaysCheckBoxsActive { get; set; } = true;
        public bool IsRemoveBtnActive { get; set; } = true;
        // Design properties
        public double ShadowRadius { get; set; } = GraphicalDesign.ShadowRadius;
        public double ShadowDirection { get; set; } = GraphicalDesign.ShadowDirection;
        public double ShadowDepth { get; set; } = GraphicalDesign.ShadowDepth;
        public double ShadowOpacity { get; set; } = GraphicalDesign.ShadowOpacity;
        // Timepicker properties
        public DateTime StartTimePicker
        {
            get
            {
                return Rate.StartTime;
            }
            set
            {
                Rate.StartTime = value;
            }
        }
        public DateTime EndTimePicker
        {
            get
            {
                return Rate.EndTime;
            }
            set
            {
                Rate.EndTime = value;
            }
        }
        // Checkbox properties
        public bool IsCheckedMon
        {
            get
            {
                return Rate.CheckFlag(Days.Monday);
            }
            set
            {
                NotifyOfPropertyChange(() => IsCheckedMon);
                if (value && Rate.CheckFlag(Days.Monday) || !value && !Rate.CheckFlag(Days.Monday))
                    return;

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
                if (value && Rate.CheckFlag(Days.Tuesday) || !value && !Rate.CheckFlag(Days.Tuesday))
                    return;

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
                if (value && Rate.CheckFlag(Days.Wednesday) || !value && !Rate.CheckFlag(Days.Wednesday))
                    return;

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
                if (value && Rate.CheckFlag(Days.Thursday) || !value && !Rate.CheckFlag(Days.Thursday))
                    return;

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
                if (value && Rate.CheckFlag(Days.Friday) || !value && !Rate.CheckFlag(Days.Friday))
                    return;

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
                if (value && Rate.CheckFlag(Days.Saturday) || !value && !Rate.CheckFlag(Days.Saturday))
                    return;

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
                if (value && Rate.CheckFlag(Days.Sunday) || !value && !Rate.CheckFlag(Days.Sunday))
                    return;

                Rate.DaysPeriod ^= Days.Sunday;
            }
        }

        // CONSTRUCTORS MADE TO SPECIFICALLY CHOSE WHICH FIELDS SHOULD BE ACTIVE
        // CONSTRUCTOR USING AGREEMENT VM (NEW AGREEMENT)
        public AddRateViewModel(AddAgreementViewModel agvm, bool isNameActive, bool isVismaIdActive, bool isStartTimeActive, bool isEndTimeActive, bool isValueActive, bool isDaysActive, bool isRemoveBtnActive)
        {
            _agreementVievModel = agvm;
            BtnCheckWorkDays();
            Rate.Type = "Arbejde";
            IsNameReadOnly = isNameActive;
            IsVismaIdActive = isVismaIdActive;
            IsStartTimeActive = isStartTimeActive;
            IsEndTimeActive = isEndTimeActive;
            IsToolTipVisible = "Visible";
            IsValueActive = isValueActive;
            IsDaysCheckBoxsActive = isDaysActive;
            IsRemoveBtnActive = isRemoveBtnActive;
        }

        // CONSTRUCTOR USING RATE (VIEWING AGREEMENT)
        public AddRateViewModel(Rate rate, bool isNameActive, bool isVismaIdActive, bool isStartTimeActive, bool isEndTimeActive, bool isValueActive, bool isDaysActive, bool isRemoveBtnActive)
        {
            Rate = rate;
            IsNameReadOnly = isNameActive;
            IsVismaIdActive = isVismaIdActive;
            IsStartTimeActive = isStartTimeActive;
            IsEndTimeActive = isEndTimeActive;
            IsToolTipVisible = "Hidden";
            IsValueActive = isValueActive;
            IsDaysCheckBoxsActive = isDaysActive;
            IsRemoveBtnActive = isRemoveBtnActive;
        }

        // Methods
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
