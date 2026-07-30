using System;
using System.Collections.Generic;
using System.Linq;
using EnterprisePOS.Core.Entities;
using EnterprisePOS.Data;

namespace EnterprisePOS.Services
{
    public class ShiftService
    {
        private readonly POSDbContext _context;

        public ShiftService(POSDbContext context)
        {
            _context = context;
        }

        public Shift? GetActiveShift(string username)
        {
            return _context.Shifts.FirstOrDefault(s => s.CashierUsername == username && !s.IsClosed);
        }

        public Shift StartShift(string username, string terminalName, decimal openingCash)
        {
            var existingShift = GetActiveShift(username);
            if (existingShift != null)
                return existingShift;

            string shiftNo = $"SH-{DateTime.Now:yyyyMMdd}-{_context.Shifts.Count() + 1:D3}";

            var shift = new Shift
            {
                ShiftNo = shiftNo,
                CashierUsername = username,
                TerminalName = terminalName,
                OpeningCash = openingCash,
                StartTime = DateTime.Now,
                IsClosed = false
            };

            _context.Shifts.Add(shift);
            _context.SaveChanges();
            return shift;
        }

        public bool CloseShift(int shiftId, decimal closingCash, string? notes, out decimal cashDifference)
        {
            cashDifference = 0;
            var shift = _context.Shifts.Find(shiftId);
            if (shift == null || shift.IsClosed)
                return false;

            // Calculate expected cash: Opening Cash + Cash Invoices created during this shift
            decimal cashSales = _context.Invoices
                .Where(i => i.ShiftId == shiftId && i.PaymentMethod == Core.PaymentMethod.Cash && i.OrderStatus == "Completed")
                .Sum(i => i.GrandTotal);

            decimal expectedCash = shift.OpeningCash + cashSales;
            cashDifference = closingCash - expectedCash;

            shift.ClosingCash = closingCash;
            shift.ExpectedCash = expectedCash;
            shift.CashDifference = cashDifference;
            shift.EndTime = DateTime.Now;
            shift.IsClosed = true;
            shift.Notes = notes;

            _context.SaveChanges();
            return true;
        }

        public decimal GetCurrentDrawerBalance(int shiftId)
        {
            var shift = _context.Shifts.Find(shiftId);
            if (shift == null) return 0;

            decimal cashSales = _context.Invoices
                .Where(i => i.ShiftId == shiftId && i.PaymentMethod == Core.PaymentMethod.Cash && i.OrderStatus == "Completed")
                .Sum(i => i.GrandTotal);

            return shift.OpeningCash + cashSales;
        }
    }
}
