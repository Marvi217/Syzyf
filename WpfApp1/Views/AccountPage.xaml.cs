using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.Data;
using WpfApp1.Model;
using WpfApp1.Services;

namespace WpfApp1.Views
{
    public partial class AccountPage : Page
    {
        private readonly Frame _mainFrame;
        private readonly User _user;
        private readonly SyzyfContext _context;

        public AccountPage(Frame mainFrame, User user, SyzyfContext context)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _user = user;
            _context = context;

            ShowProfile();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e) => ShowProfile();
        private void AccountButton_Click(object sender, RoutedEventArgs e) => ShowAccount();
        private void HelpButton_Click(object sender, RoutedEventArgs e) => ShowHelp();
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var authService = App.ServiceProvider.GetService(typeof(AuthService)) as AuthService;
            _mainFrame.Navigate(new LoginPage(_mainFrame, authService));
        }

        private void ShowProfile()
        {
            var employee = _user.Employee;

            var content = new StackPanel { Margin = new Thickness(10) };
            content.Children.Add(new TextBlock { Text = $"Imię: {employee?.FirstName}", FontSize = 16 });
            content.Children.Add(new TextBlock { Text = $"Nazwisko: {employee?.LastName}", FontSize = 16 });
            content.Children.Add(new TextBlock { Text = $"Telefon: {employee?.PhoneNumber}", FontSize = 16 });
            content.Children.Add(new TextBlock { Text = $"Email: {employee?.Email}", FontSize = 16 });

            ContentArea.Content = content;
        }

        private void ShowAccount()
        {
            var panel = new StackPanel { Margin = new Thickness(10) };

            panel.Children.Add(new TextBlock { Text = $"Login: {_user.Login}", FontSize = 16 });
            panel.Children.Add(new TextBlock { Text = $"Hasło: {"********"}", FontSize = 16, Margin = new Thickness(0, 5, 0, 10) });

            var changePassButton = new Button { Content = "Zmień hasło" };
            panel.Children.Add(changePassButton);

            changePassButton.Click += (s, e) =>
            {
                // Usuń poprzednią zawartość z formularzem zmiany hasła (jeśli jest)
                if (panel.Children.Count > 3)
                    panel.Children.RemoveAt(3);

                var passwordPanel = new StackPanel { Margin = new Thickness(0, 10, 0, 0) };

                var newPassLabel = new TextBlock { Text = "Nowe hasło:" };
                var newPassBox = new PasswordBox();

                var confirmPassLabel = new TextBlock { Text = "Potwierdź hasło:", Margin = new Thickness(0, 10, 0, 0) };
                var confirmPassBox = new PasswordBox();

                var saveButton = new Button { Content = "Zapisz nowe hasło", Margin = new Thickness(0, 10, 0, 0) };

                saveButton.Click += (sender2, e2) =>
                {
                    var newPass = newPassBox.Password;
                    var confirmPass = confirmPassBox.Password;

                    if (string.IsNullOrWhiteSpace(newPass) || string.IsNullOrWhiteSpace(confirmPass))
                    {
                        MessageBox.Show("Hasło nie może być puste.");
                    }
                    else if (newPass != confirmPass)
                    {
                        MessageBox.Show("Hasła nie są identyczne.");
                    }
                    else
                    {
                        _user.Password = newPass; // UWAGA: w rzeczywistej aplikacji hasło powinno być haszowane!
                        _context.SaveChanges();
                        MessageBox.Show("Hasło zostało zmienione.");
                        ShowAccount(); // odśwież widok
                    }
                };

                passwordPanel.Children.Add(newPassLabel);
                passwordPanel.Children.Add(newPassBox);
                passwordPanel.Children.Add(confirmPassLabel);
                passwordPanel.Children.Add(confirmPassBox);
                passwordPanel.Children.Add(saveButton);

                panel.Children.Add(passwordPanel);
            };

            ContentArea.Content = panel;
        }



        private void ShowHelp()
        {
            var panel = new StackPanel { Margin = new Thickness(10) };

            var titleBox = new TextBox { Text = "Tytuł", Foreground = Brushes.Gray };
            titleBox.GotFocus += (s, e) =>
            {
                if (titleBox.Text == "Tytuł") { titleBox.Text = ""; titleBox.Foreground = Brushes.Black; }
            };
            titleBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(titleBox.Text)) { titleBox.Text = "Tytuł"; titleBox.Foreground = Brushes.Gray; }
            };

            var descBox = new TextBox
            {
                Text = "Opis",
                Height = 100,
                AcceptsReturn = true,
                Foreground = Brushes.Gray
            };
            descBox.GotFocus += (s, e) =>
            {
                if (descBox.Text == "Opis") { descBox.Text = ""; descBox.Foreground = Brushes.Black; }
            };
            descBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(descBox.Text)) { descBox.Text = "Opis"; descBox.Foreground = Brushes.Gray; }
            };

            var sendButton = new Button { Content = "Wyślij", Margin = new Thickness(0, 10, 0, 0) };
            sendButton.Click += (s, e) =>
            {
                MessageBox.Show("Wysłano zgłoszenie do administratora.", "Pomoc");
            };

            panel.Children.Add(titleBox);
            panel.Children.Add(descBox);
            panel.Children.Add(sendButton);

            ContentArea.Content = panel;
        }
    }

}
