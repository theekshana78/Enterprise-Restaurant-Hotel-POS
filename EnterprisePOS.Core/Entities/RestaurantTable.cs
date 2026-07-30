using System;

namespace EnterprisePOS.Core.Entities
{
    public class RestaurantTable
    {
        public int Id { get; set; }
        public string TableNumber { get; set; } = string.Empty;
        public int Capacity { get; set; } = 4;
        public TableStatus Status { get; set; } = TableStatus.Available;
        public string? AssignedWaiter { get; set; }
        public string? CurrentGuestName { get; set; }
        public DateTime? ReservedTime { get; set; }
        public int? CurrentInvoiceId { get; set; }
    }
}
