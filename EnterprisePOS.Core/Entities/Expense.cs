using System;

namespace EnterprisePOS.Core.Entities
{
    public class Expense
    {
        public int Id { get; set; }
        public string Category { get; set; } = "General"; // Utility, Staff Salary, Repairs, Inventory Purchase
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; } = DateTime.Now;
        public string ApprovedBy { get; set; } = "Admin";
    }
}
