using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models;
using ZdravoCorp.Services;

namespace ZdravoCorp.ViewModels
{
    internal class EquipmentViewModel : ViewModelBase
    {
        private readonly Equipment _equipment;
        private readonly InventoryService _inventoryService = new();
        public string Name => _equipment.Name;
        public EquipmentType Type => _equipment.Type;
        public int TotalQuantity => _inventoryService.GetEquipmentQuantity(_equipment);
        public bool NotInStorage => _inventoryService.GetEquipmentQuantityInStorage(_equipment) <= 0;
        public IEnumerable<RoomViewModel> Rooms => _inventoryService.GetRoomsByEquipment(_equipment).Select(r => new RoomViewModel(r));
        public IEnumerable<string> RoomsAndQuantity
        {
            get
            {
                var roomsAndQuantity = new List<string>();

                foreach (var room in Rooms)
                {
                    var quantity = _inventoryService.GetEquipmentQuantityInRoom(_equipment, new Room(room.Id, room.Type));
                    roomsAndQuantity.Add(room.ToString() + ", qty: " +  quantity.ToString());
                }

                return roomsAndQuantity;
            }
        }

        public EquipmentViewModel(Equipment equipment)
        {
            _equipment = equipment;
        }
    }
}
