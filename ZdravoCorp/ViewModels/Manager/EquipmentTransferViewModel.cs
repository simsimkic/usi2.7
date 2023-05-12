using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZdravoCorp.Models;
using ZdravoCorp.Models.DAO;
using ZdravoCorp.Services;

namespace ZdravoCorp.ViewModels.Manager
{
    internal class EquipmentTransferViewModel : ViewModelBase
    {
        private ManagerNavigationViewModel _navigation;

        private readonly RoomService _roomService = new();
        private readonly TransferService _transferService = new();
        private readonly EquipmentService _equipmentService = new();
        private readonly InventoryService _inventoryService = new();

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

        private ObservableCollection<RoomViewModel> _fromRooms;
        public ObservableCollection<RoomViewModel> FromRooms
        {
            get
            {
                return _fromRooms;
            }
            set
            {
                _fromRooms = value;
                OnPropertyChanged(nameof(FromRooms));
            }
        }

        private RoomViewModel _selectedFromRoom;
        public RoomViewModel SelectedFromRoom
        {
            get
            {
                return _selectedFromRoom;
            }
            set
            {
                _selectedFromRoom = value;
                OnPropertyChanged(nameof(SelectedFromRoom));
            }
        }

        private ObservableCollection<TransferRoomViewModel> _toRooms;
        public ObservableCollection<TransferRoomViewModel> ToRooms
        {
            get
            {
                return _toRooms;
            }
            set
            {
                _toRooms = value;
                OnPropertyChanged(nameof(ToRooms));
            }
        }

        private TransferRoomViewModel _selectedToRoom;
        public TransferRoomViewModel SelectedToRoom
        {
            get
            {
                return _selectedToRoom;
            }
            set
            {
                _selectedToRoom = value;
                OnPropertyChanged(nameof(SelectedToRoom));
            }
        }

        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        private int _selectedQuantity;
        public int SelectedQuantity
        {
            get
            {
                return _selectedQuantity;
            }
            set
            {
                _selectedQuantity = value;
                OnPropertyChanged(nameof(SelectedQuantity));
            }
        }
        public bool IsDatePickerEnabled => _selectedEquipment.Type != EquipmentType.Dynamic;

        public ICommand SubmitCommand => new RelayCommand(SubmitTransfer);

        private void SubmitTransfer(object obj)
        {
            if (SelectedFromRoom.Id == SelectedToRoom.Id)
            {
                MessageBox.Show("Unable to transfer to same room.");
                return;
            }
            else if (SelectedQuantity < 1)
            {
                MessageBox.Show("Quantity must be 1 or above.");
                return;
            }

            var quantity = _inventoryService.GetEquipmentQuantityInRoom(
                new Equipment(SelectedEquipment.Name, SelectedEquipment.Type),
                new Room(SelectedFromRoom.Id, SelectedFromRoom.Type));

            if (quantity < SelectedQuantity)
            {
                MessageBox.Show("Insufficient quantity in selected source room.");
                return;
            }

            var transfer = new Transfer(
                Guid.NewGuid(),
                new Room(SelectedFromRoom.Id, SelectedFromRoom.Type),
                new Room(SelectedToRoom.Id, SelectedToRoom.Type),
                new TransferItem(SelectedEquipment.Name, SelectedQuantity),
                SelectedDate);

            _transferService.CreateTransfer(transfer);

            if (SelectedEquipment.Type == EquipmentType.Dynamic) _transferService.ExecuteTransfer(transfer);

            MessageBox.Show($"Transfer with the ID: {transfer.Id} has been succesfully created.",
                "Transfer created", MessageBoxButton.OK, MessageBoxImage.Information);
            _navigation.CurrentViewModel = new ManagerViewModel(_navigation);
        }
       
        public ICommand CancelCommand => new RelayCommand(CancelTransfer);

        private void CancelTransfer(object obj)
        {
            _navigation.CurrentViewModel = new ManagerViewModel(_navigation);
        }

        public EquipmentTransferViewModel(ManagerNavigationViewModel navigation, EquipmentViewModel equipment)
        {
            _navigation = navigation;
            _selectedEquipment = equipment;

            FromRooms = new ObservableCollection<RoomViewModel>(equipment.Rooms);
            ToRooms = new ObservableCollection<TransferRoomViewModel>();
            foreach (var room in _roomService.GetRooms())
            {
                ToRooms.Add(new TransferRoomViewModel(room, _equipmentService.GetEquipmentByName(_selectedEquipment.Name)));
            }

            SelectedFromRoom = FromRooms[0];
            SelectedToRoom = ToRooms[0];
            SelectedDate = DateTime.Now;
        }
    }
}
