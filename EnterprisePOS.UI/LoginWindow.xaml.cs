using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EnterprisePOS.Core;
using EnterprisePOS.Core.Entities;
using EnterprisePOS.Data;
using EnterprisePOS.Services;
using EnterprisePOS.UI.Dialogs;

namespace EnterprisePOS.UI
{
    public partial class LoginWindow : Window
    {
        public User? AuthenticatedUser { get; private set; }
        private bool _isPasswordVisible = false;

        public LoginWindow()
        {
            InitializeComponent();
            LoadBranches();
        }

        private void LoadBranches()
        {
            try
            {
                using (var db = new POSDbContext())
                {
                    var branches = db.Branches.Where(b => b.IsActive).ToList();
                    CmbBranch.ItemsSource = branches;
                    if (branches.Any())
                        CmbBranch.SelectedIndex = 0;
                }
            }
            catch
            {
                // Fallback if db empty
            }
        }

        private void BtnTogglePassword_Click(object sender, RoutedEventArgs e)
        {
            if (_isPasswordVisible)
            {
                TxtPassword.Password = TxtVisiblePassword.Text;
                TxtVisiblePassword.Visibility = Visibility.Collapsed;
                TxtPassword.Visibility = Visibility.Visible;
                _isPasswordVisible = false;
            }
            else
            {
                TxtVisiblePassword.Text = TxtPassword.Password;
                TxtPassword.Visibility = Visibility.Collapsed;
                TxtVisiblePassword.Visibility = Visibility.Visible;
                _isPasswordVisible = true;
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            PerformLogin();
        }

        private void PerformLogin()
        {
            string username = TxtUsername.Text.Trim();
            string password = _isPasswordVisible ? TxtVisiblePassword.Text.Trim() : TxtPassword.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Please enter both username and password.");
                return;
            }

            try
            {
                using (var db = new POSDbContext())
                {
                    var auditService = new AuditService(db);
                    var user = db.Users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());

                    if (user == null)
                    {
                        ShowError("Invalid credentials. Please try again.");
                        auditService.LogActivity("SYSTEM", "LOGIN_FAILED", $"Invalid username attempt: '{username}'");
                        return;
                    }

                    if (user.IsLocked)
                    {
                        ShowError("🔒 Account locked due to 5 consecutive failed login attempts. Please contact Administrator.");
                        auditService.LogActivity(username, "LOGIN_BLOCKED", "Attempted login on locked account");
                        return;
                    }

                    bool isValidPassword = Core.SecurityHelper.VerifyPassword(password, user.PasswordHash) || user.PasswordHash == password;

                    if (!isValidPassword)
                    {
                        user.FailedLoginAttempts++;
                        if (user.FailedLoginAttempts >= 5)
                        {
                            user.IsLocked = true;
                            db.SaveChanges();
                            ShowError("🔒 Account locked! 5 failed login attempts reached.");
                            auditService.LogActivity(username, "ACCOUNT_LOCKED", "Account locked after 5 failed attempts");
                        }
                        else
                        {
                            db.SaveChanges();
                            ShowError($"Invalid password. Attempt {user.FailedLoginAttempts}/5.");
                            auditService.LogActivity(username, "LOGIN_FAILED", $"Failed password attempt ({user.FailedLoginAttempts}/5)");
                        }
                        return;
                    }

                    // Reset failed attempts on success
                    user.FailedLoginAttempts = 0;
                    db.SaveChanges();

                    auditService.LogActivity(user.Username, "LOGIN_SUCCESS", $"Successful login as {user.Role} at {CmbBranch.Text}");
                    AuthenticatedUser = user;

                    // If Cashier role, check/prompt Shift Opening
                    if (user.Role == UserRole.Cashier)
                    {
                        var shiftService = new ShiftService(db);
                        var activeShift = shiftService.GetActiveShift(user.Username);
                        if (activeShift == null)
                        {
                            var shiftWindow = new ShiftOpeningWindow(user);
                            bool? shiftStarted = shiftWindow.ShowDialog();
                            if (shiftStarted != true)
                            {
                                ShowError("Shift opening canceled. Login suspended.");
                                return;
                            }
                        }
                    }

                    var main = new MainWindow(user);
                    main.Show();
                    Close();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Database error: {ex.Message}");
            }
        }

        private void ShowError(string msg)
        {
            LblError.Text = msg;
            BorderError.Visibility = Visibility.Visible;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TxtForgotPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Password resets can only be performed by system Administrators.\nDefault Admin Credentials:\nUsername: admin\nPassword: admin123", "Forgot Password", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformLogin();
            }
        }
    }
}
