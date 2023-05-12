using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.Models;
using ZdravoCorp.Models.DAO;
using ZdravoCorp.Services;

namespace ZdravoCorp.ViewModels.Manager
{
    internal class EquipmentOrderViewModel : ViewModelBase
    {
        private ManagerNavigationViewModel _navigation;
        private readonly OrderService _orderService = new();
        private readonly InventoryService _inventoryService = new();

        private ObservableCollection<EquipmentViewModel> _dynamicEquipment;
        public ObservableCollection<EquipmentViewModel> DynamicEquipment
        {
            get => _dynamicEquipment;
            set
            {
                _dynamicEquipment = value;
                OnPropertyChanged(nameof(DynamicEquipment));
            }
        }

        private ObservableCollection<OrderItemViewModel> _orderItems;
        public ObservableCollection<OrderItemViewModel> OrderItems
        {
            get => _orderItems;
            set
            {
                _orderItems = value;
                OnPropertyChanged(nameof(OrderItems));
            }
        }

        private EquipmentViewModel? _selectedEquipment;
        public EquipmentViewModel? SelectedEquipment
        {
            get => _selectedEquipment;
            set
            {
                _selectedEquipment = value;
                OnPropertyChanged(nameof(SelectedEquipment));
            }
        }

        private OrderItemViewModel? _selectedOrderItem;
        public OrderItemViewModel? SelectedOrderItem
        {
            get => _selectedOrderItem;
            set
            {
                _selectedOrderItem = value;
                OnPropertyChanged(nameof(SelectedOrderItem));
            }
        }

        public ICommand AddToOrderCommand => new RelayCommand(AddToOrder, (parameter) => _selectedEquipment != null);
        public ICommand RemoveFromOrderCommand => new RelayCommand(RemoveFromOrder, (parameter) => _selectedOrderItem != null);
        public ICommand SubmitOrderCommand => new RelayCommand(SubmitOrder, (paramater) => _orderItems.Count > 0);

        public ICommand CancelOrderCommand => new RelayCommand(CancelOrder);


        public EquipmentOrderViewModel(ManagerNavigationViewModel navigation)
        {
            _navigation = navigation;

            _dynamicEquipment = new ObservableCollection<EquipmentViewModel>();
            _orderItems = new ObservableCollection<OrderItemViewModel>();

            foreach (var item in _inventoryService.GetLowStockDynamicEquipment(5))
            {
                _dynamicEquipment.Add(new EquipmentViewModel(item));
            }
        }

        private void AddToOrder(object obj)
        {
            if (_selectedEquipment == null) return;

            var item = new OrderItem(_selectedEquipment.Name, 10);

            // Check if we already added the item to the order
            if (_orderItems.FirstOrDefault(o => o.Name == item.Name) == null)
            {
                _orderItems.Add(new OrderItemViewModel(item));
            }
        }

        private void RemoveFromOrder(object obj)
        {
            if (_selectedOrderItem == null) return;

            _orderItems.Remove(_selectedOrderItem);
        }

        private void SubmitOrder(object obj)
        {
            var orderList = new List<OrderItem>();
            foreach (var item in _orderItems)
            {
                if (item.Quantity < 1)
                {
                    MessageBox.Show($"Unable to create order due to order item {item.Name} having a quantity of zero or below.",
                        "Order error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                orderList.Add(new OrderItem(item.Name, item.Quantity));
            }

            var order = new Order(orderList);
            _orderService.CreateOrder(order);

            MessageBox.Show($"Order with the ID: {order.Id} has been succesfully created.",
                "Order created", MessageBoxButton.OK, MessageBoxImage.Information);
            _navigation.CurrentViewModel = new ManagerViewModel(_navigation);
        }

        private void CancelOrder(object obj)
        {
            _navigation.CurrentViewModel = new ManagerViewModel(_navigation);
        }
    }
}
