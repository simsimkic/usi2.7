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
    internal class RoomJsonStorage : IRoomStorage
    {
        private const string Path = "../../../Data/rooms.json";
        public IEnumerable<Room> Load()
        {
            var jsonString = File.ReadAllText(Path);
            var data = JsonSerializer.Deserialize<IEnumerable<Room>>(jsonString);

            if (data == null) return Enumerable.Empty<Room>();
            return data;
        }

        public void Save(IEnumerable<Room> equipment)
        {
            var jsonString = JsonSerializer.Serialize(equipment, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path, jsonString);
        }
    }
}
