using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.DataAccess;
using ZdravoCorp.Models;

namespace ZdravoCorp.Models
{
    [Serializable]
    public class Patient : User
    {
        public PatientRecord PatientRecord { get; set; }

        public Patient(string? username, string? password, string? firstName, string lastName, DateTime dateOfBirth,
            Gender gender, PatientRecord patientRecord)
            : base(username, password, firstName, lastName, dateOfBirth, gender)
        {
            PatientRecord = patientRecord;

        }

        public Patient()
        {
            PatientRecord = new PatientRecord();
        }

    }


}
