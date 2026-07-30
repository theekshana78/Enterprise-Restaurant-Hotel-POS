using System;
using System.Collections.Generic;
using System.Linq;
using EnterprisePOS.Core.Entities;
using EnterprisePOS.Data;

namespace EnterprisePOS.Services
{
    public class MiniBarService
    {
        private readonly POSDbContext _context;

        public MiniBarService(POSDbContext context)
        {
            _context = context;
        }

        public List<MiniBarItem> GetUnbilledMiniBarItems(int roomId)
        {
            return _context.MiniBarItems
                .Where(m => m.RoomId == roomId && !m.IsBilled)
                .ToList();
        }

        public MiniBarItem AddMiniBarConsumption(int roomId, int productId, double qty, string guestName)
        {
            var room = _context.Rooms.Find(roomId);
            var product = _context.Products.Find(productId);

            if (room == null || product == null)
                throw new InvalidOperationException("Invalid room or product selection");

            decimal totalCharge = product.SellingPrice * (decimal)qty;

            var item = new MiniBarItem
            {
                RoomId = roomId,
                ProductId = productId,
                ProductName = product.Name,
                UnitPrice = product.SellingPrice,
                Quantity = qty,
                ConsumedAt = DateTime.Now,
                IsBilled = false,
                ConsumedByGuest = guestName
            };

            // Deduct inventory stock
            product.CurrentStock -= qty;

            // Add charge to room accrued bill tab
            room.PendingAccruedBill += totalCharge;

            _context.MiniBarItems.Add(item);
            _context.SaveChanges();
            return item;
        }
    }
}
