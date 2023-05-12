using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Storage;

namespace ZdravoCorp.Models.DAO
{
    internal class RoomDAO
    {
        private IRoomStorage _storage;
        private List<Room> _rooms;

        public RoomDAO()
        {
            _storage = new RoomJsonStorage();
            _rooms = _storage.Load().ToList();
        }

        public IEnumerable<Room> GetRooms() => _rooms;

        public Room? GetRoom(int id) => _rooms.FirstOrDefault(r => r.Id == id);

        public Room? GetStorageRoom() => _rooms.FirstOrDefault(r => r.Type == RoomType.Storage);
    }
}
