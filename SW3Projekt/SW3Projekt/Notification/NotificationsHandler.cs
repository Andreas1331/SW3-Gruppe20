using SW3Projekt.Models;
using SW3Projekt.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.Notification
{
    public static class NotificationsHandler
    {
        private static ShellViewModel _shellInstance;
        public static ShellViewModel ShellInstance
        {
            private get
            {
                return _shellInstance;
            }
            set
            {
                _shellInstance = value;
            }
        }

        public static void GiveNotification(string titleTxt, string mainTxt, float timeShowingInSeconds = 10)
        {
            NotificationViewModel noti = new NotificationViewModel(titleTxt, mainTxt, timeShowingInSeconds);
            ShellInstance.NotificationList.Add(noti);
            ShellInstance.NotifyOfPropertyChange(() => ShellInstance.Notifications);
        }

        public static void DeleteNotification(NotificationViewModel notification)
        {
            ShellInstance.NotificationList.Remove(notification);
            ShellInstance.NotifyOfPropertyChange(() => ShellInstance.Notifications);
        }
    }
}
