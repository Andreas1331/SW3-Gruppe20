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
    public class NotificationPageNotificationViewModel : Screen
    {
        private string _typebox;
        public string TypeBox 
        {
            get 
            {
                return _typebox;
            }
            set 
            {
                _typebox = value;
                NotifyOfPropertyChange(()=>TypeBox);
            }
        }

        private string _messageBox;
        public string MessageBox
        {
            get 
            {
                return _messageBox;
            }
            set 
            {
                _messageBox = value;
                NotifyOfPropertyChange(()=> MessageBox);
            }
        }

        private string _dateBox;
        public string DateBox
        {
            get 
            {
                return _dateBox;
            }
            set 
            {
                _dateBox = value;
                NotifyOfPropertyChange(()=> DateBox);
            }
        }

        public NotificationsViewModel Page;
        DBNotification Notification { get; set; }

        public NotificationPageNotificationViewModel(NotificationsViewModel Page, DBNotification notification)
        {
            this.Page = Page;
            Notification = notification;
            TypeBox = Notification.Type;
            MessageBox = Notification.Message;
            DateBox = Notification.Date.ToString();
        }

        public void RemoveButton() 
        {
            ShellViewModel.Singleton.DBNotifications.Remove(Notification);
            Page.Notifications.Remove(this);
        }
    }
}
