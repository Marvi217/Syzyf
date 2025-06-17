using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Data;
using WpfApp1.Model;
using WpfApp1.Models;

namespace WpfApp1.Views
{
    public partial class CreateOrderPage : Page
    {
        private readonly Frame _mainFrame;
        private readonly User _user;
        private readonly SyzyfContext _context;
        private List<Client> _clients;

        public CreateOrderPage(Frame mainFrame, User user, SyzyfContext context)
        {
            InitializeComponent();

            _mainFrame = mainFrame;
            _user = user;
            _context = context;

            TopMenu.Initialize(_mainFrame, _user);

            this.Loaded += CreateOrderPage_Loaded;
        }

        private async void CreateOrderPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Sprawdź czy użytkownik to handlowiec
            if (_user.EmployeeId == null || _user.Employee?.Position?.PositionName != "Handlowiec")
            {
                MessageBox.Show("Tylko handlowcy mogą tworzyć zlecenia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                _mainFrame.GoBack();
                return;
            }

            await LoadClientsAsync();
            SetDefaultOrderContent();
        }

        private async Task LoadClientsAsync()
        {
            try
            {
                _clients = await _context.Clients
                    .Include(c => c.User)
                    .ToListAsync();

                ClientComboBox.ItemsSource = _clients;
                ClientComboBox.DisplayMemberPath = "Company";
                ClientComboBox.SelectedValuePath = "User.Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania klientów: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetDefaultOrderContent()
        {
            string defaultContent = @"ZLECENIE NA USŁUGI REKRUTACYJNE

Szanowny Kliencie,

Niniejszym przedstawiamy Państwu zlecenie na świadczenie usług rekrutacyjnych zgodnie z poniższymi warunkami:

1. PRZEDMIOT ZLECENIA
   - Przeprowadzenie procesu rekrutacyjnego zgodnie z wymaganiami określonymi w karcie projektu
   - Preselekcja kandydatów na podstawie profilu stanowiska
   - Przeprowadzenie wywiadów wstępnych
   - Przedstawienie najlepszych kandydatów do ostatecznej selekcji

2. ZAKRES USŁUG
   - Analiza potrzeb rekrutacyjnych
   - Opracowanie profilu kandydata
   - Publikacja ogłoszenia w odpowiednich kanałach
   - Wstępna selekcja aplikacji
   - Przeprowadzenie wywiadów rekrutacyjnych
   - Weryfikacja referencji
   - Przedstawienie rekomendacji

3. WARUNKI WSPÓŁPRACY
   - Czas realizacji: zgodnie z harmonogramem projektu
   - Forma płatności: według umowy ramowej
   - Gwarancja: 3 miesiące od daty zatrudnienia

4. DOKUMENTY WYMAGANE
   - Karta projektu z szczegółowym opisem stanowiska
   - Profil kandydata
   - Dodatkowe wymagania i preferencje

Podpisując niniejsze zlecenie, Klient wyraża zgodę na rozpoczęcie procesu rekrutacyjnego oraz zobowiązuje się do dostarczenia niezbędnych informacji w postaci karty projektu.

Data wystawienia: " + DateTime.Now.ToString("dd.MM.yyyy") + @"

Zlecenie wymaga podpisu elektronicznego Klienta dla aktywacji procesu rekrutacyjnego.";

            OrderContentTextBox.Text = defaultContent;
        }

        private async void SendOrder_Click(object sender, RoutedEventArgs e)
        {
            if (ClientComboBox.SelectedItem == null)
            {
                MessageBox.Show("Proszę wybrać klienta.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(OrderContentTextBox.Text))
            {
                MessageBox.Show("Proszę wprowadzić treść zlecenia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var selectedClient = (Client)ClientComboBox.SelectedItem;

                var order = new Order
                {
                    ClientId = selectedClient.Id,
                    SalesId = _user?.EmployeeId,
                    OrderContent = OrderContentTextBox.Text,
                    IsSignedByClient = false,
                    CreatedDate = DateTime.Now,
                    Status = OrderStatus.Sent
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Powiadom klienta o nowym zleceniu
                var notification = new Notification
                {
                    FromId = _user.Id,
                    ToId = selectedClient.User.Id,
                    Title = "Nowe zlecenie do podpisu",
                    Message = $"Handlowiec {_user.Employee.FirstName} {_user.Employee.LastName} przesłał Państwu zlecenie do podpisu. Proszę zapoznać się z treścią i podpisać zlecenie.",
                    Tag = "newOrder",
                    OrderId = order.Id,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                };

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                MessageBox.Show($"Zlecenie zostało wysłane do klienta: {selectedClient.Company}",
                               "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                _mainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas wysyłania zlecenia: {ex.Message}",
                               "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Czy na pewno chcesz anulować tworzenie zlecenia?",
                                       "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _mainFrame.GoBack();
            }
        }
    }
}