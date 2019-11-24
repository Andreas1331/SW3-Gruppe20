using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Globalization;
using SW3Projekt.Tools;

namespace SW3Projekt.ViewModels
{
    public class NotificationsViewModel : Conductor<object>
    {
        public List<DBNotification> NotificationList 
        {
            get 
            {
                return ShellViewModel.Singleton.DBNotifications;
            }
        }
        public BindableCollection<NotificationPageNotificationViewModel> Notifications { get; set; } = new BindableCollection<NotificationPageNotificationViewModel>();

        public NotificationsViewModel()
        {
            foreach (DBNotification notification in NotificationList) 
            {
                Notifications.Add(new NotificationPageNotificationViewModel(this, notification));
            }
        }
    }
}
