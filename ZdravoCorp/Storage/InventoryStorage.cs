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
    public class InventoryStorage
    {
        private const string Path = "../../../Data/inventory.json";

        public static IEnumerable<InventoryItem> Load()
        {
            var jsonString = File.ReadAllText(Path);
            var data = JsonSerializer.Deserialize<IEnumerable<InventoryItem>>(jsonString);

            if (data == null) return Enumerable.Empty<InventoryItem>();
            return data;
        }

        public static void Save(IEnumerable<InventoryItem> orders)
        {
            var jsonString = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path, jsonString);
        }
    }
}
