using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZdravoCorp.Models
{
    public class Order
    {
        public Guid Id { get; }
        public DateTime OrderDate { get; }
        public DateTime ArrivalDate { get; }
        public bool IsCompleted { get; set; }
        public List<OrderItem> Items { get; }

        // The first constructor is used when we are creating a new order,
        // and the second is used when reading orders from a json file.
        public Order(List<OrderItem> items)
        {
            Id = Guid.NewGuid();
            OrderDate = DateTime.Now;
            ArrivalDate = OrderDate.AddDays(1);
            Items = items;
            IsCompleted = false;
        }

        [JsonConstructorAttribute]
        public Order(Guid id, DateTime orderDate, DateTime arrivalDate, List<OrderItem> items, bool isCompleted)
        {
            Id = id;
            OrderDate = orderDate;
            ArrivalDate = arrivalDate;
            Items = items;
            IsCompleted = isCompleted;
        }
    }
}
