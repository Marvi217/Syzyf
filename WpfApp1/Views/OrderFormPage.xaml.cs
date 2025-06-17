using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Data;
using WpfApp1.Model;
using WpfApp1.Models;

namespace WpfApp1.Views
{
    public partial class OrderFormPage : Page
    {
        private readonly Frame _mainFrame;
        private readonly User _user;
        private readonly SyzyfContext _context;
        private readonly Order _order;

        public OrderFormPage(Frame mainFrame, User user, SyzyfContext context, Order order)
        {
            InitializeComponent();

            _mainFrame = mainFrame;
            _user = user;
            _context = context;
            _order = order;

            TopMenu.Initialize(_mainFrame, _user);
            DisplayOrderContent();
        }

        private void DisplayOrderContent()
        {
            if (_order == null)
            {
                MessageBox.Show("Brak danych zlecenia do wyświetlenia.");
                return;
            }

            // Wyświetl treść zlecenia
            OrderContentTextBlock.Text = _order.OrderContent;
            
            // Pokaż przycisk "Podpisz" tylko dla klientów i tylko jeśli zlecenie nie jest podpisane
            if (_user.ClientId != null && !_order.IsSignedByClient)
            {
                SignButton.Visibility = Visibility.Visible;
            }
            else
            {
                SignButton.Visibility = Visibility.Collapsed;
                
                if (_order.IsSignedByClient)
                {
                    StatusTextBlock.Text = $"Zlecenie zostało podpisane w dniu: {_order.SignedDate?.ToString("dd.MM.yyyy HH:mm")}";
                    StatusTextBlock.Visibility = Visibility.Visible;
                }
            }
        }

        private async void SignOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_user.ClientId == null)
            {
                MessageBox.Show("Tylko klienci mogą podpisywać zlecenia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Czy na pewno chcesz podpisać to zlecenie? Tej akcji nie można cofnąć.",
                "Potwierdzenie podpisu",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                // Aktualizuj zlecenie
                _order.IsSignedByClient = true;
                _order.SignedDate = DateTime.Now;
                _order.Status = OrderStatus.Signed;

                _context.Orders.Update(_order);

                // Powiadom wsparcie o podpisaniu zlecenia
                var supportEmployees = await _context.Employees
                    .Include(e => e.User)
                    .Include(e => e.Position)
                    .Where(e => e.Position.PositionName == "Wsparcie")
                    .ToListAsync();

                foreach (var supportEmployee in supportEmployees)
                {
                    var notification = new Notification
                    {
                        FromId = _user.Id,
                        ToId = supportEmployee.User.Id,
                        Title = "Zlecenie podpisane",
                        Message = $"Klient {_user.Login} podpisał zlecenie. Wyślij kartę projektu do wypełnienia.",
                        Tag = "orderSigned",
                        OrderId = _order.Id,
                        IsRead = false,
                        CreatedAt = DateTime.Now
                    };

                    _context.Notifications.Add(notification);
                }

                await _context.SaveChangesAsync();

                MessageBox.Show("Zlecenie zostało pomyślnie podpisane. Wsparcie zostało powiadomione.", 
                               "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                // Odśwież widok
                DisplayOrderContent();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas podpisywania zlecenia: {ex.Message}", 
                               "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}