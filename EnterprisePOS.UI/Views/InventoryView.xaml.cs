using System.Windows;
using System.Windows.Controls;
using EnterprisePOS.Core;
using EnterprisePOS.Core.Entities;
using EnterprisePOS.Data;
using EnterprisePOS.Services;

namespace EnterprisePOS.UI.Views
{
    public partial class InventoryView : UserControl
    {
        private readonly POSDbContext _context;
        private readonly StockService _stockService;
        private readonly User? _currentUser;

        public InventoryView() : this(new User { Role = UserRole.Cashier })
        {
        }

        public InventoryView(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _context = new POSDbContext();
            _stockService = new StockService(_context);
            
            ApplyRolePermissions();
            LoadInventory();
        }

        private void ApplyRolePermissions()
        {
            // Strict RBAC: Only Admin can refill stock. Hide button for Cashiers.
            if (_currentUser != null && _currentUser.Role != UserRole.Admin)
            {
                BtnRefillStock.Visibility = Visibility.Collapsed;
            }
            else
            {
                BtnRefillStock.Visibility = Visibility.Visible;
            }
        }

        private void LoadInventory()
        {
            DgInventory.ItemsSource = _stockService.GetAllStockItems();
        }

        private void BtnRefillStock_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser != null && _currentUser.Role != UserRole.Admin)
            {
                MessageBox.Show("Access Denied: Only Administrators can refill or adjust stock levels.", "Admin Permission Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DgInventory.SelectedItem is Product selectedProduct)
            {
                string qtyStr = Microsoft.VisualBasic.Interaction.InputBox($"Enter Quantity to Add for '{selectedProduct.Name}':", "Refill Stock", "20");
                if (double.TryParse(qtyStr, out double qty) && qty > 0)
                {
                    _stockService.AddStock(selectedProduct.Id, qty, selectedProduct.CostPrice, _currentUser?.FullName ?? "Admin");
                    MessageBox.Show($"Added {qty} {selectedProduct.Unit} to {selectedProduct.Name} successfully!", "Stock Refilled", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadInventory();
                }
            }
            else
            {
                MessageBox.Show("Please select a product row from the table to refill stock.", "Stock Refill", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
