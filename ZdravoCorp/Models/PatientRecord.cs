using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Xml.Serialization;

namespace ZdravoCorp.Models
{
    [Serializable]
    public class PatientRecord
    {
        public double Height { get; set; }
        public double Weight { get; set; }
        public List<string> MedicalHistory { get; set; }
        public List<Anamnesis> Anamnesis { get; set; }
        public List<string> Allergens { get; set; }

        //public PatientRecord(List<string> medicalHistory)
        //{
        //    MedicalHistory = new List<string>();
        //    Anamnesis = new List<Anamnesis>();
        //}

        public PatientRecord(double height, double weight, List<string>? medicalHistory, List<Anamnesis>? anamneses, List<string> allergens)
        {
            Height = height;
            Weight = weight;
            MedicalHistory = medicalHistory;
            Anamnesis = anamneses;
            Allergens = allergens;
        }

        public PatientRecord()
        {

        }

        public PatientRecord(double height, double weight, List<string> medicalHistory, List<string> allergens)
        {
            Height = height;
            Weight = weight;
            Anamnesis = new List<Anamnesis>();
            MedicalHistory = medicalHistory;
            Allergens = allergens;          
        }
      
    }

}
