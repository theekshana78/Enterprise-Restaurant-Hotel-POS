using System;

namespace EnterprisePOS.Core.Entities
{
    public class MiniBarItem
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public double Quantity { get; set; }
        public DateTime ConsumedAt { get; set; } = DateTime.Now;
        public bool IsBilled { get; set; } = false;
        public string ConsumedByGuest { get; set; } = string.Empty;
    }
}
