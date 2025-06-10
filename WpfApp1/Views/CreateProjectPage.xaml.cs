using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Data;
using WpfApp1.Model;
using WpfApp1.Models;

namespace WpfApp1.Views
{
    public partial class CreateProjectPage : Page, INotifyPropertyChanged
    {
        private Frame _mainFrame;
        private User _user;
        private readonly SyzyfContext _context;
        private string _login;

        public string Login
        {
            get => _login;
            set
            {
                if (_login != value)
                {
                    _login = value;
                    OnPropertyChanged();
                }
            }
        }

        public CreateProjectPage(Frame mainFrame, User user, SyzyfContext context)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _user = user;
            _context = context;

            TopMenu.Initialize(_mainFrame, _user);
            DataContext = this;
        }

        private async void SendProjectCard_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Login))
            {
                MessageBox.Show("Proszę wprowadzić login użytkownika.",
                                "Brak danych", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var recipient = await _context.Users.FirstOrDefaultAsync(u => u.Login == Login);
                if (recipient == null)
                {
                    MessageBox.Show("Nie znaleziono użytkownika o podanym loginie.",
                                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (recipient.Id == _user.Id)
                {
                    MessageBox.Show("Nie można wysłać karty projektu do samego siebie.",
                                    "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string message = GenerateProjectCardMessage();

                var notification = new Notification
                {
                    FromId = _user.Id,
                    ToId = new List<long> { recipient.Id },
                    Title = "Karta Projektu",
                    Tag = "empty",
                    Message = message,
                    IsRead = false
                };

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                MessageBox.Show($"Komunikat został wysłany do użytkownika: {Login}\n\n" +
                                $"Treść wiadomości:\n{message}",
                                "Karta projektu wysłana", MessageBoxButton.OK, MessageBoxImage.Information);

                _mainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas wysyłania karty projektu: {ex.Message}",
                                "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private string GenerateProjectCardMessage()
        {
            string senderName = $"{_user.Employee?.FirstName} {_user.Employee?.LastName}".Trim();

            return $@"Dzień dobry,

                Jestem {senderName} z firmy Syzyf. Zwracam się do Państwa z prośbą o wypełnienie karty projektu, która pozwoli nam lepiej zrozumieć Państwa potrzeby i oczekiwania.

                Karta projektu zawiera kluczowe informacje niezbędne do prawidłowego zaplanowania i realizacji współpracy. Prosimy o szczegółowe wypełnienie wszystkich sekcji, co umożliwi nam przygotowanie oferty maksymalnie dopasowanej do Państwa wymagań.

                W przypadku pytań lub wątpliwości dotyczących wypełniania karty, proszę o kontakt pod tym adresem. Jesteśmy do Państwa dyspozycji i chętnie udzielimy wszelkich wyjaśnień.

                Dziękujemy za zainteresowanie naszymi usługami i wyrażamy nadzieję na owocną współpracę.

                Z poważaniem,
                {senderName}
                Firma Syzyf

                ---
                Karta projektu zostanie dostarczona w osobnym komunikacie.";
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.GoBack();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}