using System;
using System.Collections.Generic;

namespace EnterprisePOS.Core.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; } = string.Empty; // Auto generated e.g., INV-20260728-001
        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal ServiceCharge { get; set; } = 0;
        public decimal GrandTotal { get; set; }
        public OrderType OrderType { get; set; } = OrderType.DineIn;
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;
        public string CashierName { get; set; } = "Admin";
        public int? ShiftId { get; set; }
        public int? RoomId { get; set; } // Linked room if paid/charged to room
        public int? TableId { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string OrderStatus { get; set; } = "Completed"; // Completed, Held, Voided

        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    }
}
