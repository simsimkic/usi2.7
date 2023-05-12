using System;
using System.Collections.ObjectModel;
using ZdravoCorp.DataAccess;

namespace ZdravoCorp.Models
{
    [Serializable]
    public enum Specialization
    {
        GeneralMedicine,
        Cardiology,
        Surgery
    }
    public class Doctor : User
    {
        public Specialization Specialization { get; set; }
        // upitno da li je potrebno, trenutno bi trebalo da se ne koristi
        public ObservableCollection<Examination> Examinations { get; set; }
        public Doctor() { }
        public Doctor(string? username, string? password, string? firstName, string lastName, DateTime dateOfBirth, Gender gender, ObservableCollection<Examination> examinations, Specialization specialization) : base(username, password, firstName, lastName, dateOfBirth, gender)
        {
            Examinations = DataAccessManager.LoadExaminations(username);
            Specialization = specialization;
        }

    }
}
