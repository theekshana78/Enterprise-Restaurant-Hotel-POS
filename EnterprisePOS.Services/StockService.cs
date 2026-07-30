using System;
using System.Collections.Generic;
using System.Linq;
using EnterprisePOS.Core;
using EnterprisePOS.Core.Entities;
using EnterprisePOS.Data;

namespace EnterprisePOS.Services
{
    public class StockService
    {
        private readonly POSDbContext _context;

        public StockService(POSDbContext context)
        {
            _context = context;
        }

        public List<Product> GetLowStockAlerts()
        {
            return _context.Products.Where(p => p.CurrentStock <= p.MinStockLevel).ToList();
        }

        public List<Product> GetAllStockItems()
        {
            return _context.Products.ToList();
        }

        public void ConsumeStockItem(int productId, double quantityUsed, string destinationRoomOrDept, string handledBy)
        {
            var product = _context.Products.Find(productId);
            if (product == null)
                throw new InvalidOperationException("Product not found");

            product.CurrentStock -= quantityUsed;

            _context.StockTransactions.Add(new StockTransaction
            {
                ProductId = product.Id,
                ProductName = product.Name,
                QuantityChange = -quantityUsed,
                Reason = $"Usage: {destinationRoomOrDept}",
                TransactionDate = DateTime.Now,
                HandledBy = handledBy
            });

            _context.SaveChanges();
        }

        public void AddStock(int productId, double quantityAdded, decimal costPrice, string handledBy)
        {
            var product = _context.Products.Find(productId);
            if (product == null)
                throw new InvalidOperationException("Product not found");

            product.CurrentStock += quantityAdded;
            if (costPrice > 0)
                product.CostPrice = costPrice;

            _context.StockTransactions.Add(new StockTransaction
            {
                ProductId = product.Id,
                ProductName = product.Name,
                QuantityChange = quantityAdded,
                Reason = "Stock Purchase / Refill",
                TransactionDate = DateTime.Now,
                HandledBy = handledBy
            });

            _context.SaveChanges();
        }
    }
}
