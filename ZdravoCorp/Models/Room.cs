using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Models
{
    public class Room
    {
        public int Id { get; set; }
        public RoomType Type { get; set; }

        public Room(int id, RoomType type)
        {
            Id = id;
            Type = type;
        }

        public override string? ToString()
        {
            return $"{Id} {Type}";
        }
    }

    public enum RoomType
    {
        ExamRoom,
        WaitingRoom,
        Ward,
        OperatingTheatre,
        Storage
    }
}
