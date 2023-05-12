using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Models
{
    public class Person
    {
        // Properties
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime Birthday { get; set; }
        public string? Gender { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        // Constructor
        public Person(string name, string surname, DateTime birthday, string gender, string username, string password)
        {
            Name = name;
            Surname = surname;
            Birthday = birthday;
            Gender = gender;
            Username = username;
            Password = password;
        }
    }


}
