using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Storage;

namespace ZdravoCorp.Models.DAO
{
    public class PatientDAO
    {
        private PatientStorage _patientStorage;
        private List<Patient> _patients;

        public PatientDAO()
        {
            _patientStorage = new PatientStorage();
            _patients = _patientStorage.LoadPatients();
        }

        public List<Patient> GetPatients()
        {
            return _patients;
        }
        public void CreatePatient(Patient patient)
        {
            _patients = UpdatePatients(patient, _patients);
            _patientStorage.SavePatients(_patients);
        }

        public Patient GetPatientByUsername(string username)
        {
            List<Patient> _patients = _patientStorage.LoadPatients();
            foreach (var patient in _patients)
            {
                if (patient.Username == username)
                {
                    return patient;
                }

            }
            return null;
        }

        public  PatientRecord GetPatientsRecordByUsername(string username)
        {
            Patient patient = GetPatientByUsername(username);
            PatientRecord patientRecord = patient.PatientRecord;
            return patientRecord;
        }

        public void DeletePatient(Patient patient)
        {
            _patients = _patientStorage.LoadPatients();
            foreach (var p in _patients)
            {
                if (p.Username == patient.Username)
                {
                    _patients.Remove(p);
                    break;
                }
            }
            _patientStorage.SavePatients(_patients);
            return;
        }

        public bool IsUsernameTaken(string username)
        {

            List<Patient> _patients = _patientStorage.LoadPatients();
            foreach (var patient in _patients)
            {
                if (patient.Username == username)
                {
                    return true;
                }
            }
            return false;
        }

        public List<string> ParseAllergensAndHistory(string data)
        {
            List<string> parsedData = data.Split(',').ToList();
            for (int i = 0; i < parsedData.Count; i++)
            {
                parsedData[i].Trim();
                if (string.IsNullOrWhiteSpace(parsedData[i]))
                {
                    parsedData.RemoveAt(i);
                    i--;
                }
            }
            return parsedData;
        }

        //Update list of patients by inserting new or existing patient
        public List<Patient> UpdatePatients(Patient patient, List<Patient> patients)
        {
            Patient existingPatient = patients.FirstOrDefault(p => p.Username == patient.Username);
            if (existingPatient != null)
            {
                int index = patients.IndexOf(existingPatient);
                patients[index] = patient;
            }
            else
            {
                // Add the new object to the collection
                patients.Add(patient);
            }
            return patients;
        }
    }
}
