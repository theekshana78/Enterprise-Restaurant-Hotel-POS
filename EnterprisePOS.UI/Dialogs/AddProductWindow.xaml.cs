using System;
using System.Windows;
using System.Windows.Controls;
using EnterprisePOS.Core;
using EnterprisePOS.Core.Entities;
using EnterprisePOS.Data;

namespace EnterprisePOS.UI.Dialogs
{
    public partial class AddProductWindow : Window
    {
        public bool IsSaved { get; private set; } = false;

        public AddProductWindow()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string name = TxtName.Text.Trim();
            string barcode = TxtBarcode.Text.Trim();
            string category = (CmbCategory.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Food";
            string unit = TxtUnit.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter Product Name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(TxtSellingPrice.Text, out decimal sellingPrice) || sellingPrice <= 0)
            {
                MessageBox.Show("Please enter valid Selling Price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            decimal.TryParse(TxtCostPrice.Text, out decimal costPrice);
            double.TryParse(TxtStock.Text, out double stock);
            double.TryParse(TxtMinStock.Text, out double minStock);

            try
            {
                using (var db = new POSDbContext())
                {
                    if (string.IsNullOrEmpty(barcode))
                    {
                        barcode = (1000 + db.Products.Count() + 1).ToString();
                    }

                    var product = new Product
                    {
                        Name = name,
                        Barcode = barcode,
                        Category = category,
                        Unit = string.IsNullOrEmpty(unit) ? "Pcs" : unit,
                        CostPrice = costPrice,
                        SellingPrice = sellingPrice,
                        CurrentStock = stock,
                        MinStockLevel = minStock,
                        ItemType = category == "Consumables" ? ItemType.ConsumableStock : ItemType.SalableProduct,
                        IsKitchenItem = ChkIsKitchen.IsChecked ?? false,
                        CreatedAt = DateTime.Now
                    };

                    db.Products.Add(product);
                    db.SaveChanges();
                }

                IsSaved = true;
                MessageBox.Show($"Product '{name}' added to inventory successfully!", "Product Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error saving product: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            IsSaved = false;
            this.Close();
        }
    }
}
