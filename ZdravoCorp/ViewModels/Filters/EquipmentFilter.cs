using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Observers;

namespace ZdravoCorp.ViewModels.Filters
{
    class EquipmentFilter
    {
        public string? RoomType { get; set; }
        public string? EquipmentType { get; set; }
        public string? QuantityRange { get; set; }
        public bool IsStorageChecked { get; set; }
        public string? Query { get; set; }

        private readonly List<Predicate<EquipmentViewModel>> _filters;

        private readonly string[] _quantityRangeItems;

        public EquipmentFilter(string[] quantityRangeItems)
        {
            _quantityRangeItems = quantityRangeItems;
            _filters = new List<Predicate<EquipmentViewModel>>
            {
                FilterByRoomType,
                FilterByEquipmentType,
                FilterByQuantityRange,
                FilterByStorage,
                FilterByQuery
            };
        }

        public bool Filter(object obj)
        {
            if (obj is not EquipmentViewModel item) return false;
            return _filters.Aggregate(true, (previous, predicate) => previous && predicate(item));
        }

        private bool FilterByRoomType(object obj)
        {
            if (obj is not EquipmentViewModel item) return false;
            if (RoomType == "All") return true;

            return null != item.Rooms.FirstOrDefault(r => r.Type.ToString() == RoomType);
        }

        private bool FilterByEquipmentType(object obj)
        {
            if (obj is not EquipmentViewModel item) return false;
            if (EquipmentType == "All") return true;

            return item.Type.ToString() == EquipmentType;
        }

        private bool FilterByQuantityRange(object obj)
        {
            if (obj is not EquipmentViewModel item) return false;
            if (QuantityRange == "All") return true;

            if (QuantityRange == _quantityRangeItems[1])
                return item.TotalQuantity == 0;
            else if (QuantityRange == _quantityRangeItems[2])
                return item.TotalQuantity >= 1 && item.TotalQuantity <= 10;
            else
                return item.TotalQuantity >= 10;
        }

        private bool FilterByStorage(object obj)
        {
            if (obj is not EquipmentViewModel item) return false;
            if (!IsStorageChecked) return true;

            return item.NotInStorage;
        }

        private bool FilterByQuery(object obj)
        {
            if (obj is not EquipmentViewModel item) return false;
            if (string.IsNullOrEmpty(Query)) return true;

            return item.Rooms.Any(r => r.Type.ToString().Contains(Query)) ||
                item.Type.ToString().Contains(Query) ||
                item.Name.Contains(Query) ||
                item.TotalQuantity.ToString().Contains(Query);
        }
    }
}
