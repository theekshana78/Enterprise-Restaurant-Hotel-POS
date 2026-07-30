using System;
using System.Windows;
using System.Windows.Controls;
using EnterprisePOS.Core;
using EnterprisePOS.Data;
using EnterprisePOS.Services;

namespace EnterprisePOS.UI.Views
{
    public partial class TableManagementView : UserControl
    {
        public TableManagementView()
        {
            InitializeComponent();
            LoadTables();
        }

        private void LoadTables()
        {
            try
            {
                using (var db = new POSDbContext())
                {
                    var tableService = new TableService(db);
                    ItemsControlTables.ItemsSource = tableService.GetAllTables();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load tables: {ex.Message}", "Table Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadTables();
        }

        private void BtnAddTable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new POSDbContext())
                {
                    int nextNo = db.Tables.Count() + 1;
                    db.Tables.Add(new Core.Entities.RestaurantTable
                    {
                        TableNumber = $"Table {nextNo:D2}",
                        Capacity = 4,
                        Status = TableStatus.Available
                    });
                    db.SaveChanges();
                }
                LoadTables();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding table: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnManageTable_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int tableId)
            {
                using (var db = new POSDbContext())
                {
                    var table = db.Tables.Find(tableId);
                    if (table != null)
                    {
                        if (table.Status == TableStatus.Available)
                        {
                            var res = MessageBox.Show($"Occupy {table.TableNumber} now?", "Occupy Table", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (res == MessageBoxResult.Yes)
                            {
                                table.Status = TableStatus.Occupied;
                                table.CurrentGuestName = "Dine-In Guest";
                                table.AssignedWaiter = "Waiter 01";
                                db.SaveChanges();
                            }
                        }
                        else if (table.Status == TableStatus.Occupied)
                        {
                            var res = MessageBox.Show($"Set {table.TableNumber} status to Cleaning / Clear?", "Clear Table", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (res == MessageBoxResult.Yes)
                            {
                                table.Status = TableStatus.Available;
                                table.CurrentGuestName = null;
                                table.AssignedWaiter = null;
                                db.SaveChanges();
                            }
                        }
                    }
                }
                LoadTables();
            }
        }
    }
}
