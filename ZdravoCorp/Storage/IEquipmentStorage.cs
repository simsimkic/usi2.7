using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models;

namespace ZdravoCorp.Storage
{
    internal interface IEquipmentStorage
    {
        IEnumerable<Equipment> Load();
        void Save(IEnumerable<Equipment> equipment);
    }
}
