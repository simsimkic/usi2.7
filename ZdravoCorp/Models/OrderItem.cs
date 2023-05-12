using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Models
{
    public class OrderItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }

        public OrderItem(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }
    }
}
