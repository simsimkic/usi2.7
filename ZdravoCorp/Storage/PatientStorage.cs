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
    internal class PatientStorage
    {
        public readonly string PatientJsonPath = "../../../Data/patients.json";
        public List<Patient> LoadPatients()
        {
            List<Patient> patients;
            string json = File.ReadAllText(PatientJsonPath);
            if (!string.IsNullOrEmpty(json))
            {
                patients = JsonSerializer.Deserialize<List<Patient>>(json);
            }
            else
            {
                patients = new List<Patient>();
            }

            return patients;
        }

        public void SavePatients(List<Patient> patients)
        {
            string json = JsonSerializer.Serialize(patients, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(PatientJsonPath, json);
        }

        
    }
}
