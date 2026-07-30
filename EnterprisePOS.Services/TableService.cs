using System;
using System.Collections.Generic;
using System.Linq;
using EnterprisePOS.Core;
using EnterprisePOS.Core.Entities;
using EnterprisePOS.Data;

namespace EnterprisePOS.Services
{
    public class TableService
    {
        private readonly POSDbContext _context;

        public TableService(POSDbContext context)
        {
            _context = context;
        }

        public List<RestaurantTable> GetAllTables()
        {
            return _context.Tables.ToList();
        }

        public bool UpdateTableStatus(int tableId, TableStatus status, string? guestName = null, string? waiterName = null)
        {
            var table = _context.Tables.Find(tableId);
            if (table == null) return false;

            table.Status = status;
            if (guestName != null) table.CurrentGuestName = guestName;
            if (waiterName != null) table.AssignedWaiter = waiterName;

            if (status == TableStatus.Available)
            {
                table.CurrentGuestName = null;
                table.AssignedWaiter = null;
                table.CurrentInvoiceId = null;
            }

            _context.SaveChanges();
            return true;
        }

        public bool ReserveTable(int tableId, string guestName, DateTime reservationTime)
        {
            var table = _context.Tables.Find(tableId);
            if (table == null || table.Status != TableStatus.Available) return false;

            table.Status = TableStatus.Reserved;
            table.CurrentGuestName = guestName;
            table.ReservedTime = reservationTime;

            _context.SaveChanges();
            return true;
        }

        public bool MoveTable(int sourceTableId, int targetTableId)
        {
            var source = _context.Tables.Find(sourceTableId);
            var target = _context.Tables.Find(targetTableId);

            if (source == null || target == null) return false;
            if (source.Status != TableStatus.Occupied || target.Status != TableStatus.Available) return false;

            target.Status = TableStatus.Occupied;
            target.CurrentGuestName = source.CurrentGuestName;
            target.AssignedWaiter = source.AssignedWaiter;
            target.CurrentInvoiceId = source.CurrentInvoiceId;

            source.Status = TableStatus.Available;
            source.CurrentGuestName = null;
            source.AssignedWaiter = null;
            source.CurrentInvoiceId = null;

            _context.SaveChanges();
            return true;
        }

        public bool MergeTables(int sourceTableId, int targetTableId)
        {
            var source = _context.Tables.Find(sourceTableId);
            var target = _context.Tables.Find(targetTableId);

            if (source == null || target == null) return false;

            target.Status = TableStatus.Occupied;
            if (string.IsNullOrEmpty(target.CurrentGuestName))
                target.CurrentGuestName = source.CurrentGuestName;

            source.Status = TableStatus.Available;
            source.CurrentGuestName = null;

            _context.SaveChanges();
            return true;
        }
    }
}
