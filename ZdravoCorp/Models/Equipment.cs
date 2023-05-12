using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models.DAO;

namespace ZdravoCorp.Models
{
    public class Equipment
    {
        public string Name { get; set; }
        public EquipmentType Type { get; set; }

        public Equipment(string name, EquipmentType type)
        {
            Name = name;
            Type = type;
        }
    }

    public enum EquipmentType
    {
        Surgical,
        ExamEquipment,
        Furniture,
        HallwayEquipment,
        Dynamic
    }
}
