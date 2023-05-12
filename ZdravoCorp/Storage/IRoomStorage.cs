using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models;

namespace ZdravoCorp.Storage
{
    internal interface IRoomStorage
    {
        IEnumerable<Room> Load();
        void Save(IEnumerable<Room> rooms);
    }
}
