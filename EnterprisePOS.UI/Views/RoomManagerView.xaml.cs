using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EnterprisePOS.Core;
using EnterprisePOS.Core.Entities;
using EnterprisePOS.Data;
using EnterprisePOS.Services;
using EnterprisePOS.UI.Dialogs;

namespace EnterprisePOS.UI.Views
{
    public partial class RoomManagerView : UserControl
    {
        private readonly POSDbContext _context;
        private readonly RoomService _roomService;

        public RoomManagerView()
        {
            InitializeComponent();
            _context = new POSDbContext();
            _roomService = new RoomService(_context);
            LoadRooms();
        }

        private void LoadRooms()
        {
            ItemsRoomGrid.ItemsSource = _roomService.GetAllRooms();
        }

        private void BtnCheckIn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Room room)
            {
                if (room.Status != RoomStatus.Available)
                {
                    MessageBox.Show($"{room.RoomNumber} is currently {room.Status} and cannot be checked into.", "Check-In Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var dlg = new CheckInDialogWindow(room)
                {
                    Owner = Window.GetWindow(this)
                };
                dlg.ShowDialog();

                if (dlg.IsConfirmed)
                {
                    if (_roomService.CheckInGuest(room.Id, dlg.GuestName, dlg.GuestPhone))
                    {
                        MessageBox.Show($"{room.RoomNumber} checked in successfully for {dlg.GuestName}!", "Check-In Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadRooms();
                    }
                }
            }
        }

        private void BtnCheckOut_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Room room)
            {
                if (room.Status != RoomStatus.Occupied)
                {
                    MessageBox.Show($"{room.RoomNumber} is not occupied.", "Check-Out Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                decimal total = _roomService.CalculateFinalRoomBill(room.Id, out int nights);
                var payDlg = new PaymentDialogWindow(total)
                {
                    Owner = Window.GetWindow(this)
                };
                payDlg.ShowDialog();

                if (payDlg.IsConfirmed)
                {
                    if (_roomService.CheckOutGuest(room.Id, PaymentMethod.Cash, "Cashier", out Invoice? inv))
                    {
                        if (inv != null)
                        {
                            var rpt = new ReceiptWindow(inv, payDlg.CashTendered)
                            {
                                Owner = Window.GetWindow(this)
                            };
                            rpt.ShowDialog();
                        }
                        LoadRooms();
                    }
                }
            }
        }
    }
}
