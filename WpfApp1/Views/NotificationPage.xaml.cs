using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Data;
using WpfApp1.Model;

namespace WpfApp1.Views
{
    public partial class NotificationPage : Page, INotifyPropertyChanged
    {
        private Frame _mainFrame;
        private Notification _selectedNotification;
        private User _user;
        private readonly SyzyfContext _context;

        public ObservableCollection<Notification> Notifications { get; set; }

        public Notification SelectedNotification
        {
            get => _selectedNotification;
            set
            {
                if (_selectedNotification != value)
                {
                    _selectedNotification = value;
                    OnPropertyChanged();
                    if (_selectedNotification != null && !_selectedNotification.IsRead)
                    {
                        _ = MarkAsReadAsync(_selectedNotification);
                    }
                }
            }
        }

        public NotificationPage(Frame mainFrame, User user, SyzyfContext context)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _user = user;
            _context = context;

            Notifications = new ObservableCollection<Notification>();
            TopMenu.Initialize(_mainFrame, _user);
            DataContext = this;

            _ = LoadNotificationsAsync();
        }

        private async Task LoadNotificationsAsync()
        {
            try
            {
                long userId = _user.Id;
                bool isSupportUser = _user.EmployeeId != null &&
                                     _user.Employee?.Position?.PositionName == "Wsparcie";

                List<Notification> filteredNotifications;

                if (!isSupportUser)
                {
                    filteredNotifications = await _context.Notifications
                        .Where(n => n.ToId == userId)
                        .OrderByDescending(n => n.Id)
                        .ToListAsync();
                }
                else
                {
                    var allNotifications = await _context.Notifications
                        .OrderByDescending(n => n.Id)
                        .ToListAsync();

                    filteredNotifications = new List<Notification>();

                    foreach (var notif in allNotifications)
                    {
                        if (notif.ToId == userId)
                        {
                            filteredNotifications.Add(notif);
                            continue;
                        }

                        if (notif.Tag == "orderSigned" && notif.ProjectCardId == null)
                        {
                            filteredNotifications.Add(notif);
                        }
                    }

                }

                Notifications.Clear();
                foreach (var notif in filteredNotifications)
                {
                    Notifications.Add(notif);
                }

                OnPropertyChanged(nameof(Notifications));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania powiadomień: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private async Task MarkAsReadAsync(Notification notification)
        {
            try
            {
                if (!notification.IsRead)
                {
                    notification.IsRead = true;
                    _context.Notifications.Update(notification);
                    await _context.SaveChangesAsync();

                    OnPropertyChanged(nameof(Notifications));
                    OnPropertyChanged(nameof(SelectedNotification));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas oznaczania jako przeczytane: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender is Expander exp && exp.DataContext is Notification notif && !notif.IsRead)
            {
                await MarkAsReadAsync(notif);
            }
        }

        // NOWE METODY DLA OBSŁUGI ZLECEŃ

        private async void ViewOrder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Notification notif && notif.OrderId.HasValue)
            {
                try
                {
                    var order = await _context.Orders
                        .Include(o => o.Client)
                        .Include(o => o.Sales)
                        .FirstOrDefaultAsync(o => o.Id == notif.OrderId.Value);

                    if (order == null)
                    {
                        MessageBox.Show("Nie znaleziono zlecenia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var orderPage = new OrderFormPage(_mainFrame, _user, _context, order);
                    _mainFrame.Navigate(orderPage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Nie można otworzyć zlecenia: {ex.Message}",
                                   "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void SendProjectCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Notification notif && notif.OrderId.HasValue)
            {
                try
                {
                    var order = await _context.Orders
                        .Include(o => o.Client)
                        .ThenInclude(c => c.User)
                        .FirstOrDefaultAsync(o => o.Id == notif.OrderId.Value);

                    if (order == null || !order.IsSignedByClient)
                    {
                        MessageBox.Show("Zlecenie nie zostało jeszcze podpisane przez klienta.", 
                                       "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Sprawdź czy karta projektu już nie została wysłana
                    if (order.Status >= OrderStatus.ProjectCardSent)
                    {
                        MessageBox.Show("Karta projektu została już wysłana dla tego zlecenia.", 
                                       "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    // Aktualizuj status zlecenia
                    order.Status = OrderStatus.ProjectCardSent;
                    _context.Orders.Update(order);

                    // Wyślij powiadomienie do klienta o konieczności wypełnienia karty projektu
                    var clientNotification = new Notification
                    {
                        FromId = _user.Id,
                        ToId = order.Client.User.Id,
                        Title = "Karta projektu do wypełnienia",
                        Message = "Dziękujemy za podpisanie zlecenia. Prosimy o wypełnienie karty projektu z szczegółowymi wymaganiami stanowiska.",
                        Tag = "projectCardRequest",
                        OrderId = order.Id,
                        IsRead = false
                    };

                    _context.Notifications.Add(clientNotification);
                    await _context.SaveChangesAsync();

                    MessageBox.Show("Wysłano prośbę o wypełnienie karty projektu do klienta.", 
                                   "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                    await LoadNotificationsAsync(); // Odśwież powiadomienia
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas wysyłania karty projektu: {ex.Message}",
                                   "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void AssignRecruiter_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Notification notif && notif.ProjectCardId.HasValue)
            {
                try
                {
                    var projectCard = await _context.ProjectCards
                        .Include(pc => pc.Client)
                        .FirstOrDefaultAsync(pc => pc.Id == notif.ProjectCardId.Value);

                    if (projectCard == null)
                    {
                        MessageBox.Show("Nie znaleziono karty projektu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Otwórz okno wyboru rekrutera
                    var assignRecruiterPage = new AssignRecruiterPage(_mainFrame, _user, _context, projectCard);
                    _mainFrame.Navigate(assignRecruiterPage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas przydzielania rekrutera: {ex.Message}",
                                   "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // STARE METODY (zachowane)

        private void FillProjectCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Notification notif)
            {
                // Sprawdź czy to powiadomienie o konieczności wypełnienia karty projektu
                if (notif.Tag == "projectCardRequest" && notif.OrderId.HasValue)
                {
                    _mainFrame.Navigate(new ProjectCardFormPage(_mainFrame, _user, _context, notif));
                }
                else
                {
                    _mainFrame.Navigate(new ProjectCardFormPage(_mainFrame, _user, _context, notif));
                }
            }
        }

        private async void MarkAsRead_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Notification notif)
            {
                await MarkAsReadAsync(notif);
            }
        }

        private async void MarkAllAsRead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var unread = Notifications.Where(n => !n.IsRead).ToList();
                foreach (var n in unread)
                {
                    n.IsRead = true;
                    _context.Notifications.Update(n);
                }

                if (unread.Any())
                {
                    await _context.SaveChangesAsync();
                    OnPropertyChanged(nameof(Notifications));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd przy oznaczaniu wszystkich jako przeczytane: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void RefreshNotifications_Click(object sender, RoutedEventArgs e)
        {
            await LoadNotificationsAsync();
        }

        private async void seePreview_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Notification notif)
            {
                try
                {
                    var card = await _context.ProjectCards
                        .Include(pc => pc.Client)
                            .ThenInclude(c => c.User)
                        .FirstOrDefaultAsync(pc => pc.Id == notif.ProjectCardId.Value);

                    if (card == null)
                    {
                        MessageBox.Show("Nie znaleziono projektu o podanym Id.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var detailsPage = new ProjectDetailsPage(_mainFrame, _user, _context, card);
                    _mainFrame.Navigate(detailsPage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Nie można otworzyć szczegółów projektu: {ex.Message}",
                                   "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void seeProject_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Notification notif)
            {
                try
                {
                    var project = await _context.Projects.FindAsync(notif.ProjectId.Value);

                    if (project == null)
                    {
                        MessageBox.Show("Nie znaleziono projektu o podanym Id.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var detailsPage = new ProjectDetailsPage(_mainFrame, _user, _context, project);
                    _mainFrame.Navigate(detailsPage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Nie można otworzyć szczegółów projektu: {ex.Message}",
                                   "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}