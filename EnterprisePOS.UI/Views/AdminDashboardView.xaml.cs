using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EnterprisePOS.Core;
using EnterprisePOS.Data;
using EnterprisePOS.Services;
using EnterprisePOS.UI.Dialogs;

namespace EnterprisePOS.UI.Views
{
    public partial class AdminDashboardView : UserControl
    {
        private readonly POSDbContext _context;
        private readonly AIService _aiService;

        public AdminDashboardView()
        {
            InitializeComponent();
            _context = new POSDbContext();
            _aiService = new AIService(_context);
            LoadAdminDashboard();
        }

        private void LoadAdminDashboard()
        {
            // Tab 1: Metrics & Invoices
            decimal totalRevenue = _context.Invoices.Sum(i => (decimal?)i.GrandTotal) ?? 0;
            int invoiceCount = _context.Invoices.Count();
            int activeRoomsCount = _context.Rooms.Count(r => r.Status == RoomStatus.Occupied);
            int lowStockCount = _context.Products.Count(p => p.CurrentStock <= p.MinStockLevel);

            TxtTotalRevenue.Text = $"LKR {totalRevenue:N0}";
            TxtTotalInvoices.Text = invoiceCount.ToString();
            TxtActiveRooms.Text = $"{activeRoomsCount} / 4";
            TxtLowStockCount.Text = $"{lowStockCount} Items";

            DgInvoices.ItemsSource = _context.Invoices.OrderByDescending(i => i.InvoiceDate).ToList();

            // Tab 2: Products
            DgProducts.ItemsSource = _context.Products.ToList();

            // Tab 3: Users
            DgUsers.ItemsSource = _context.Users.ToList();

            // Tab 4: AI & Security
            DgAIForecast.ItemsSource = _aiService.PredictSalesDemand();
            DgFraudAlerts.ItemsSource = _aiService.DetectFraudulentActivity();
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            var addDlg = new AddProductWindow
            {
                Owner = Window.GetWindow(this)
            };
            addDlg.ShowDialog();

            if (addDlg.IsSaved)
            {
                LoadAdminDashboard();
            }
        }

        private void BtnZReport_Click(object sender, RoutedEventArgs e)
        {
            var zReportDlg = new ZReportWindow("Admin")
            {
                Owner = Window.GetWindow(this)
            };
            zReportDlg.ShowDialog();
        }
    }
}
