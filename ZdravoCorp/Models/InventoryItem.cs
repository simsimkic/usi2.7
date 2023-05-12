using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Models
{
    public class InventoryItem
    {
        public Equipment Equipment { get; set; }
        public Room Room { get; set; }
        public int Quantity { get; set; }

        public InventoryItem(Equipment equipment, Room room, int quantity)
        {
            Equipment = equipment;
            Room = room;
            Quantity = quantity;
        }
    }
}
