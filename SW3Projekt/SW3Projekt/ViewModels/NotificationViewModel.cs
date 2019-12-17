using Caliburn.Micro;
using SW3Projekt.Models;
using System;

namespace SW3Projekt.ViewModels
{
    public class NotificationViewModel : Screen
    {
        private Notification _activeNotification;
        public Notification ActiveNotification
        {
            get
            {
                return _activeNotification;
            }
            set
            {
                _activeNotification = value;
            }
        }

        public NotificationViewModel(Notification _notification)
        {
            ActiveNotification = _notification;
            _notification.NotiViewModel = this;
        }

        public void BtnDeleteNotification()
        {
            ActiveNotification.DeleteNotification();
        }
    }
}
