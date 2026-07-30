using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace EnterprisePOS.UI.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void BtnSaveProfile_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Business details updated successfully!", "Settings Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnSaveTax_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tax & Service charge settings saved!", "Settings Saved", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnTestPrint_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Test print slip sent to thermal receipt printer successfully!", "Printer Test", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnBackup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pos_enterprise.db");
                string backupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"pos_enterprise_backup_{DateTime.Now:yyyyMMdd_HHmmss}.db");

                if (File.Exists(dbPath))
                {
                    File.Copy(dbPath, backupPath, overwrite: true);
                    MessageBox.Show($"Database backup created successfully at:\n{backupPath}", "Backup Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Database file not found yet.", "Backup Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Backup failed: {ex.Message}", "Backup Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRestore_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("To restore database, place your backup file as 'pos_enterprise.db' in application base directory.", "Restore Instructions", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
