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
using ZdravoCorp.ViewModels.Filters;
using ZdravoCorp.Views;

namespace ZdravoCorp.ViewModels.Manager
{
    internal class ManagerViewModel : ViewModelBase
    {
        private ManagerNavigationViewModel _navigation;

        private readonly string[] _roomTypeItems = new string[]
        {
            "All",
            RoomType.ExamRoom.ToString(),
            RoomType.WaitingRoom.ToString(),
            RoomType.Ward.ToString(),
            RoomType.OperatingTheatre.ToString()
        };
        public string[] RoomTypeItems
        {
            get => _roomTypeItems;
        }
        private string _selectedRoomType;
        public string SelectedRoomType
        {
            get => _selectedRoomType;
            set
            {
                _selectedRoomType = value;
                OnPropertyChanged(nameof(SelectedRoomType));
                _filter.RoomType = value;
                _equipmentCollection.Refresh();
            }
        }

        private readonly string[] _equipmentTypeItems = new string[]
        {
            "All",
            EquipmentType.ExamEquipment.ToString(),
            EquipmentType.HallwayEquipment.ToString(),
            EquipmentType.Surgical.ToString(),
            EquipmentType.Furniture.ToString(),
            EquipmentType.Dynamic.ToString()
        };
        public string[] EquipmentTypeItems
        {
            get => _equipmentTypeItems;
        }
        private string _selectedEquipmentType;
        public string SelectedEquipmentType
        {
            get => _selectedEquipmentType;
            set
            {
                _selectedEquipmentType = value;
                OnPropertyChanged(nameof(SelectedEquipmentType));
                _filter.EquipmentType = value;
                _equipmentCollection.Refresh();
            }
        }

        private readonly string[] _quantityRangeItems = new string[]
        {
            "All",
            "Out of stock",
            "1-10",
            "10+"
        };
        public string[] QuantityRangeItems
        {
            get => _quantityRangeItems;
        }
        private string _selectedQuantityRange;
        public string SelectedQuantityRange
        {
            get => _selectedQuantityRange;
            set
            {
                _selectedQuantityRange = value;
                OnPropertyChanged(nameof(SelectedQuantityRange));
                _filter.QuantityRange = value;
                _equipmentCollection.Refresh();
            }
        }

        private bool _isCheckedStorage;
        public bool IsCheckedStorage
        {
            get => _isCheckedStorage;
            set
            {
                _isCheckedStorage = value;
                OnPropertyChanged(nameof(IsCheckedStorage));
                _filter.IsStorageChecked = value;
                _equipmentCollection.Refresh();
            }
        }

        private string _query;
        public string Query
        {
            get => _query;
            set
            {
                _query = value;
                OnPropertyChanged(nameof(Query));
                _filter.Query = value;
                _equipmentCollection.Refresh();
            }
        }

        private ObservableCollection<EquipmentViewModel> _equipmentItems;
        public ObservableCollection<EquipmentViewModel> EquipmentItems
        {
            get => _equipmentItems;
            set
            {
                _equipmentItems = value;
                OnPropertyChanged(nameof(EquipmentItems));
            }
        }

        private EquipmentViewModel _selectedEquipment;
        public EquipmentViewModel SelectedEquipment
        {
            get
            {
                return _selectedEquipment;
            }
            set
            {
                _selectedEquipment = value;
                OnPropertyChanged(nameof(SelectedEquipment));
            }
        }

        private ICollectionView _equipmentCollection;
        public ICollectionView EquipmentCollection
        {
            get => _equipmentCollection;
            set
            {
                _equipmentCollection = value;
                OnPropertyChanged(nameof(EquipmentCollection));
            }
        }

        private EquipmentDAO _equipmentDAO;
        private EquipmentFilter _filter;

        public ICommand PlaceOrderNavigationCommand => new RelayCommand(OrderNavigateCommand);

        private void OrderNavigateCommand(object obj)
        {
            _navigation.CurrentViewModel = new EquipmentOrderViewModel(_navigation);
        }

        public ICommand EquipmentTransferCommand => new RelayCommand(TransferNavigateCommand, (object obj) => _selectedEquipment != null && _selectedEquipment.RoomsAndQuantity.Count() > 0);
        
        private void TransferNavigateCommand(object obj)
        {
            _navigation.CurrentViewModel = new EquipmentTransferViewModel(_navigation, _selectedEquipment);
        }

        public ICommand LogoutCommand => new RelayCommand<Window>(Logout);

        private void Logout(Window window)
        {
            LoginWindowView loginWindow = new LoginWindowView();
            loginWindow.Show();

            if (window != null)
            {
                window.Close();
            }
        }

        public ManagerViewModel(ManagerNavigationViewModel navigation)
        {
            _navigation = navigation;

            _equipmentDAO = new EquipmentDAO();
            _equipmentItems = new ObservableCollection<EquipmentViewModel>();
            _filter = new EquipmentFilter(_quantityRangeItems);

            SetupView();
            SetupFilter();
        }

        private void SetupFilter()
        {
            _filter.RoomType = _selectedRoomType;
            _filter.EquipmentType = _selectedEquipmentType;
            _filter.Query = _query;
            _filter.IsStorageChecked = _isCheckedStorage;
            _filter.QuantityRange = _selectedQuantityRange;
            _equipmentCollection.Filter = _filter.Filter;
        }

        private void SetupView()
        {
            // setup equipment view models
            foreach (var item in _equipmentDAO.GetAllEquipment())
            {
                _equipmentItems.Add(new EquipmentViewModel(item));
            }

            // setup initial combobox values
            _selectedRoomType = _roomTypeItems[0];
            _selectedEquipmentType = _equipmentTypeItems[0];
            _selectedQuantityRange = _quantityRangeItems[0];

            // setup view for filtering
            _equipmentCollection = CollectionViewSource.GetDefaultView(_equipmentItems);
        }
    }
}
