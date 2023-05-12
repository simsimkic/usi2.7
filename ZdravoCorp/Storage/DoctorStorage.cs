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
    internal class DoctorStorage
    {
        public readonly string DoctorJsonPath = "../../../Data/doctors.json";
        public List<Doctor> LoadDoctors()
        {
            List<Doctor> doctors;
            string json = File.ReadAllText(DoctorJsonPath);
            if (!string.IsNullOrEmpty(json))
            {
                doctors = JsonSerializer.Deserialize<List<Doctor>>(json);
            }
            else
            {
                doctors = new List<Doctor>();
            }

            return doctors;
        }

        public void SaveDoctors(List<Doctor> doctors)
        {
            string json = JsonSerializer.Serialize(doctors, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(DoctorJsonPath, json);
        }
    }
}
