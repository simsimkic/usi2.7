using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Storage;

namespace ZdravoCorp.Models.DAO
{
    public class OrderDAO
    {
        private readonly List<Order> _orders;

        public OrderDAO()
        {
            _orders = OrderStorage.Load().ToList();
        }

        public IEnumerable<Order> GetOrders() => _orders;

        public void CreateOrder(Order order)
        {
            _orders.Add(order);
            OrderStorage.Save(_orders);
        }

        public Order? GetOrderById(Guid id) => _orders.FirstOrDefault(o => o.Id == id);

        public void UpdateOrder(Order order)
        {
            var targetOrder = _orders.FirstOrDefault(o => o.Id == order.Id);
            if (targetOrder != null) targetOrder = order;
            OrderStorage.Save(_orders);
        }
    }
}
