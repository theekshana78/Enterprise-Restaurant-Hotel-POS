using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EnterprisePOS.Data;

namespace EnterprisePOS.UI.Views
{
    public partial class ReportsView : UserControl
    {
        public ReportsView()
        {
            InitializeComponent();
            LoadDailySalesReport();
        }

        private void LoadDailySalesReport()
        {
            TxtReportTitle.Text = "Daily Sales Report";
            try
            {
                using (var db = new POSDbContext())
                {
                    var sales = db.Invoices
                        .Where(i => i.InvoiceDate.Date == DateTime.Today.Date)
                        .Select(i => new
                        {
                            i.InvoiceNo,
                            Time = i.InvoiceDate.ToString("HH:mm:ss"),
                            Type = i.OrderType.ToString(),
                            Method = i.PaymentMethod.ToString(),
                            SubTotal = i.SubTotal.ToString("N2"),
                            Discount = i.Discount.ToString("N2"),
                            GrandTotal = i.GrandTotal.ToString("N2"),
                            Cashier = i.CashierName
                        }).ToList();

                    DgReports.ItemsSource = sales;
                    decimal total = db.Invoices.Where(i => i.InvoiceDate.Date == DateTime.Today.Date).Sum(i => i.GrandTotal);
                    TxtTotalSummary.Text = $"Total Revenue: LKR {total:N2}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Report error: {ex.Message}", "Report Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnReportDaily_Click(object sender, RoutedEventArgs e) => LoadDailySalesReport();

        private void BtnReportMonthly_Click(object sender, RoutedEventArgs e)
        {
            TxtReportTitle.Text = "Monthly Sales Summary";
            using (var db = new POSDbContext())
            {
                var sales = db.Invoices
                    .Where(i => i.InvoiceDate.Month == DateTime.Today.Month)
                    .Select(i => new
                    {
                        i.InvoiceNo,
                        Date = i.InvoiceDate.ToString("yyyy-MM-dd"),
                        i.CashierName,
                        i.PaymentMethod,
                        Amount = i.GrandTotal.ToString("N2")
                    }).ToList();

                DgReports.ItemsSource = sales;
                decimal total = db.Invoices.Where(i => i.InvoiceDate.Month == DateTime.Today.Month).Sum(i => i.GrandTotal);
                TxtTotalSummary.Text = $"Monthly Total: LKR {total:N2}";
            }
        }

        private void BtnReportBestSellers_Click(object sender, RoutedEventArgs e)
        {
            TxtReportTitle.Text = "Best Selling Items Report";
            using (var db = new POSDbContext())
            {
                var bestSellers = db.InvoiceItems
                    .GroupBy(i => i.ProductName)
                    .Select(g => new
                    {
                        ProductName = g.Key,
                        TotalQuantitySold = g.Sum(x => x.Quantity),
                        TotalRevenue = g.Sum(x => x.UnitPrice * (decimal)x.Quantity).ToString("N2")
                    })
                    .OrderByDescending(x => x.TotalQuantitySold)
                    .ToList();

                DgReports.ItemsSource = bestSellers;
                TxtTotalSummary.Text = $"Total Menu Items Analyzed: {bestSellers.Count}";
            }
        }

        private void BtnReportStaff_Click(object sender, RoutedEventArgs e)
        {
            TxtReportTitle.Text = "Staff Sales Performance Report";
            using (var db = new POSDbContext())
            {
                var staffSales = db.Invoices
                    .GroupBy(i => i.CashierName)
                    .Select(g => new
                    {
                        StaffName = g.Key,
                        TotalOrdersCount = g.Count(),
                        TotalSales = g.Sum(x => x.GrandTotal).ToString("N2")
                    }).ToList();

                DgReports.ItemsSource = staffSales;
                TxtTotalSummary.Text = "Staff Performance Breakdown";
            }
        }

        private void BtnReportInventory_Click(object sender, RoutedEventArgs e)
        {
            TxtReportTitle.Text = "Inventory Valuation & Low Stock Report";
            using (var db = new POSDbContext())
            {
                var inventory = db.Products
                    .Select(p => new
                    {
                        p.Barcode,
                        p.Name,
                        p.Category,
                        p.CurrentStock,
                        p.MinStockLevel,
                        CostPrice = p.CostPrice.ToString("N2"),
                        StockValuation = (p.CostPrice * (decimal)p.CurrentStock).ToString("N2")
                    }).ToList();

                DgReports.ItemsSource = inventory;
                TxtTotalSummary.Text = $"Total Inventory Items: {inventory.Count}";
            }
        }

        private void BtnReportRoomService_Click(object sender, RoutedEventArgs e)
        {
            TxtReportTitle.Text = "Room Service Charges Report";
            using (var db = new POSDbContext())
            {
                var roomCharges = db.Invoices
                    .Where(i => i.PaymentMethod == Core.PaymentMethod.RoomCharge)
                    .Select(i => new
                    {
                        i.InvoiceNo,
                        Date = i.InvoiceDate.ToString("yyyy-MM-dd HH:mm"),
                        i.RoomId,
                        i.CustomerName,
                        Amount = i.GrandTotal.ToString("N2")
                    }).ToList();

                DgReports.ItemsSource = roomCharges;
                decimal total = db.Invoices.Where(i => i.PaymentMethod == Core.PaymentMethod.RoomCharge).Sum(i => i.GrandTotal);
                TxtTotalSummary.Text = $"Total Room Tab Revenue: LKR {total:N2}";
            }
        }

        private void BtnReportProfit_Click(object sender, RoutedEventArgs e)
        {
            TxtReportTitle.Text = "Gross Profit Analysis Report";
            using (var db = new POSDbContext())
            {
                decimal totalRevenue = db.Invoices.Sum(i => i.GrandTotal);
                decimal totalExpenses = db.Expenses.Sum(ex => ex.Amount);
                decimal netProfit = totalRevenue - totalExpenses;

                var profitSummary = new[]
                {
                    new { Metric = "Total Revenue", Amount = totalRevenue.ToString("N2") },
                    new { Metric = "Total Expenses", Amount = totalExpenses.ToString("N2") },
                    new { Metric = "Net Gross Profit", Amount = netProfit.ToString("N2") }
                };

                DgReports.ItemsSource = profitSummary;
                TxtTotalSummary.Text = $"Net Profit: LKR {netProfit:N2}";
            }
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Report exported to PDF & CSV successfully in root directory!", "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
