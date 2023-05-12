using Newtonsoft.Json;
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
    internal class EquipmentJsonStorage : IEquipmentStorage
    {
        private const string Path = "../../../Data/equipment.json";
        public IEnumerable<Equipment> Load()
        {
            var json = File.ReadAllText(Path);
            var data = JsonConvert.DeserializeObject<IEnumerable<Equipment>>(json);

            if (data == null) return Enumerable.Empty<Equipment>();
            return data;
        }

        public void Save(IEnumerable<Equipment> equipment)
        {
            var json = JsonConvert.SerializeObject(equipment, Formatting.Indented);
            File.WriteAllText(Path, json);
        }
    }
}
