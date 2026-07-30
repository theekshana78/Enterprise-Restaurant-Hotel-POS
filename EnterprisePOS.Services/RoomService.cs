using System;
using System.Collections.Generic;
using System.Linq;
using EnterprisePOS.Core;
using EnterprisePOS.Core.Entities;
using EnterprisePOS.Data;

namespace EnterprisePOS.Services
{
    public class RoomService
    {
        private readonly POSDbContext _context;

        public RoomService(POSDbContext context)
        {
            _context = context;
        }

        public List<Room> GetAllRooms()
        {
            return _context.Rooms.ToList();
        }

        public bool CheckInGuest(int roomId, string guestName, string guestPhone)
        {
            var room = _context.Rooms.Find(roomId);
            if (room == null || room.Status != RoomStatus.Available)
                return false;

            room.Status = RoomStatus.Occupied;
            room.CurrentGuestName = guestName;
            room.CurrentGuestPhone = guestPhone;
            room.CheckInTime = DateTime.Now;
            room.PendingAccruedBill = 0;

            _context.SaveChanges();
            return true;
        }

        public decimal CalculateFinalRoomBill(int roomId, out int nightsSpent)
        {
            var room = _context.Rooms.Find(roomId);
            if (room == null)
            {
                nightsSpent = 0;
                return 0;
            }

            TimeSpan stayedTime = DateTime.Now - (room.CheckInTime ?? DateTime.Now);
            nightsSpent = Math.Max(1, (int)Math.Ceiling(stayedTime.TotalDays));

            decimal roomChargeTotal = nightsSpent * room.RatePerNight;
            decimal combinedTotal = roomChargeTotal + room.PendingAccruedBill;

            return combinedTotal;
        }

        public bool CheckOutGuest(int roomId, PaymentMethod paymentMethod, string cashierName, out Invoice? roomInvoice)
        {
            var room = _context.Rooms.Find(roomId);
            roomInvoice = null;

            if (room == null || room.Status != RoomStatus.Occupied)
                return false;

            decimal grandTotal = CalculateFinalRoomBill(roomId, out int nights);

            string invoiceNo = $"ROOM-INV-{DateTime.Now:yyyyMMdd}-{_context.Invoices.Count() + 1:D4}";

            roomInvoice = new Invoice
            {
                InvoiceNo = invoiceNo,
                InvoiceDate = DateTime.Now,
                SubTotal = grandTotal,
                Discount = 0,
                Tax = 0,
                GrandTotal = grandTotal,
                PaymentMethod = paymentMethod,
                CashierName = cashierName,
                RoomId = roomId,
                CustomerName = room.CurrentGuestName,
                Items = new List<InvoiceItem>
                {
                    new InvoiceItem
                    {
                        ProductName = $"{room.RoomNumber} Stay ({nights} Nights @ LKR {room.RatePerNight:N0})",
                        UnitPrice = room.RatePerNight * nights,
                        Quantity = 1
                    }
                }
            };

            if (room.PendingAccruedBill > 0)
            {
                roomInvoice.Items.Add(new InvoiceItem
                {
                    ProductName = "Food & Beverage Room Tab Charges",
                    UnitPrice = room.PendingAccruedBill,
                    Quantity = 1
                });
            }

            // Mark all unbilled minibar items as billed
            var miniBarItems = _context.MiniBarItems.Where(m => m.RoomId == roomId && !m.IsBilled).ToList();
            foreach (var mb in miniBarItems)
            {
                mb.IsBilled = true;
                roomInvoice.Items.Add(new InvoiceItem
                {
                    ProductName = $"[MiniBar] {mb.ProductName}",
                    UnitPrice = mb.UnitPrice,
                    Quantity = mb.Quantity
                });
            }

            _context.Invoices.Add(roomInvoice);

            // Reset Room Status
            room.Status = RoomStatus.Cleaning;
            room.CurrentGuestName = null;
            room.CurrentGuestPhone = null;
            room.CheckInTime = null;
            room.PendingAccruedBill = 0;

            _context.SaveChanges();
            return true;
        }

        public void SetRoomStatus(int roomId, RoomStatus status)
        {
            var room = _context.Rooms.Find(roomId);
            if (room != null)
            {
                room.Status = status;
                _context.SaveChanges();
            }
        }
    }
}
