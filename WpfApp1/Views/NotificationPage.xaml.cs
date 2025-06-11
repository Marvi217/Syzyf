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
                bool isSupportUser = false;
                if (_user.EmployeeId != null)
                {
                    isSupportUser = _user.Employee.Position.PositionName == "Wsparcie";
                }


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

                        if (notif.Tag == "fulfilled" && notif.ProjectCardId != null)
                        {
                            var project = await _context.ProjectCards.FindAsync(notif.ProjectCardId.Value);
                            if (project == null)
                                continue;


                            var projectEmployee = await _context.ProjectEmployees
                                .FirstOrDefaultAsync(pe => pe.ProjectId == notif.ProjectCardId && pe.EmployeeId == _user.EmployeeId);

                            if (!project.IsAcceptedBySupport)
                            {
                                filteredNotifications.Add(notif);
                            }
                            else if (projectEmployee != null)
                            {
                                filteredNotifications.Add(notif);
                            }
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

        private void FillProjectCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Notification notif)
            {
                _mainFrame.Navigate(new ProjectCardFormPage(_mainFrame, _user, _context, notif));
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


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private async void seePreview_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Notification notif)
            {
                try
                {
                    var project = await _context.ProjectCards.FindAsync(notif.ProjectCardId.Value);

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

        private async void seeProject_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Notification notif)
            {
                try
                {
                    var project = await _context.Projects.FindAsync(notif.ProjectId);

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
    }
}
