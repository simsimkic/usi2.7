using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models;
using ZdravoCorp.Models.DAO;

namespace ZdravoCorp.Services
{
    public class RoomService
    {
        private readonly RoomDAO _roomDAO = new();

        public IEnumerable<Room> GetRooms()
        {
            return _roomDAO.GetRooms();
        }

        public Room? GetRoom(int id)
        {
            return _roomDAO.GetRoom(id);
        }

        public Room? GetStorageRoom()
        {
            return _roomDAO.GetStorageRoom();
        }
    }
}
