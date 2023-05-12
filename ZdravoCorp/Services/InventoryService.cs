using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.Primitives;
using ZdravoCorp.Models;
using ZdravoCorp.Models.DAO;

namespace ZdravoCorp.Services
{
    public class InventoryService
    {
        private readonly EquipmentService _equipmentService = new();
        private readonly InventoryDAO _inventoryDAO = new();

        public void CreateItem(InventoryItem item)
        {
            _inventoryDAO.CreateItem(item);
        }

        public IEnumerable<InventoryItem> GetAll()
        {
            return _inventoryDAO.GetAll();
        }

        public InventoryItem? GetItem(Equipment equipment, Room room) 
        {
            return _inventoryDAO.GetItem(equipment, room);
        }

        public void UpdateItem(InventoryItem item)
        {
            _inventoryDAO.UpdateItem(item);
        }

        public void DeleteItem(InventoryItem item)
        {
            _inventoryDAO.DeleteItem(item);
        }

        public IEnumerable<InventoryItem> GetItemsByEquipment(Equipment equipment)
        {
            return GetAll().Where(item => item.Equipment.Name == equipment.Name);
        }

        public int GetEquipmentQuantity(Equipment equipment)
        {
            return GetItemsByEquipment(equipment).Sum(item => item.Quantity);
        }

        public int? GetEquipmentQuantityInRoom(Equipment equipment, Room room)
        {
            return GetItem(equipment, room)?.Quantity;
        }

        public int GetEquipmentQuantityInStorage(Equipment equipment)
        {
            return GetItemsByEquipment(equipment).Where(item => item.Room.Type == RoomType.Storage).Sum(item => item.Quantity);
        }

        public IEnumerable<Room> GetRoomsByEquipment(Equipment equipment)
        {
            return GetItemsByEquipment(equipment).Select(item => item.Room);
        }

        public void ReplenishEquipment(Equipment equipment, Room room, int quantity)
        {
            var item = GetItem(equipment, room);

            if (item != null)
            {
                item.Quantity += quantity;
                UpdateItem(item);
            }
            else
            {
                CreateItem(new InventoryItem(equipment, room, quantity));
            }
        }

        public void DepleteEquipment(Equipment equipment, Room room, int quantity)
        {
            var item = GetItem(equipment, room);

            if (item != null)
            {
                item.Quantity -= quantity;

                if (item.Quantity <= 0)
                {
                    DeleteItem(item);
                }
                else
                {
                    UpdateItem(item);
                }
            }
            else
            {
                CreateItem(new InventoryItem(equipment, room, quantity));
            }
        }

        public IEnumerable<Equipment> GetEquipmentByRoom(Room room)
        {
            return GetAll().Where(item => item.Room.Id == room.Id).Select(item => item.Equipment);
        }

        /// <summary>
        /// Gets dynamic equipment that is low on stock.
        /// </summary>
        /// <param name="stockCeiling"></param>
        /// <returns>Low stock dynamic equipment</returns>
        public IEnumerable<Equipment> GetLowStockDynamicEquipment(int stockCeiling)
        {
            var dynamicEquipment = _equipmentService.GetDynamicEquipment();
            var lowStockList = new List<Equipment>();

            foreach (var item in dynamicEquipment)
            {
                var quantity = GetEquipmentQuantity(item);
                if (quantity < stockCeiling) lowStockList.Add(item);
            }

            return lowStockList;
        }
    }
}
