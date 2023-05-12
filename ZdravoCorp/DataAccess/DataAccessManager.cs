using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using ZdravoCorp.Models;
using Constants = ZdravoCorp.Helper.Constants;

namespace ZdravoCorp.DataAccess
{
    public class DataAccessManager
    {
        public static ObservableCollection<Examination> LoadExaminations(string username)
        {
            var json = File.ReadAllText(Constants.ExaminationsFilePath);
            var loadedExaminations = JsonSerializer.Deserialize<ObservableCollection<Examination>>(json);
            return new ObservableCollection<Examination>(
                (loadedExaminations ?? new ObservableCollection<Examination>()).Where(examination =>
                    examination.DoctorUsername == username));
        }
        public static List<Patient>? LoadPatients()
        {
            List<Patient>? patients;
            var json = File.ReadAllText(Constants.PatientsFilePath);
            patients = !string.IsNullOrEmpty(json) ? JsonSerializer.Deserialize<List<Patient>>(json) : new List<Patient>();

            return patients;
        }

        public static void SaveExaminations(ObservableCollection<Examination> examinations)
        {
            var json = JsonSerializer.Serialize(examinations, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Constants.ExaminationsFilePath, json);
        }

        public static void SavePatients(List<Patient> patients)
        {
            var json = JsonSerializer.Serialize(patients, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Constants.PatientsFilePath, json);
        }
        public static void SavePatient(Patient patient)
        {
            //string path = Path.Combine("..", "Data", "Patient.json");
            List<Patient> patients = LoadPatients();

            // Search for an object with the same ID and replace it if found
            if (patients == null) return;
            patients = UpdatePatients(patient, patients);

            // Serialize the updated collection back to JSON format and save it to the file

            var json = JsonSerializer.Serialize(patients);
            File.WriteAllText(Constants.PatientsFilePath, json);
        }
        private static List<Patient> UpdatePatients(Patient patient, List<Patient> patients)
        {
            Patient existingPatient = patients.FirstOrDefault(p => p.Username == patient.Username) ?? new Patient();
            {
                var index = patients.IndexOf(existingPatient);
                patients[index] = patient;
            }
            return patients;
        }
    }
}
