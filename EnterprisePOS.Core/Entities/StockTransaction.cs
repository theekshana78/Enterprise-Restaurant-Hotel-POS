using System;

namespace EnterprisePOS.Core.Entities
{
    public class StockTransaction
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public double QuantityChange { get; set; } // Positive for addition/purchase, negative for sales/usage
        public string Reason { get; set; } = "Sale"; // Purchase, Sale, Usage, Damage, Adjustment
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public string HandledBy { get; set; } = "Admin";
    }
}
