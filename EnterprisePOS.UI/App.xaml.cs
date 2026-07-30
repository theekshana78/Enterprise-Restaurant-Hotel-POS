using System.Windows;
using EnterprisePOS.Data;

namespace EnterprisePOS.UI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using (var db = new POSDbContext())
            {
                // Ensure SQLite Database & Tables are created automatically on first run
                db.Database.EnsureCreated();
            }
        }
    }
}
