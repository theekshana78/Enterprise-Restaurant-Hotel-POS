using System;
using System.Collections.Generic;
using System.Linq;
using EnterprisePOS.Core.Entities;
using EnterprisePOS.Data;

namespace EnterprisePOS.Services
{
    public class CustomerService
    {
        private readonly POSDbContext _context;

        public CustomerService(POSDbContext context)
        {
            _context = context;
        }

        public Customer RegisterCustomer(string name, string phone, string? email = null, DateTime? dob = null)
        {
            var existing = _context.Customers.FirstOrDefault(c => c.Phone == phone);
            if (existing != null)
                return existing;

            var customer = new Customer
            {
                Name = name,
                Phone = phone,
                Email = email,
                DateOfBirth = dob,
                LoyaltyPoints = 50, // 50 Welcome bonus points
                MembershipLevel = "Silver",
                RegisteredDate = DateTime.Now
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();
            return customer;
        }

        public void AddLoyaltyPoints(int customerId, decimal totalBillAmount)
        {
            var customer = _context.Customers.Find(customerId);
            if (customer == null) return;

            // 1 Loyalty point earned per LKR 100 spent
            int pointsEarned = (int)(totalBillAmount / 100);
            customer.LoyaltyPoints += pointsEarned;

            // Update membership tier
            if (customer.LoyaltyPoints > 1000) customer.MembershipLevel = "Platinum";
            else if (customer.LoyaltyPoints > 500) customer.MembershipLevel = "Gold";

            _context.SaveChanges();
        }

        public bool RedeemLoyaltyPoints(int customerId, int pointsToRedeem, out decimal discountAmount)
        {
            discountAmount = 0;
            var customer = _context.Customers.Find(customerId);
            if (customer == null || customer.LoyaltyPoints < pointsToRedeem) return false;

            // 10 Loyalty points = LKR 10 discount
            discountAmount = pointsToRedeem * 1.0m;
            customer.LoyaltyPoints -= pointsToRedeem;

            _context.SaveChanges();
            return true;
        }

        public List<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }
    }
}
