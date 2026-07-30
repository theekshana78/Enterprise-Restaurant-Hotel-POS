using System;
using System.IO;
using EnterprisePOS.Core;
using EnterprisePOS.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnterprisePOS.Data
{
    public class POSDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<Invoice> Invoices { get; set; } = null!;
        public DbSet<InvoiceItem> InvoiceItems { get; set; } = null!;
        public DbSet<KOTOrder> KOTOrders { get; set; } = null!;
        public DbSet<StockTransaction> StockTransactions { get; set; } = null!;
        public DbSet<Expense> Expenses { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Shift> Shifts { get; set; } = null!;
        public DbSet<RestaurantTable> Tables { get; set; } = null!;
        public DbSet<MiniBarItem> MiniBarItems { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Recipe> Recipes { get; set; } = null!;
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<Branch> Branches { get; set; } = null!;

        public POSDbContext()
        {
        }

        public POSDbContext(DbContextOptions<POSDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pos_enterprise.db");
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Branches
            modelBuilder.Entity<Branch>().HasData(
                new Branch { Id = 1, BranchName = "Main Branch (Colombo)", Code = "HQ-01", Address = "123 Galle Road, Colombo", Phone = "011-2345678", IsActive = true },
                new Branch { Id = 2, BranchName = "Kandy Resort Branch", Code = "KD-02", Address = "45 Peradeniya Road, Kandy", Phone = "081-2345678", IsActive = true }
            );

            // Seed Restaurant Tables
            modelBuilder.Entity<RestaurantTable>().HasData(
                new RestaurantTable { Id = 1, TableNumber = "Table 01", Capacity = 2, Status = TableStatus.Available },
                new RestaurantTable { Id = 2, TableNumber = "Table 02", Capacity = 2, Status = TableStatus.Available },
                new RestaurantTable { Id = 3, TableNumber = "Table 03", Capacity = 4, Status = TableStatus.Available },
                new RestaurantTable { Id = 4, TableNumber = "Table 04", Capacity = 4, Status = TableStatus.Available },
                new RestaurantTable { Id = 5, TableNumber = "Table 05", Capacity = 6, Status = TableStatus.Available },
                new RestaurantTable { Id = 6, TableNumber = "Table 06", Capacity = 6, Status = TableStatus.Available },
                new RestaurantTable { Id = 7, TableNumber = "VIP Table 01", Capacity = 8, Status = TableStatus.Available },
                new RestaurantTable { Id = 8, TableNumber = "VIP Table 02", Capacity = 10, Status = TableStatus.Available }
            );

            // Seed Rooms
            modelBuilder.Entity<Room>().HasData(
                new Room { Id = 1, RoomNumber = "Room 101", RoomType = "Standard Double", RatePerNight = 8500, Status = RoomStatus.Available, PendingAccruedBill = 0 },
                new Room { Id = 2, RoomNumber = "Room 102", RoomType = "Standard Double", RatePerNight = 8500, Status = RoomStatus.Available, PendingAccruedBill = 0 },
                new Room { Id = 3, RoomNumber = "Room 201", RoomType = "Deluxe Sea View", RatePerNight = 14000, Status = RoomStatus.Available, PendingAccruedBill = 0 },
                new Room { Id = 4, RoomNumber = "Room 205", RoomType = "Executive Suite", RatePerNight = 22000, Status = RoomStatus.Available, PendingAccruedBill = 0 }
            );

            // Seed Initial Menu & Inventory
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Barcode = "1001", Name = "Chicken Fried Rice", Category = "Food", CostPrice = 700, SellingPrice = 1200, CurrentStock = 100, MinStockLevel = 15, Unit = "Portion", ItemType = ItemType.SalableProduct, IsKitchenItem = true },
                new Product { Id = 2, Barcode = "1002", Name = "Chicken Kottu", Category = "Food", CostPrice = 650, SellingPrice = 1100, CurrentStock = 100, MinStockLevel = 15, Unit = "Portion", ItemType = ItemType.SalableProduct, IsKitchenItem = true },
                new Product { Id = 3, Barcode = "1003", Name = "RedBull Energy Drink 250ml", Category = "Beverages", CostPrice = 850, SellingPrice = 1150, CurrentStock = 45, MinStockLevel = 10, Unit = "Can", ItemType = ItemType.SalableProduct, IsKitchenItem = false },
                new Product { Id = 4, Barcode = "1004", Name = "Mineral Water Bottle 500ml", Category = "Beverages", CostPrice = 50, SellingPrice = 100, CurrentStock = 120, MinStockLevel = 25, Unit = "Bottle", ItemType = ItemType.SalableProduct, IsKitchenItem = false },
                new Product { Id = 5, Barcode = "1005", Name = "Fridge Water Bottle 1L", Category = "Beverages", CostPrice = 90, SellingPrice = 180, CurrentStock = 60, MinStockLevel = 15, Unit = "Bottle", ItemType = ItemType.SalableProduct, IsKitchenItem = false },
                new Product { Id = 6, Barcode = "1006", Name = "Lion Lager Beer 625ml", Category = "Beverages", CostPrice = 550, SellingPrice = 850, CurrentStock = 80, MinStockLevel = 20, Unit = "Bottle", ItemType = ItemType.SalableProduct, IsKitchenItem = false },
                new Product { Id = 7, Barcode = "1007", Name = "Jumbo Peanuts 100g", Category = "Snacks", CostPrice = 180, SellingPrice = 300, CurrentStock = 50, MinStockLevel = 10, Unit = "Pack", ItemType = ItemType.SalableProduct, IsKitchenItem = false },
                new Product { Id = 8, Barcode = "1008", Name = "Dairy Milk Chocolate 50g", Category = "Snacks", CostPrice = 220, SellingPrice = 350, CurrentStock = 40, MinStockLevel = 10, Unit = "Bar", ItemType = ItemType.SalableProduct, IsKitchenItem = false },
                
                // Consumables / Stock Tracking
                new Product { Id = 9, Barcode = "9001", Name = "Toilet Paper Roll", Category = "Consumables", CostPrice = 110, SellingPrice = 0, CurrentStock = 50, MinStockLevel = 15, Unit = "Roll", ItemType = ItemType.ConsumableStock, IsKitchenItem = false },
                new Product { Id = 10, Barcode = "9002", Name = "Thermal Receipt Paper Roll 80mm", Category = "Consumables", CostPrice = 150, SellingPrice = 0, CurrentStock = 30, MinStockLevel = 8, Unit = "Roll", ItemType = ItemType.ConsumableStock, IsKitchenItem = false }
            );

            // Seed Default Users with SHA-256 Hashed Passwords for all roles
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", PasswordHash = SecurityHelper.HashPassword("admin123"), FullName = "System Administrator", Role = UserRole.Admin, IsActive = true, BranchId = 1 },
                new User { Id = 2, Username = "manager", PasswordHash = SecurityHelper.HashPassword("manager123"), FullName = "General Manager", Role = UserRole.Manager, IsActive = true, BranchId = 1 },
                new User { Id = 3, Username = "cashier", PasswordHash = SecurityHelper.HashPassword("cashier123"), FullName = "Head Cashier", Role = UserRole.Cashier, IsActive = true, BranchId = 1 },
                new User { Id = 4, Username = "waiter", PasswordHash = SecurityHelper.HashPassword("waiter123"), FullName = "Senior Waiter", Role = UserRole.Waiter, IsActive = true, BranchId = 1 },
                new User { Id = 5, Username = "chef", PasswordHash = SecurityHelper.HashPassword("chef123"), FullName = "Head Chef", Role = UserRole.KitchenStaff, IsActive = true, BranchId = 1 }
            );
        }
    }
}
