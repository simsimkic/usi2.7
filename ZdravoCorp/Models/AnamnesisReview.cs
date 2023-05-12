using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Models
{
    public class AnamnesisReview
    {
        public DateTime ExaminationDate { get; set; }
        public string DoctorUsername { get; set; }
        public Specialization DoctorSpecialization { get; set; }
        public string Symptoms { get; set; }
        public string Conclusion { get; set; }

        public string SymptomsAndConclusion
        {
            get
            {
                return $"{Symptoms}; {Conclusion}";
            }
        }

        public AnamnesisReview(Anamnesis anamnesis, Doctor doctor)
        {
            ExaminationDate = anamnesis.Date;
            DoctorUsername = doctor.Username;
            DoctorSpecialization = doctor.Specialization;
            Symptoms = anamnesis.Symptoms;
            Conclusion = anamnesis.Conclusion;
        }
    }
}
