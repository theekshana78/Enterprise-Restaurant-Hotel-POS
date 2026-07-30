using System;

namespace EnterprisePOS.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Cashier;
        public bool IsActive { get; set; } = true;
        public int FailedLoginAttempts { get; set; } = 0;
        public bool IsLocked { get; set; } = false;
        public int? BranchId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
