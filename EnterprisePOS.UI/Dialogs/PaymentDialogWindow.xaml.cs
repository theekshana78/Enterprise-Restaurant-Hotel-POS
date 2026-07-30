using System;
using System.Windows;
using System.Windows.Controls;

namespace EnterprisePOS.UI.Dialogs
{
    public partial class PaymentDialogWindow : Window
    {
        public decimal GrandTotal { get; }
        public decimal CashTendered { get; private set; }
        public bool IsConfirmed { get; private set; } = false;

        public PaymentDialogWindow(decimal grandTotal)
        {
            InitializeComponent();
            GrandTotal = grandTotal;
            TxtTotalDue.Text = $"LKR {GrandTotal:N0}";
            TxtCashTendered.Text = GrandTotal.ToString("0");
            CalculateChange();
        }

        private void CalculateChange()
        {
            if (TxtChangeDue == null) return;

            decimal.TryParse(TxtCashTendered.Text, out decimal tendered);
            CashTendered = tendered;
            decimal change = tendered - GrandTotal;

            if (change >= 0)
            {
                TxtChangeDue.Text = $"LKR {change:N0}";
                TxtChangeDue.Foreground = (System.Windows.Media.Brush)FindResource("AccentGreen");
            }
            else
            {
                TxtChangeDue.Text = $"Short LKR {Math.Abs(change):N0}";
                TxtChangeDue.Foreground = (System.Windows.Media.Brush)FindResource("AccentRed");
            }
        }

        private void TxtCashTendered_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateChange();
        }

        private void Numpad_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Content is string key)
            {
                if (TxtCashTendered.Text == "0") TxtCashTendered.Text = "";
                TxtCashTendered.Text += key;
            }
        }

        private void BtnBackspace_Click(object sender, RoutedEventArgs e)
        {
            if (TxtCashTendered.Text.Length > 0)
            {
                TxtCashTendered.Text = TxtCashTendered.Text.Substring(0, TxtCashTendered.Text.Length - 1);
                if (string.IsNullOrEmpty(TxtCashTendered.Text))
                    TxtCashTendered.Text = "0";
            }
        }

        private void BtnQuickCash_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tag)
            {
                if (tag == "Exact")
                {
                    TxtCashTendered.Text = GrandTotal.ToString("0");
                }
                else if (decimal.TryParse(tag, out decimal val))
                {
                    TxtCashTendered.Text = val.ToString("0");
                }
            }
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (CashTendered < GrandTotal)
            {
                MessageBox.Show($"Cash tendered (LKR {CashTendered:N0}) is less than total due (LKR {GrandTotal:N0}).", "Payment Insufficient", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            IsConfirmed = true;
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            IsConfirmed = false;
            this.Close();
        }
    }
}
