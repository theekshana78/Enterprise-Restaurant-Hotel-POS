using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using EnterprisePOS.Core;
using EnterprisePOS.Core.Entities;
using EnterprisePOS.Data;

namespace EnterprisePOS.Services
{
    public class KOTService
    {
        private readonly POSDbContext _context;

        public KOTService(POSDbContext context)
        {
            _context = context;
        }

        public KOTOrder? GenerateKOT(string tableOrRoom, List<InvoiceItem> items)
        {
            var kitchenItems = items.Where(i =>
            {
                var p = _context.Products.Find(i.ProductId);
                return p != null && p.IsKitchenItem;
            }).ToList();

            if (!kitchenItems.Any())
                return null;

            var detailsList = kitchenItems.Select(i => new
            {
                ItemName = i.ProductName,
                Qty = i.Quantity
            }).ToList();

            string ticketNo = $"KOT-{DateTime.Now:HHmmss}-{_context.KOTOrders.Count() + 1}";

            var kot = new KOTOrder
            {
                TicketNumber = ticketNo,
                TargetTableOrRoom = tableOrRoom,
                OrderDetailsJson = JsonSerializer.Serialize(detailsList),
                Status = OrderStatus.InKitchen,
                CreatedAt = DateTime.Now
            };

            _context.KOTOrders.Add(kot);
            _context.SaveChanges();
            return kot;
        }

        public List<KOTOrder> GetActiveKOTOrders()
        {
            return _context.KOTOrders.Where(k => k.Status == OrderStatus.InKitchen || k.Status == OrderStatus.Pending).ToList();
        }

        public void UpdateKOTStatus(int kotId, OrderStatus status)
        {
            var kot = _context.KOTOrders.Find(kotId);
            if (kot != null)
            {
                kot.Status = status;
                _context.SaveChanges();
            }
        }
    }
}
