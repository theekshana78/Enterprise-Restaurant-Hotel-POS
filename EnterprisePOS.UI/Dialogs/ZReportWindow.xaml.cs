using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EnterprisePOS.Core;
using EnterprisePOS.Data;

namespace EnterprisePOS.UI.Dialogs
{
    public partial class ZReportWindow : Window
    {
        private decimal _expectedCash = 0;

        public ZReportWindow(string cashierName)
        {
            InitializeComponent();
            TxtShiftMeta.Text = $"Cashier: {cashierName} | Closing Date: {DateTime.Now:dd/MM/yyyy HH:mm}";
            CalculateShiftTotals();
        }

        private void CalculateShiftTotals()
        {
            using (var db = new POSDbContext())
            {
                var todayInvoices = db.Invoices.Where(i => i.InvoiceDate.Date == DateTime.Today.Date).ToList();

                int totalCount = todayInvoices.Count;
                decimal cashSales = todayInvoices.Where(i => i.PaymentMethod == PaymentMethod.Cash).Sum(i => i.GrandTotal);
                decimal cardSales = todayInvoices.Where(i => i.PaymentMethod == PaymentMethod.Card).Sum(i => i.GrandTotal);
                decimal roomSales = todayInvoices.Where(i => i.PaymentMethod == PaymentMethod.RoomCharge).Sum(i => i.GrandTotal);
                decimal totalDiscounts = todayInvoices.Sum(i => i.Discount);
                decimal grossRevenue = todayInvoices.Sum(i => i.GrandTotal);

                _expectedCash = cashSales;

                TxtInvoicesCount.Text = totalCount.ToString();
                TxtCashSales.Text = $"LKR {cashSales:N0}";
                TxtCardSales.Text = $"LKR {cardSales:N0}";
                TxtRoomTabSales.Text = $"LKR {roomSales:N0}";
                TxtDiscounts.Text = $"LKR {totalDiscounts:N0}";
                TxtGrossRevenue.Text = $"LKR {grossRevenue:N0}";

                TxtActualCash.Text = _expectedCash.ToString("0");
                UpdateVariance();
            }
        }

        private void TxtActualCash_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateVariance();
        }

        private void UpdateVariance()
        {
            if (TxtVariance == null) return;

            decimal.TryParse(TxtActualCash.Text, out decimal actual);
            decimal variance = actual - _expectedCash;

            if (variance == 0)
            {
                TxtVariance.Text = "LKR 0 (Balanced)";
                TxtVariance.Foreground = (System.Windows.Media.Brush)FindResource("AccentGreen");
            }
            else if (variance > 0)
            {
                TxtVariance.Text = $"+LKR {variance:N0} (Excess)";
                TxtVariance.Foreground = (System.Windows.Media.Brush)FindResource("AccentAmber");
            }
            else
            {
                TxtVariance.Text = $"-LKR {Math.Abs(variance):N0} (Shortage)";
                TxtVariance.Foreground = (System.Windows.Media.Brush)FindResource("AccentRed");
            }
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            decimal.TryParse(TxtActualCash.Text, out decimal actual);
            decimal variance = actual - _expectedCash;

            MessageBox.Show($"Shift Closed & Z-Report Sent to Thermal Printer!\n\nGross Sales: {TxtGrossRevenue.Text}\nExpected Cash: LKR {_expectedCash:N0}\nActual Cash: LKR {actual:N0}\nVariance: LKR {variance:N0}", "Z-Report Printed", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
