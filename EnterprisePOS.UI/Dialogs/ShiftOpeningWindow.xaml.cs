using System;
using System.Windows;
using EnterprisePOS.Core.Entities;
using EnterprisePOS.Data;
using EnterprisePOS.Services;

namespace EnterprisePOS.UI.Dialogs
{
    public partial class ShiftOpeningWindow : Window
    {
        private readonly User _user;

        public ShiftOpeningWindow(User user)
        {
            InitializeComponent();
            _user = user;
            TxtCashierName.Text = user.FullName;
            TxtTerminalName.Text = Environment.MachineName;
        }

        private void BtnStartShift_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(TxtOpeningCash.Text.Trim(), out decimal openingCash) || openingCash < 0)
            {
                MessageBox.Show("Please enter a valid positive opening cash amount.", "Shift Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var db = new POSDbContext())
                {
                    var shiftService = new ShiftService(db);
                    shiftService.StartShift(_user.Username, Environment.MachineName, openingCash);
                    var auditService = new AuditService(db);
                    auditService.LogActivity(_user.Username, "SHIFT_STARTED", $"Started shift with opening float of LKR {openingCash:N2}");
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start shift: {ex.Message}", "Shift Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
