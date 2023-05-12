using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ZdravoCorp.Models;
using ZdravoCorp.Models.DAO;

namespace ZdravoCorp.Services
{
    public class OrderService
    {
        private readonly OrderDAO _orderDAO = new();
        private readonly EquipmentService _equipmentService = new();
        private readonly InventoryService _inventoryService = new();
        private readonly RoomService _roomService = new();

        public IEnumerable<Order> GetOrders()
        {
            return _orderDAO.GetOrders();
        }

        public void CreateOrder(Order order)
        {
            _orderDAO.CreateOrder(order);
        }

        public Order? GetOrderById(Guid id)
        {
            return _orderDAO.GetOrderById(id);
        }

        public void UpdateOrder(Order order)
        {
            _orderDAO.UpdateOrder(order);
        }

        public void CheckOrders()
        {
            foreach (var order in _orderDAO.GetOrders())
            {
                if (!order.IsCompleted && DateTime.Now >= order.ArrivalDate) ExecuteOrder(order);
            }
        }

        public void ExecuteOrder(Order order)
        {
            var storageRoom = _roomService.GetStorageRoom();
            if (storageRoom == null) return;

            foreach (var item in order.Items) 
            {
                var equipment = _equipmentService.GetEquipmentByName(item.Name);
                if (equipment == null) continue;

                _inventoryService.ReplenishEquipment(equipment, storageRoom, item.Quantity);
            }

            order.IsCompleted = true;
            UpdateOrder(order);
        }
    }
}
