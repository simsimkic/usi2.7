using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models;
using ZdravoCorp.Services;

namespace ZdravoCorp.ViewModels.Manager
{
    internal class TransferRoomViewModel : RoomViewModel
    {
        private readonly Equipment _equipment;
        private readonly InventoryService _inventoryService = new();
        public bool IsLowSupply
        {
            get
            {
                var inventoryItem = _inventoryService.GetItem(_equipment, _room);
                return _equipment.Type == EquipmentType.Dynamic &&
                    (inventoryItem == null || inventoryItem.Quantity < 5);
            }
        }

        public TransferRoomViewModel(Room room, Equipment equipment) : base(room)
        {
            _equipment = equipment;
        }
    }
}
