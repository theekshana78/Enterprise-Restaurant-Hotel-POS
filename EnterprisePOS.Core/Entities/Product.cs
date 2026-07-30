using System;

namespace EnterprisePOS.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public double CurrentStock { get; set; }
        public double MinStockLevel { get; set; }
        public string Unit { get; set; } = "Pcs"; // Pcs, Bottles, Packs, Rolls
        public ItemType ItemType { get; set; } = ItemType.SalableProduct;
        public bool IsKitchenItem { get; set; } = false; // Send to KOT if true (e.g. Rice, Kottu)
        public string? Brand { get; set; }
        public string? Supplier { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
