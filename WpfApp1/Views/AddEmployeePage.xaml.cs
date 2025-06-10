using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Data;
using WpfApp1.Model;
using WpfApp1.Models;
using Microsoft.EntityFrameworkCore;

namespace WpfApp1.Views
{
    public partial class AddEmployeePage : Page
    {
        private readonly Frame _mainFrame;
        private readonly SyzyfContext _context;
        private readonly User _user;

        public AddEmployeePage(Frame mainFrame, User user, SyzyfContext context)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _user = user;
            _context = context;
            TopMenu.Initialize(_mainFrame, _user);
            LoadPositions();
        }

        private async void LoadPositions()
        {
            try
            {
                var positions = await _context.Positions.ToListAsync();

                foreach (var pos in positions)
                {
                    System.Diagnostics.Debug.WriteLine($"ID: {pos.Id}, Nazwa: {pos.PositionName}");
                }

                PositionComboBox.ItemsSource = positions;

                if (positions.Any())
                {
                    PositionComboBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBlock.Foreground = System.Windows.Media.Brushes.Red;
                MessageBlock.Text = "Błąd ładowania pozycji: " + ex.Message;
                System.Diagnostics.Debug.WriteLine($"Błąd ładowania pozycji: {ex}");
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Walidacja danych
                if (string.IsNullOrWhiteSpace(FirstNameTextBox.Text))
                {
                    MessageBlock.Foreground = System.Windows.Media.Brushes.Red;
                    MessageBlock.Text = "Imię jest wymagane!";
                    return;
                }

                if (string.IsNullOrWhiteSpace(LastNameTextBox.Text))
                {
                    MessageBlock.Foreground = System.Windows.Media.Brushes.Red;
                    MessageBlock.Text = "Nazwisko jest wymagane!";
                    return;
                }

                if (string.IsNullOrWhiteSpace(LoginTextBox.Text))
                {
                    MessageBlock.Foreground = System.Windows.Media.Brushes.Red;
                    MessageBlock.Text = "Login jest wymagany!";
                    return;
                }

                if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                {
                    MessageBlock.Foreground = System.Windows.Media.Brushes.Red;
                    MessageBlock.Text = "Hasło jest wymagane!";
                    return;
                }

                if (PositionComboBox.SelectedValue == null)
                {
                    MessageBlock.Foreground = System.Windows.Media.Brushes.Red;
                    MessageBlock.Text = "Wybierz stanowisko!";
                    return;
                }

                // Sprawdź czy login już istnieje
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == LoginTextBox.Text);
                if (existingUser != null)
                {
                    MessageBlock.Foreground = System.Windows.Media.Brushes.Red;
                    MessageBlock.Text = "Login już istnieje!";
                    return;
                }

                // Utwórz pracownika
                var employee = new Employee
                {
                    FirstName = FirstNameTextBox.Text.Trim(),
                    LastName = LastNameTextBox.Text.Trim(),
                    Email = string.IsNullOrWhiteSpace(EmailTextBox.Text) ? null : EmailTextBox.Text.Trim(),
                    PhoneNumber = string.IsNullOrWhiteSpace(PhoneTextBox.Text) ? null : PhoneTextBox.Text.Trim(),
                    WorkSince = DateTime.Now.Date,
                    PositionId = (long)PositionComboBox.SelectedValue
                };

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync(); // Potrzebne by wygenerować ID

                // Utwórz użytkownika
                var user = new User
                {
                    Login = LoginTextBox.Text.Trim(),
                    Password = PasswordBox.Password, // W prawdziwej aplikacji powinieneś zahashować hasło
                    EmployeeId = employee.Id
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Sukces
                MessageBlock.Foreground = System.Windows.Media.Brushes.Green;
                MessageBlock.Text = "Pracownik został dodany pomyślnie!";

                // Wyczyść formularz
                ClearForm();

            }
            catch (Exception ex)
            {
                MessageBlock.Foreground = System.Windows.Media.Brushes.Red;
                MessageBlock.Text = "Błąd: " + ex.Message;
                System.Diagnostics.Debug.WriteLine($"Błąd zapisywania: {ex}");
            }
        }

        private void ClearForm()
        {
            FirstNameTextBox.Text = "";
            LastNameTextBox.Text = "";
            EmailTextBox.Text = "";
            PhoneTextBox.Text = "";
            LoginTextBox.Text = "";
            PasswordBox.Password = "";
            PositionComboBox.SelectedIndex = -1;
        }
    }
}