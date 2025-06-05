using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
                        _selectedNotification.IsRead = true;
                        OnPropertyChanged(nameof(Notifications));
                        OnPropertyChanged(nameof(SelectedNotification));
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
            TopMenu.Initialize(_mainFrame, _user);
            LoadNotifications();
            DataContext = this;
        }
        private async void LoadNotifications()
        {
            long employeeId = _user.EmployeeId ?? 0;

            var notifications = await _context.Notifications.ToListAsync();

            var filtered = notifications.Where(n => n.ToId.Contains(employeeId)).ToList();

            Notifications = new ObservableCollection<Notification>(filtered);
            OnPropertyChanged(nameof(Notifications));
        }


        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender is Expander expander && expander.DataContext is Notification notif && !notif.IsRead)
            {
                notif.IsRead = true;
                OnPropertyChanged(nameof(Notifications));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
