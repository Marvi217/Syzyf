using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using WpfApp1.Data;
using WpfApp1.Model;
using WpfApp1.Services;

namespace WpfApp1.Views
{
    public partial class RegisterPage : Page
    {
        private readonly Frame _mainFrame;
        private readonly SyzyfContext _context;

        public RegisterPage(Frame mainFrame, SyzyfContext context)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _context = context;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var email = EmailTextBox.Text.Trim();
            var password = PasswordBox.Password;
            var confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageText.Text = "Wszystkie pola są wymagane.";
                return;
            }

            if (password != confirmPassword)
            {
                MessageText.Text = "Hasła nie są takie same.";
                return;
            }

            if (_context.Users.Any(u => u.Login == email))
            {
                MessageText.Text = "Użytkownik z takim e-mailem już istnieje.";
                return;
            }

            var user = new User
            {
                Login = email,
                Password = password
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            MessageBox.Show("Zarejestrowano pomyślnie.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            _mainFrame.Navigate(new LoginPage(_mainFrame, App.ServiceProvider.GetRequiredService<AuthService>()));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new LoginPage(_mainFrame, App.ServiceProvider.GetRequiredService<AuthService>()));
        }
    }
}
