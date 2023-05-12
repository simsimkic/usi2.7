using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace ZdravoCorp.Models
{
    public class Nurse : User
    {

        public Nurse(string username, string password, string firstName, string lastName, DateTime dateOfBirth, Gender gender)
            : base(username, password, firstName, lastName, dateOfBirth, gender)
        {
        }

        public Nurse() : base()
        {
        }

    }
}
