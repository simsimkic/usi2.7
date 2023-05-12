using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Models
{
    internal class Transfer
    {
        public Guid Id { get; }
        public Room FromRoom { get; }
        public Room ToRoom { get; }
        public TransferItem Item { get; }
        public int Quantity { get; }
        public DateTime ExecutionDate { get; }
        public bool IsCompleted { get; set; }

        public Transfer(Guid id, Room fromRoom, Room toRoom, TransferItem item, DateTime executionDate)
        {
            Id = id;
            FromRoom = fromRoom;
            ToRoom = toRoom;
            Item = item;
            ExecutionDate = executionDate;
            IsCompleted = false;
        }
    }
}