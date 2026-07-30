using System;
using System.Threading.Tasks;
using System.Windows;
using EnterprisePOS.Data;

namespace EnterprisePOS.UI.Dialogs
{
    public partial class SplashScreenWindow : Window
    {
        public SplashScreenWindow()
        {
            InitializeComponent();
            Loaded += SplashScreenWindow_Loaded;
        }

        private async void SplashScreenWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                TxtStatus.Text = "Initializing Database & System Settings...";
                LoadingProgress.Value = 25;
                await Task.Delay(600);

                // Warm up EF Core DbContext & Ensure Schema matches
                await Task.Run(() =>
                {
                    using (var context = new POSDbContext())
                    {
                        try
                        {
                            context.Database.EnsureCreated();
                            _ = System.Linq.Enumerable.FirstOrDefault(context.Users);
                        }
                        catch
                        {
                            context.Database.EnsureDeleted();
                            context.Database.EnsureCreated();
                        }
                    }
                });

                LoadingProgress.Value = 65;
                TxtStatus.Text = "Loading Security & Branch Configurations...";
                await Task.Delay(600);

                LoadingProgress.Value = 95;
                TxtStatus.Text = "Starting POS Core Engine...";
                await Task.Delay(500);

                // Launch Login Window
                var loginWindow = new LoginWindow();
                loginWindow.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Startup Error: {ex.Message}", "POS Initialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }
    }
}
