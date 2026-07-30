using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EnterprisePOS.Core;
using EnterprisePOS.Core.Entities;
using EnterprisePOS.Data;
using EnterprisePOS.Services;
using EnterprisePOS.UI.Dialogs;

namespace EnterprisePOS.UI.Views
{
    public partial class CashierPOSView : UserControl
    {
        private readonly POSDbContext _context;
        private readonly BillingService _billingService;
        private readonly KOTService _kotService;
        private ObservableCollection<InvoiceItem> _cartItems = new ObservableCollection<InvoiceItem>();
        private List<Product> _allProducts = new List<Product>();

        public CashierPOSView()
        {
            InitializeComponent();
            _context = new POSDbContext();
            _billingService = new BillingService(_context);
            _kotService = new KOTService(_context);

            DgCart.ItemsSource = _cartItems;
            LoadProducts();
        }

        private void LoadProducts(string category = "All")
        {
            _allProducts = _billingService.GetSalableProducts();
            if (category != "All")
            {
                ItemsProductGrid.ItemsSource = _allProducts.Where(p => p.Category == category).ToList();
            }
            else
            {
                ItemsProductGrid.ItemsSource = _allProducts;
            }
        }

        private void BtnCategory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string category)
            {
                LoadProducts(category);
            }
        }

        private void BtnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Product product)
            {
                AddToCart(product);
            }
        }

        private void AddToCart(Product product)
        {
            var existing = _cartItems.FirstOrDefault(i => i.ProductId == product.Id);
            if (existing != null)
            {
                existing.Quantity++;
                DgCart.Items.Refresh();
            }
            else
            {
                _cartItems.Add(new InvoiceItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.SellingPrice,
                    Quantity = 1
                });
            }
            UpdateCartSummary();
        }

        private void BtnRemoveCartItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is InvoiceItem item)
            {
                _cartItems.Remove(item);
                UpdateCartSummary();
            }
        }

        private void BtnClearCart_Click(object sender, RoutedEventArgs e)
        {
            _cartItems.Clear();
            UpdateCartSummary();
        }

        private void TxtDiscount_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCartSummary();
        }

        private void UpdateCartSummary()
        {
            if (TxtSubTotal == null || TxtGrandTotal == null || TxtDiscount == null || _cartItems == null)
                return;

            decimal subTotal = _cartItems.Sum(i => i.UnitPrice * (decimal)i.Quantity);
            decimal.TryParse(TxtDiscount.Text, out decimal discount);

            decimal grandTotal = Math.Max(0, subTotal - discount);

            TxtSubTotal.Text = $"LKR {subTotal:N0}";
            TxtGrandTotal.Text = $"LKR {grandTotal:N0}";
        }

        private void TxtBarcode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string code = TxtBarcode.Text.Trim();
                if (!string.IsNullOrEmpty(code))
                {
                    var product = _billingService.FindProductByBarcode(code);
                    if (product != null)
                    {
                        AddToCart(product);
                        TxtBarcode.Clear();
                    }
                    else
                    {
                        MessageBox.Show($"Product with barcode '{code}' not found.", "Barcode Search", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        private void BtnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            TxtBarcode.Clear();
            LoadProducts("All");
        }

        private void BtnPayCash_Click(object sender, RoutedEventArgs e)
        {
            ProcessPayment(PaymentMethod.Cash);
        }

        private void BtnPayCard_Click(object sender, RoutedEventArgs e)
        {
            ProcessPayment(PaymentMethod.Card);
        }

        private void BtnPayRoomCharge_Click(object sender, RoutedEventArgs e)
        {
            var activeRooms = _context.Rooms.Where(r => r.Status == RoomStatus.Occupied).ToList();
            if (!activeRooms.Any())
            {
                MessageBox.Show("No occupied rooms available to charge.", "Room Charge Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Default to first occupied room or room 1 for rapid checkout
            int targetRoomId = activeRooms.First().Id;
            ProcessPayment(PaymentMethod.RoomCharge, targetRoomId);
        }

        private void ProcessPayment(PaymentMethod method, int? roomId = null)
        {
            if (!_cartItems.Any())
            {
                MessageBox.Show("Please add items to cart before completing sale.", "Empty Cart", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                decimal subTotal = _cartItems.Sum(i => i.UnitPrice * (decimal)i.Quantity);
                decimal.TryParse(TxtDiscount.Text, out decimal discount);
                decimal grandTotal = Math.Max(0, subTotal - discount);

                decimal cashTendered = grandTotal;

                if (method == PaymentMethod.Cash)
                {
                    var payDlg = new PaymentDialogWindow(grandTotal)
                    {
                        Owner = Window.GetWindow(this)
                    };
                    payDlg.ShowDialog();

                    if (!payDlg.IsConfirmed) return;
                    cashTendered = payDlg.CashTendered;
                }

                var invoice = _billingService.CreateInvoice(_cartItems.ToList(), discount, method, "Cashier", roomId);

                // Auto Print KOT for Kitchen items if any
                _kotService.GenerateKOT(roomId.HasValue ? $"Room {roomId}" : "Dine-In", _cartItems.ToList());

                // Show Commercial Thermal Receipt Preview & Print Window
                var receiptWindow = new ReceiptWindow(invoice, cashTendered)
                {
                    Owner = Window.GetWindow(this)
                };
                receiptWindow.ShowDialog();

                _cartItems.Clear();
                TxtDiscount.Text = "0";
                UpdateCartSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing transaction: {ex.Message}", "Transaction Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnPrintKOT_Click(object sender, RoutedEventArgs e)
        {
            if (!_cartItems.Any())
            {
                MessageBox.Show("No items in cart to print KOT.", "KOT Printer", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var kot = _kotService.GenerateKOT("Restaurant Table", _cartItems.ToList());
            if (kot != null)
            {
                MessageBox.Show($"KOT {kot.TicketNumber} printed to Kitchen Printer successfully!", "KOT Printed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Cart contains no kitchen items (e.g. Rice/Kottu) to print KOT.", "KOT Printer", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnPrintPreBill_Click(object sender, RoutedEventArgs e)
        {
            if (!_cartItems.Any())
            {
                MessageBox.Show("Please add items to cart before printing Pre-Bill Table Slip.", "Empty Cart", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            decimal subTotal = _cartItems.Sum(i => i.UnitPrice * (decimal)i.Quantity);
            decimal.TryParse(TxtDiscount.Text, out decimal discount);
            decimal grandTotal = Math.Max(0, subTotal - discount);

            var tempInvoice = new Invoice
            {
                InvoiceNo = $"PRE-BILL-{DateTime.Now:HHmmss}",
                InvoiceDate = DateTime.Now,
                SubTotal = subTotal,
                Discount = discount,
                GrandTotal = grandTotal,
                CashierName = "Cashier",
                CustomerName = "Table / Dine-In Guest",
                Items = _cartItems.ToList()
            };

            var preBillWindow = new ReceiptWindow(tempInvoice, 0, isPreBill: true)
            {
                Owner = Window.GetWindow(this)
            };
            preBillWindow.ShowDialog();
        }
    }
}
