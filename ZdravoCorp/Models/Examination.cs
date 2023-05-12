using System;
using System.ComponentModel;

namespace ZdravoCorp.Models
{
    public class Examination
    {
        [Serializable]
        public enum Status
        {
            Scheduled,
            Ready,
            Finished,
        }
        public Status CurrentStatus { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public bool IsOperation { get; set; }
        public string DoctorUsername { get; set; }
        public string PatientUsername { get; set; }

        // constructors
        public Examination() { }
        public Examination(TimeSlot timeSlot, string doctorUsername, string patientUsername, bool isOperation, Status status)
        {
            TimeSlot = timeSlot;
            DoctorUsername = doctorUsername;
            PatientUsername = patientUsername;
            IsOperation = isOperation;
            CurrentStatus = status;
        }
        public Examination(TimeSlot timeSlot, string doctorUsername, string patientUsername)
        {
            TimeSlot = timeSlot;
            DoctorUsername = doctorUsername;
            PatientUsername = patientUsername;
            IsOperation = false; ;
            CurrentStatus = Status.Scheduled;
        }
    }
}