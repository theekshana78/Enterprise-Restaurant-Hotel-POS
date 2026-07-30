using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EnterprisePOS.Core;
using EnterprisePOS.Data;
using EnterprisePOS.Services;

namespace EnterprisePOS.UI.Views
{
    public partial class KitchenDisplayView : UserControl
    {
        public KitchenDisplayView()
        {
            InitializeComponent();
            LoadKOTOrders();
        }

        private void LoadKOTOrders()
        {
            try
            {
                using (var db = new POSDbContext())
                {
                    var kotService = new KOTService(db);
                    ItemsControlKOT.ItemsSource = kotService.GetActiveKOTOrders();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load KOT orders: {ex.Message}", "KDS Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadKOTOrders();
        }

        private void UpdateKOTStatus(object sender, OrderStatus status)
        {
            if (sender is Button btn && btn.Tag is int kotId)
            {
                using (var db = new POSDbContext())
                {
                    var kotService = new KOTService(db);
                    kotService.UpdateKOTStatus(kotId, status);
                }
                LoadKOTOrders();
            }
        }

        private void BtnSetPreparing_Click(object sender, RoutedEventArgs e) => UpdateKOTStatus(sender, OrderStatus.Preparing);
        private void BtnSetReady_Click(object sender, RoutedEventArgs e) => UpdateKOTStatus(sender, OrderStatus.Ready);
        private void BtnSetServed_Click(object sender, RoutedEventArgs e) => UpdateKOTStatus(sender, OrderStatus.Served);
        private void BtnSetCancelled_Click(object sender, RoutedEventArgs e) => UpdateKOTStatus(sender, OrderStatus.Cancelled);
    }
}
