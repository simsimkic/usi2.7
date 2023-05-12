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
    internal class OrderStorage
    {
        private const string Path = "../../../Data/orders.json";

        public static IEnumerable<Order> Load()
        {
            var jsonString = File.ReadAllText(Path);
            var data = JsonSerializer.Deserialize<IEnumerable<Order>>(jsonString);

            if (data == null) return Enumerable.Empty<Order>();
            return data;
        }

        public static void Save(IEnumerable<Order> orders)
        {
            var jsonString = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(Path, jsonString);
        }
    }
}
