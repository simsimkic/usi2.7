using System;
using System.Collections;
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
using ZdravoCorp.Services;

namespace ZdravoCorp.ViewModels
{
    internal class DoctorSpentEquipmentViewModel : DoctorViewModel
    {
        private EquipmentService _equipmentService = new();
        private InventoryService _inventoryService = new();

        private Room _room;

        private ObservableCollection<SpentEquipmentViewModel> _equipmentList;
        public ObservableCollection<SpentEquipmentViewModel> EquipmentList
        {
            get
            {
                return _equipmentList;
            }
            set
            {
                _equipmentList = value;
                OnPropertyChanged(nameof(EquipmentList));
            }
        }

        private SpentEquipmentViewModel _selectedEquipment;
        public SpentEquipmentViewModel SelectedEquipment
        {
            get => _selectedEquipment;
            set
            {
                if (_selectedEquipment == value) return;
                _selectedEquipment = value;
                OnPropertyChanged(nameof(SelectedEquipment));
            }
        }

        public ICommand IncrementCommand => new RelayCommand(Increment);
        private void Increment(object parameter)
        {
            if (_selectedEquipment.Spent >= _selectedEquipment.Available) return;
            _selectedEquipment.Spent++;
        }
        public ICommand DecrementCommand => new RelayCommand(Decrement);
        private void Decrement(object parameter)
        {
            if (_selectedEquipment.Spent <= 0) return;
            _selectedEquipment.Spent--;
        }
        public ICommand FinishCommand => new RelayCommand(Finish);

        private void Finish(object parameter)
        {
            foreach (var item in EquipmentList)
            {
                var equipment = _equipmentService.GetEquipmentByName(item.Name);
                if (equipment == null) continue;
                _inventoryService.DepleteEquipment(equipment, _room, item.Spent);
            }

            MessageBox.Show("Equipment updated", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
            Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive).Close();
        }
        public DoctorSpentEquipmentViewModel()
        {
            // TODO: Pass room to constructor
            var roomService = new RoomService();
            _room = roomService.GetStorageRoom();

            _equipmentList = new ObservableCollection<SpentEquipmentViewModel>();

            var dynamicEquipment = _inventoryService.GetEquipmentByRoom(_room)
                .Where(e => e.Type == EquipmentType.Dynamic);

            foreach (var eq in dynamicEquipment)
            {
                var available = _inventoryService.GetEquipmentQuantityInRoom(eq, _room);
                if (available == null) continue;
                _equipmentList.Add(new SpentEquipmentViewModel(eq.Name, 0, (int)available));
            }
        }
    }
}
