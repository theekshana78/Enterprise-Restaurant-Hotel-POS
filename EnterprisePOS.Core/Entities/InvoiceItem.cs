namespace EnterprisePOS.Core.Entities
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public double Quantity { get; set; }
        public decimal TotalPrice => UnitPrice * (decimal)Quantity;
    }
}
