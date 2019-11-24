using SW3Projekt.ViewModels;
using System;
using System.Collections.Generic;
using System.Timers;

namespace SW3Projekt.Models
{
    public class Notification
    {
        // Constants used to define the max string lengths to avoid text spilling over the edges.
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
                if (value.Length >= MAX_MAIN_LENGTH)
                {
                    _mainTxt = value.Substring(0, MAX_MAIN_LENGTH);
                }
                else
                {
                    _mainTxt = value;
                }
            }
        }

        // Reference to the view model where the notification is being displayed.
        public NotificationViewModel NotiViewModel;

        public enum NotificationType { Warning, Error, Edited, Added, Removed }
        private readonly Dictionary<NotificationType, string> NotificationTitles = new Dictionary<NotificationType, string>() {
            { NotificationType.Warning, "Advarsel" },
            { NotificationType.Error, "Fejl" },
            { NotificationType.Edited, "Redigeret" },
            { NotificationType.Added, "Tilføjet" },
            { NotificationType.Removed, "Slettet" }
        };
        private string GetNotificationTitle (NotificationType type) => (NotificationTitles.ContainsKey(type) ? NotificationTitles[type] : "Udefineret title");

        #region Contructors
        public Notification(NotificationType type, string mainTxt, float timeShowingInSeconds = 5)
        {
            this.TitleTxt = GetNotificationTitle(type);
            this.MainTxt = mainTxt;
            SetupNotifikation(timeShowingInSeconds);
        }

        public Notification(string customTitle, string mainTxt, float timeShowingInSeconds = 5)
        {
            this.TitleTxt = customTitle;
            this.MainTxt = mainTxt;
            SetupNotifikation(timeShowingInSeconds);
        }
        #endregion

        private void SetupNotifikation(float timeShowingInSeconds)
        {
            // Instantiate a new timer and hook it to the DeleteElapsed method.
            Timer timer = new Timer(timeShowingInSeconds * 1000);
            timer.Elapsed += DeleteElapsed;
            timer.AutoReset = true;
            timer.Enabled = true;

            // Instantiate the actual view model to display the notification.
            NotiViewModel = new NotificationViewModel(this);

            // Let the instance of the ShellViewModel know of the new notification so it appears on screen.
            ShellViewModel.Singleton.DBNotifications.Add(new DBNotification()
            {
                Type = TitleTxt,
                Message = MainTxt,
                Date = DateTime.Now
            }); ;
            ShellViewModel.Singleton.NotificationList.Add(NotiViewModel);
            ShellViewModel.Singleton.NotifyOfPropertyChange(() => ShellViewModel.Singleton.Notifications);
        }

        private void DeleteElapsed(object sender, EventArgs e)
        {
            DeleteNotification();
        }

        public void DeleteNotification()
        {
            // Lock the list while removing to avoid multiple threads accessing the list at the same time.
            lock (ShellViewModel.Singleton.NotificationList)
            {
                if (ShellViewModel.Singleton.NotificationList.Contains(NotiViewModel))
                {
                    ShellViewModel.Singleton.NotificationList.Remove(NotiViewModel);
                    ShellViewModel.Singleton.NotifyOfPropertyChange(() => ShellViewModel.Singleton.Notifications);
                }
            }
        }
    }
}
