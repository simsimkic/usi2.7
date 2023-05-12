using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Models
{
    internal class TransferItem
    {
        public string Name { get; set; }
        public int Quantity { get; set; }

        public TransferItem(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }
    }
}
