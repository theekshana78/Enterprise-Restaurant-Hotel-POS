using System;

namespace EnterprisePOS.Core.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; } = string.Empty; // e.g. Room 01, Room 02, Room 03, Room 04 (Family)
        public string RoomType { get; set; } = "Double"; // Double, Family
        public decimal RatePerNight { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Available;
        public string? CurrentGuestName { get; set; }
        public string? CurrentGuestPhone { get; set; }
        public DateTime? CheckInTime { get; set; }
        public decimal PendingAccruedBill { get; set; } = 0; // Total food/drinks charged to room tab
    }
}
