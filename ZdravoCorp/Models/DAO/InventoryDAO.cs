using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Storage;

namespace ZdravoCorp.Models.DAO
{
    public class InventoryDAO
    {
        private readonly List<InventoryItem> _inventory;

        public InventoryDAO()
        {
            _inventory = new List<InventoryItem>(InventoryStorage.Load());
        }

        public void CreateItem(InventoryItem item)
        {
            _inventory.Add(item);
            InventoryStorage.Save(_inventory);
        }

        public IEnumerable<InventoryItem> GetAll() 
        {
            return _inventory;
        }

        public InventoryItem? GetItem(Equipment equipment, Room room) 
        { 
            return _inventory.FirstOrDefault(item => item.Equipment.Name == equipment.Name && item.Room.Id == room.Id);
        }

        public void UpdateItem(InventoryItem item)
        {
            var target = _inventory.FirstOrDefault(i => i == item);
            if (target == null) return;
            
            target = item;
            InventoryStorage.Save(_inventory);
        }

        public void DeleteItem(InventoryItem item)
        {
            _inventory.Remove(item);
            InventoryStorage.Save(_inventory);
        }
    }
}
