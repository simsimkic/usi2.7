using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models;

namespace ZdravoCorp.ViewModels
{
    internal class RoomViewModel : ViewModelBase
    {
        protected readonly Room _room;
        public int Id => _room.Id;
        public RoomType Type => _room.Type;

        public RoomViewModel(Room room)
        {
            _room = room;
        }

        public override string ToString()
        {
            return _room.ToString();
        }
    }
}
