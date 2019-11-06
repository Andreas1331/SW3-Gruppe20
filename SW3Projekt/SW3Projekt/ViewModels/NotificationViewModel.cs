using Caliburn.Micro;
using SW3Projekt.Notification;
using System;
using System.Timers;
using System.Windows.Threading;

namespace SW3Projekt.ViewModels
{
    public class NotificationViewModel : Screen
    {
        private static int MAX_TITLE_LENGTH = 16;
        private static int MAX_MAIN_LENGTH = 128;

        private string _titleTxt = "";
        public string TitleTxt
        {
            get
            {
                return _titleTxt;
            }
            set
            {
                // Ensure the title length is not exceeding the available space on the view.
                if (value.Length >= MAX_TITLE_LENGTH)
                {
                    _titleTxt = value.Substring(0, MAX_TITLE_LENGTH);
                }
                else
                {
                    _titleTxt = value;
                }

                NotifyOfPropertyChange(() => TitleTxt);
            }
        }

        private string _mainTxt = "";
        public string MainTxt
        {
            get
            {
                return _mainTxt;
            }
            set
            {
                // Ensure the main length is not exceeding the available space on the view.
                if(value.Length >= MAX_MAIN_LENGTH)
                {
                    _mainTxt = value.Substring(0, MAX_MAIN_LENGTH);
                }
                else
                {
                    _mainTxt = value;
                }

                NotifyOfPropertyChange(() => MainTxt);
            }
        }

        private float _startTimeValue;
        private float _timeLeftBeforeDeletion;

        private float _statusBarValue = 0;
        public float StatusBarValue
        {
            get { 
                return _statusBarValue; 
            }
            set
            {
                _statusBarValue = value;
                NotifyOfPropertyChange(() => StatusBarValue);
            }
        }

        public NotificationViewModel(string titleTxt, string mainTxt, float timeShowingInSeconds)
        {
            this.TitleTxt = titleTxt;
            this.MainTxt = mainTxt;

            // (60 * 10 = 600) * 15 = 9000 ~ 10000
            _startTimeValue = (timeShowingInSeconds * 60);
            _timeLeftBeforeDeletion = (timeShowingInSeconds * 60);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(15);
            timer.Tick += DeleteTick;
            timer.Start();
        }

        public void BtnDeleteNotification()
        {
            NotificationsHandler.DeleteNotification(this);
        }

        private void OnDeleteTimerElapsed(object source, ElapsedEventArgs e)
        {
            // Delete the notification
            BtnDeleteNotification();
        }

        // Get called every second
        private void DeleteTick(object sender, EventArgs e)
        {
            _timeLeftBeforeDeletion -= 1;
            StatusBarValue = (_timeLeftBeforeDeletion / _startTimeValue) * 100;
            if(_timeLeftBeforeDeletion <= 0)
            {
                BtnDeleteNotification();
            }
        }
    }
}
