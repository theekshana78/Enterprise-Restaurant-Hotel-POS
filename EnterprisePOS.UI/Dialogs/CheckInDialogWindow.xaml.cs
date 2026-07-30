using System.Windows;
using EnterprisePOS.Core.Entities;

namespace EnterprisePOS.UI.Dialogs
{
    public partial class CheckInDialogWindow : Window
    {
        public string GuestName => TxtGuestName.Text.Trim();
        public string GuestPhone => TxtGuestPhone.Text.Trim();
        public string GuestNIC => TxtGuestNIC.Text.Trim();
        public bool IsConfirmed { get; private set; } = false;

        public CheckInDialogWindow(Room room)
        {
            InitializeComponent();
            TxtRoomHeader.Text = $"🔑 Check-In: {room.RoomNumber} ({room.RoomType} Room)";
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(GuestName))
            {
                MessageBox.Show("Please enter Guest Full Name.", "Check-In Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(GuestPhone))
            {
                MessageBox.Show("Please enter Guest Phone Number.", "Check-In Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
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
