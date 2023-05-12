using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.Models;
using ZdravoCorp.Models.DAO;

namespace ZdravoCorp.ViewModels
{
    internal class NotificationViewModel : ViewModelBase
    {
        private NotificationDAO _notificationDAO = new NotificationDAO();

        private string _notificationText;
        public string NotificationText
        {
            get => _notificationText;
            set
            {
                if (_notificationText == value) return;
                _notificationText = value;
                OnPropertyChanged(nameof(NotificationText));
            }
        }

        public NotificationViewModel(string username)
        {
            List<Notification> chosenNotifications = new List<Notification>();
            foreach (var notification in _notificationDAO.GetNotifications())
            {
                
                if (notification.Username == username)
                {
                    chosenNotifications.Add(notification);
                    _notificationText += notification.Message + "\n";                   
                }
            }
            foreach (var notification in chosenNotifications)
            {
                _notificationDAO.DeleteNotification(notification);
            }
        }

    }
}
