using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ZdravoCorp.Models;

namespace ZdravoCorp.Storage
{
    internal class NotificationStorage
    {
        public readonly string NotificationJsonPath = "../../../Data/notifications.json";
        public List<Notification> LoadNotifications()
        {
            List<Notification> notifications;
            string json = File.ReadAllText(NotificationJsonPath);
            if (!string.IsNullOrEmpty(json))
            {
                notifications = JsonSerializer.Deserialize<List<Notification>>(json);
            }
            else
            {
                notifications = new List<Notification>();
            }

            return notifications;
        }

        public void SaveNotifications(List<Notification> notifications)
        {
            string json = JsonSerializer.Serialize(notifications, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(NotificationJsonPath, json);
        }
    }
}
