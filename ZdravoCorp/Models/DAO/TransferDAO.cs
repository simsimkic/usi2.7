using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Storage;

namespace ZdravoCorp.Models.DAO
{
    internal class TransferDAO
    {
        private readonly List<Transfer> _transfers;

        public TransferDAO()
        {
            _transfers = TransferStorage.Load().ToList();
        }

        public void CreateTransfer(Transfer transfer)
        {
            _transfers.Add(transfer);
            TransferStorage.Save(_transfers);
        }

        public IEnumerable<Transfer> GetTransfers() => _transfers;

        public void UpdateTransfer(Transfer transfer)
        {
            var target = _transfers.FirstOrDefault(t => t.Id ==  transfer.Id);
            if (target == null) return;

            target = transfer;
            TransferStorage.Save(_transfers);
        }
    }
}
