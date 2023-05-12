using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Storage;
using ZdravoCorp.Models;
using System.Windows.Controls;

namespace ZdravoCorp.Models.DAO
{
    public class EquipmentDAO
    {
        private IEquipmentStorage _storage;
        private List<Equipment> _equipment;

        public EquipmentDAO()
        {
            _storage = new EquipmentJsonStorage();
            _equipment = _storage.Load().ToList();
        }

        public IEnumerable<Equipment> GetAllEquipment() => _equipment;

        public void UpdateEquipment(Equipment equipment)
        {
            var targetEquipment = _equipment.FirstOrDefault(e => e.Name == equipment.Name);
            if (targetEquipment != null) targetEquipment = equipment;
            _storage.Save(_equipment);
        }

        public Equipment? GetEquipmentByName(string name)
        {
            return _equipment.FirstOrDefault(e => e.Name == name);
        }
    }
}
