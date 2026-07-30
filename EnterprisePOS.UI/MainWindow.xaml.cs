using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using EnterprisePOS.Core;
using EnterprisePOS.Core.Entities;
using EnterprisePOS.UI.Views;

namespace EnterprisePOS.UI
{
    public partial class MainWindow : Window
    {
        private CashierPOSView _posView;
        private TableManagementView _tableView;
        private RoomManagerView _roomView;
        private KitchenDisplayView _kdsView;
        private InventoryView _inventoryView;
        private AdminDashboardView _adminView;
        public User CurrentUser { get; private set; }

        public MainWindow() : this(new User { FullName = "Head Cashier", Role = UserRole.Cashier })
        {
        }

        public MainWindow(User user)
        {
            InitializeComponent();
            CurrentUser = user;

            _posView = new CashierPOSView();
            _tableView = new TableManagementView();
            _roomView = new RoomManagerView();
            _kdsView = new KitchenDisplayView();
            _inventoryView = new InventoryView(CurrentUser);
            _adminView = new AdminDashboardView();

            TxtUserStatus.Text = $"{CurrentUser.FullName} ({CurrentUser.Role})";

            ApplyRolePermissions();

            // Default initial view based on role
            if (CurrentUser.Role == UserRole.KitchenStaff)
                ShowKDSView();
            else if (CurrentUser.Role == UserRole.Waiter)
                ShowTablesView();
            else
                ShowPOSView();
        }

        private void ApplyRolePermissions()
        {
            if (CurrentUser.Role == UserRole.KitchenStaff)
            {
                BtnNavPOS.Visibility = Visibility.Collapsed;
                BtnNavRooms.Visibility = Visibility.Collapsed;
                BtnNavStock.Visibility = Visibility.Collapsed;
                BtnNavAI.Visibility = Visibility.Collapsed;
            }
            else if (CurrentUser.Role == UserRole.Waiter)
            {
                BtnNavStock.Visibility = Visibility.Collapsed;
                BtnNavAI.Visibility = Visibility.Collapsed;
            }
            else if (CurrentUser.Role == UserRole.Cashier)
            {
                BtnNavAI.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private void ShowPOSView()
        {
            MainContentArea.Content = _posView;
            HighlightButton(BtnNavPOS);
        }

        private void ShowTablesView()
        {
            MainContentArea.Content = _tableView;
            HighlightButton(BtnNavTables);
        }

        private void ShowRoomsView()
        {
            MainContentArea.Content = _roomView;
            HighlightButton(BtnNavRooms);
        }

        private void ShowKDSView()
        {
            MainContentArea.Content = _kdsView;
            HighlightButton(BtnNavKDS);
        }

        private void ShowStockView()
        {
            MainContentArea.Content = _inventoryView;
            HighlightButton(BtnNavStock);
        }

        private void ShowAIView()
        {
            MainContentArea.Content = _adminView;
            HighlightButton(BtnNavAI);
        }

        private void HighlightButton(System.Windows.Controls.Button selectedBtn)
        {
            var defaultBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#334155")!;
            var activeBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#0284C7")!;

            BtnNavPOS.Background = defaultBrush;
            BtnNavTables.Background = defaultBrush;
            BtnNavRooms.Background = defaultBrush;
            BtnNavKDS.Background = defaultBrush;
            BtnNavStock.Background = defaultBrush;
            BtnNavAI.Background = defaultBrush;

            selectedBtn.Background = activeBrush;
        }

        private void BtnNavPOS_Click(object sender, RoutedEventArgs e) => ShowPOSView();
        private void BtnNavTables_Click(object sender, RoutedEventArgs e) => ShowTablesView();
        private void BtnNavRooms_Click(object sender, RoutedEventArgs e) => ShowRoomsView();
        private void BtnNavKDS_Click(object sender, RoutedEventArgs e) => ShowKDSView();
        private void BtnNavStock_Click(object sender, RoutedEventArgs e) => ShowStockView();
        private void BtnNavAI_Click(object sender, RoutedEventArgs e) => ShowAIView();

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                    if (BtnNavPOS.Visibility == Visibility.Visible) ShowPOSView();
                    e.Handled = true;
                    break;
                case Key.F2:
                    if (BtnNavTables.Visibility == Visibility.Visible) ShowTablesView();
                    e.Handled = true;
                    break;
                case Key.F3:
                    if (BtnNavRooms.Visibility == Visibility.Visible) ShowRoomsView();
                    e.Handled = true;
                    break;
                case Key.F4:
                    if (BtnNavKDS.Visibility == Visibility.Visible) ShowKDSView();
                    e.Handled = true;
                    break;
                case Key.F5:
                    if (BtnNavStock.Visibility == Visibility.Visible) ShowStockView();
                    e.Handled = true;
                    break;
                case Key.F6:
                    if (BtnNavAI.Visibility == Visibility.Visible) ShowAIView();
                    e.Handled = true;
                    break;
            }
        }
    }
}