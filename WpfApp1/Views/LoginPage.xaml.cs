using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.Data;
using WpfApp1.Model;
using WpfApp1.Services;

namespace WpfApp1.Views
{
    public partial class LoginPage : Page
    {
        private readonly Frame _mainFrame;
        private readonly AuthService _authService;

        public LoginPage(Frame mainFrame, AuthService authService)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _authService = authService;
        }

        private void LoginTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "Login")
            {
                LoginTextBox.Text = "";
                LoginTextBox.Foreground = Brushes.Black;
            }
        }

        private void LoginTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text))
            {
                LoginTextBox.Text = "Login";
                LoginTextBox.Foreground = Brushes.Gray;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordPlaceholder.Visibility = string.IsNullOrEmpty(PasswordBox.Password)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginTextBox.Text.Trim();
            var password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) || login == "Login")
            {
                ErrorMessage.Text = "Wprowadź login i hasło.";
                return;
            }

            var user = _authService.Authenticate(login, password);

            if (user == null)
            {
                ErrorMessage.Foreground = Brushes.Red;
                ErrorMessage.Text = "Nieprawidłowy login lub hasło.";
                return;
            }

            ErrorMessage.Foreground = Brushes.Green;
            ErrorMessage.Text = "Zalogowano pomyślnie.";
            var context = App.ServiceProvider.GetRequiredService<SyzyfContext>();
            _mainFrame.Navigate(new HomePage(_mainFrame, user, context));
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var context = App.ServiceProvider.GetRequiredService<SyzyfContext>();
            _mainFrame.Navigate(new RegisterPage(_mainFrame, context));
        }
    }
}
