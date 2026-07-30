using System;

namespace EnterprisePOS.Core.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public int LoyaltyPoints { get; set; } = 0;
        public string MembershipLevel { get; set; } = "Silver";
        public DateTime? DateOfBirth { get; set; }
        public decimal CreditBalance { get; set; } = 0;
        public DateTime RegisteredDate { get; set; } = DateTime.Now;
    }
}
