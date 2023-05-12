using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Models
{
    [Serializable]
    public class Notification
    {

        public string ID { get; set; }
        public string Message { get; set; }
        public string Username { get; set; }

        private string generateID()
        {
            return Guid.NewGuid().ToString("N");
        }
        public Notification(string message, string username)
        {
            this.ID = generateID();
            this.Message = message;
            this.Username = username;
        }

        public Notification(string id, string message, string username)
        {
            this.ID = id;
            this.Message = message;
            this.Username = username;
        }
        public Notification() { }

    }
}
