using System;
using System.Collections.Generic;

namespace ZdravoCorp.Models
{
    [Serializable]
    public class Anamnesis
    {
        public DateTime Date { get; set; }
        public string Symptoms { get; set; }
        public string Conclusion { get; set; }

        // empty constructor for serialization
        public Anamnesis() { }

        public Anamnesis(string symptoms, string conclusion)
        {
            Symptoms = symptoms;
            Date = DateTime.Now;
            Conclusion = conclusion;
        }

        public Anamnesis(DateTime date, string symptoms, string conclusion)
        {
            Symptoms = symptoms;
            Date = date;
            Conclusion = conclusion;
        }
    }
}