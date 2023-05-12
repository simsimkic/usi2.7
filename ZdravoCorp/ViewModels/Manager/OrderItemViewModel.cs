using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models;

namespace ZdravoCorp.ViewModels.Manager
{
    internal class OrderItemViewModel : ViewModelBase
    {
        private OrderItem _orderItem;
        public string Name
        {
            get => _orderItem.Name;
            set
            {
                _orderItem.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public int Quantity
        {
            get => _orderItem.Quantity;
            set
            {
                _orderItem.Quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public OrderItemViewModel(OrderItem orderItem)
        {
            _orderItem = orderItem;
        }
    }
}
