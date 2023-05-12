using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Storage;

namespace ZdravoCorp.Models.DAO
{
    public class NotificationDAO
    {
        private NotificationStorage _notificationsStorage;
        private List<Notification> _notifications;

        public NotificationDAO()
        {
            _notificationsStorage = new NotificationStorage();
            _notifications = _notificationsStorage.LoadNotifications();
        }

        public List<Notification> GetNotifications()
        {
            return _notifications;
        }
        public void AddNotification(Notification notification)
        {
            _notifications.Add(notification);
            _notificationsStorage.SaveNotifications(_notifications);
        }

        public void DeleteNotification(Notification notification)
        {
            foreach (var _notification in _notifications)
            {
                if(_notification.ID == notification.ID)
                {
                    _notifications.Remove(_notification);
                    break;
                }
            }
            _notificationsStorage.SaveNotifications(_notifications);
        }

        public bool HasNotification(string username)
        {
            foreach (var notification in _notifications)
            {
                if (notification.Username == username)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
