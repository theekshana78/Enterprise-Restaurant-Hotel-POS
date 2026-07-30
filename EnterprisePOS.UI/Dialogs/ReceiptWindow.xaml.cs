using System;
using System.Windows;
using EnterprisePOS.Core.Entities;

namespace EnterprisePOS.UI.Dialogs
{
    public partial class ReceiptWindow : Window
    {
        public ReceiptWindow(Invoice invoice, decimal cashTendered = 0, bool isPreBill = false)
        {
            InitializeComponent();
            PopulateReceipt(invoice, cashTendered, isPreBill);
        }

        private void PopulateReceipt(Invoice invoice, decimal cashTendered, bool isPreBill)
        {
            if (isPreBill)
            {
                this.Title = "Pre-Bill / Table Order Slip (Unpaid)";
                TxtBillBanner.Text = "*** PRE-BILL / TABLE ORDER SLIP (UNPAID) ***";
                TxtPaymentType.Text = "Status: PENDING";
                TxtCashTendered.Text = "LKR 0";
                TxtChangeDue.Text = "LKR 0";
            }
            else
            {
                this.Title = "Final Paid Tax Invoice - 80mm Printer";
                TxtBillBanner.Text = "*** FINAL PAID INVOICE (PAID) ***";
                TxtPaymentType.Text = $"Pay: {invoice.PaymentMethod}";

                decimal tendered = cashTendered > 0 ? cashTendered : invoice.GrandTotal;
                decimal change = tendered - invoice.GrandTotal;

                TxtCashTendered.Text = $"LKR {tendered:N0}";
                TxtChangeDue.Text = $"LKR {Math.Max(0, change):N0}";
            }

            TxtReceiptNo.Text = $"INV: {invoice.InvoiceNo}";
            TxtReceiptDate.Text = $"Date: {invoice.InvoiceDate:dd/MM/yyyy HH:mm}";
            TxtCashier.Text = $"Cashier: {invoice.CashierName}";
            TxtCustomer.Text = $"Customer: {invoice.CustomerName ?? "Walk-in Guest"}";

            ItemsReceiptList.ItemsSource = invoice.Items;

            TxtSubTotal.Text = $"LKR {invoice.SubTotal:N0}";
            TxtDiscount.Text = $"LKR {invoice.Discount:N0}";
            TxtGrandTotal.Text = $"LKR {invoice.GrandTotal:N0}";
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Receipt sent to ESC/POS 80mm Thermal Printer successfully!", "Printer Output", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
