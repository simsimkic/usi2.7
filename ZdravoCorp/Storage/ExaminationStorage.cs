using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ZdravoCorp.Models;

namespace ZdravoCorp.Storage
{
    internal class ExaminationStorage
    {
        public readonly string ExaminationJsonPath = "../../../Data/examinations.json";
        public void SaveExaminations(List<Examination> examinations)
        {
            string json = JsonSerializer.Serialize(examinations, new JsonSerializerOptions { WriteIndented = true});
            File.WriteAllText(ExaminationJsonPath, json);
        }
        private const string Path = "../../../Data/examinations.json";

        public List<Examination> LoadExaminations()
        {
            List<Examination> examinations;
            string json = File.ReadAllText(ExaminationJsonPath);
            if (!string.IsNullOrEmpty(json))
            {
                examinations = JsonSerializer.Deserialize<List<Examination>>(json);
            }
            else
            {
                examinations = new List<Examination>();
            }

            return examinations;
        }
        public ObservableCollection<Examination> LoadExaminations(string username)
        {
            var json = File.ReadAllText(Path);
            var loadedExaminations = JsonSerializer.Deserialize<ObservableCollection<Examination>>(json);
            return new ObservableCollection<Examination>(
                (loadedExaminations ?? new ObservableCollection<Examination>()).Where(examination =>
                    examination.DoctorUsername == username));
        }
        public void SaveExaminations(ObservableCollection<Examination> examinations)
        {
            var json = JsonSerializer.Serialize(examinations, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path, json);
        }
    }
}
