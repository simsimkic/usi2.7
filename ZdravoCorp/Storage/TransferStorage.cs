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
    internal class TransferStorage
    {
        private const string Path = "../../../Data/transfers.json";

        public static IEnumerable<Transfer> Load()
        {
            var jsonString = File.ReadAllText(Path);
            var data = JsonSerializer.Deserialize<IEnumerable<Transfer>>(jsonString);

            if (data == null) return Enumerable.Empty<Transfer>();
            return data;
        }

        public static void Save(IEnumerable<Transfer> orders)
        {
            var jsonString = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path, jsonString);
        }
    }
}
