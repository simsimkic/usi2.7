using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Models;
using ZdravoCorp.Models.DAO;

namespace ZdravoCorp.Services
{
    internal class TransferService
    {
        private readonly TransferDAO _transferDAO = new();
        private readonly InventoryService _inventoryService = new();
        private readonly EquipmentService _equipmentService = new();

        public void CreateTransfer(Transfer transfer)
        {
            _transferDAO.CreateTransfer(transfer);
        }

        public IEnumerable<Transfer> GetTransfers() 
        {
            return _transferDAO.GetTransfers();
        }

        public void CheckTransfers()
        {
            foreach (var transfer in _transferDAO.GetTransfers())
            {
                if (!transfer.IsCompleted && DateTime.Now >= transfer.ExecutionDate) ExecuteTransfer(transfer);
            }
        }

        public void UpdateTransfer(Transfer transfer)
        {
            _transferDAO.UpdateTransfer(transfer);
        }

        public void ExecuteTransfer(Transfer transfer)
        {
            var equipment = _equipmentService.GetEquipmentByName(transfer.Item.Name);
            if (equipment == null) return;

            _inventoryService.DepleteEquipment(equipment, transfer.FromRoom, transfer.Item.Quantity);
            _inventoryService.ReplenishEquipment(equipment, transfer.ToRoom, transfer.Item.Quantity);
            transfer.IsCompleted = true;
            UpdateTransfer(transfer);
        }
    }
}
