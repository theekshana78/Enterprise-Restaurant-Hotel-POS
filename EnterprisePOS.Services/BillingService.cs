using System;
using System.Collections.Generic;
using System.Linq;
using EnterprisePOS.Core;
using EnterprisePOS.Core.Entities;
using EnterprisePOS.Data;
using Microsoft.EntityFrameworkCore;

namespace EnterprisePOS.Services
{
    public class BillingService
    {
        private readonly POSDbContext _context;

        public BillingService(POSDbContext context)
        {
            _context = context;
        }

        public Product? FindProductByBarcode(string barcode)
        {
            return _context.Products.FirstOrDefault(p => p.Barcode == barcode && p.ItemType == ItemType.SalableProduct);
        }

        public List<Product> GetSalableProducts()
        {
            return _context.Products.Where(p => p.ItemType == ItemType.SalableProduct).ToList();
        }

        public Invoice CreateInvoice(List<InvoiceItem> items, decimal discount, PaymentMethod paymentMethod, string cashierName, int? roomId = null, string? customerName = null)
        {
            if (items == null || !items.Any())
                throw new InvalidOperationException("Cannot create an empty invoice.");

            decimal subTotal = items.Sum(i => i.UnitPrice * (decimal)i.Quantity);
            decimal grandTotal = Math.Max(0, subTotal - discount);

            string invoiceNo = $"INV-{DateTime.Now:yyyyMMdd}-{_context.Invoices.Count() + 1:D4}";

            var invoice = new Invoice
            {
                InvoiceNo = invoiceNo,
                InvoiceDate = DateTime.Now,
                SubTotal = subTotal,
                Discount = discount,
                Tax = 0,
                GrandTotal = grandTotal,
                PaymentMethod = paymentMethod,
                CashierName = cashierName,
                RoomId = roomId,
                CustomerName = customerName,
                Items = items
            };

            // Deduct stock for sold items & recipe ingredients
            foreach (var item in items)
            {
                var product = _context.Products.Find(item.ProductId);
                if (product != null)
                {
                    product.CurrentStock -= item.Quantity;
                    _context.StockTransactions.Add(new StockTransaction
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        QuantityChange = -item.Quantity,
                        Reason = $"Sale: {invoiceNo}",
                        TransactionDate = DateTime.Now,
                        HandledBy = cashierName
                    });

                    // Check if product has a recipe and deduct raw ingredients
                    var recipe = _context.Recipes.FirstOrDefault(r => r.ProductId == product.Id);
                    if (recipe != null)
                    {
                        var ingredients = _context.RecipeIngredients.Where(ri => ri.RecipeId == recipe.Id).ToList();
                        foreach (var ing in ingredients)
                        {
                            var rawProduct = _context.Products.Find(ing.IngredientProductId);
                            if (rawProduct != null)
                            {
                                double totalIngredientDeduction = ing.QuantityRequired * item.Quantity;
                                rawProduct.CurrentStock -= totalIngredientDeduction;
                                _context.StockTransactions.Add(new StockTransaction
                                {
                                    ProductId = rawProduct.Id,
                                    ProductName = rawProduct.Name,
                                    QuantityChange = -totalIngredientDeduction,
                                    Reason = $"Recipe Ingredient Deduction ({product.Name}): {invoiceNo}",
                                    TransactionDate = DateTime.Now,
                                    HandledBy = cashierName
                                });
                            }
                        }
                    }
                }
            }

            // Handle Room Charge payment method
            if (paymentMethod == PaymentMethod.RoomCharge && roomId.HasValue)
            {
                var room = _context.Rooms.Find(roomId.Value);
                if (room != null && room.Status == RoomStatus.Occupied)
                {
                    room.PendingAccruedBill += grandTotal;
                }
            }

            _context.Invoices.Add(invoice);
            _context.SaveChanges();

            return invoice;
        }
    }
}
