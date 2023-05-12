using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models;
using ZdravoCorp.Models.DAO;

namespace ZdravoCorp.Services
{
    internal class EquipmentService
    {
        private readonly EquipmentDAO _equipmentDAO = new();

        public IEnumerable<Equipment> GetAllEquipment()
        {
            return _equipmentDAO.GetAllEquipment();
        }

        public void UpdateEquipment(Equipment equipment)
        {
            _equipmentDAO.UpdateEquipment(equipment);
        }

        public Equipment? GetEquipmentByName(string name)
        {
            return _equipmentDAO.GetEquipmentByName(name);
        }
        public IEnumerable<Equipment> GetDynamicEquipment()
        {
            return _equipmentDAO.GetAllEquipment().Where(e => e.Type == EquipmentType.Dynamic);
        }
    }
}
