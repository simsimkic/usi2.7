using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Storage;

namespace ZdravoCorp.Models.DAO
{
    public class DoctorDAO
    {
        private DoctorStorage _doctorsStorage;
        private List<Doctor> _doctors;
        private ExaminationDAO _examinationDAO;

        public DoctorDAO()
        {
            _examinationDAO = new ExaminationDAO();
            _doctorsStorage = new DoctorStorage();
            _doctors = _doctorsStorage.LoadDoctors();
        }
        
        public List<Doctor> GetDoctors()
        {
            return _doctors;
        }

        public List<Doctor> GetQualfiedDoctors(Specialization specialization)
        {
            List<Doctor> specializedDoctors = new List<Doctor>();
            foreach (var doctor in _doctors)
            {
                if (specialization == doctor.Specialization)
                {
                    specializedDoctors.Add(doctor);
                }
                
            }
            return specializedDoctors;
        }

        public Doctor GetDoctorByUsername(string username)
        {
            List<Doctor> _doctors = _doctorsStorage.LoadDoctors();
            foreach (var doctor in _doctors)
            {
                if (doctor.Username == username)
                {
                    return doctor;
                }

            }
            return null;
        }

        public Doctor? FindDoctorForAnamnesis(DateTime anamnesisDate, string PatientUsername)
        {
            foreach( var _doctor in _doctors)
            { 
                List<Examination> matchingExaminations = _examinationDAO.GetUserExaminations(_doctor.Username).Where(e => e.TimeSlot.DateTime == anamnesisDate).ToList();
                foreach (var examination in matchingExaminations)
                {
                    if (examination.PatientUsername.Equals(PatientUsername))
                    {
                       ;

                        return _doctor;
                    }
                }
               
            }
            return null;
        }
    }
}
