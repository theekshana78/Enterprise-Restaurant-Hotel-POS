using System;

namespace EnterprisePOS.Core.Entities
{
    public class KOTOrder
    {
        public int Id { get; set; }
        public string TicketNumber { get; set; } = string.Empty; // e.g. KOT-101
        public string TargetTableOrRoom { get; set; } = string.Empty; // Table 01 or Room 02
        public string OrderDetailsJson { get; set; } = string.Empty; // Items and notes e.g., Rice x2, Kottu x1 (Less Spicy)
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
