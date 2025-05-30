using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Model;

namespace WpfApp1.Views
{
    public partial class NotificationPage : Page, INotifyPropertyChanged
    {
        private Frame _mainFrame;
        private Notification _selectedNotification;
        private int currentUserId = 5; // Przykładowy ID zalogowanego użytkownika

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

                    // Oznacz powiadomienie jako przeczytane po kliknięciu
                    if (_selectedNotification != null && !_selectedNotification.IsRead)
                    {
                        _selectedNotification.IsRead = true;
                        OnPropertyChanged(nameof(Notifications));
                        OnPropertyChanged(nameof(SelectedNotification));
                    }
                }
            }
        }

        public NotificationPage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            TopMenu.Initialize(_mainFrame, isAdmin: true);

            // Przykładowe powiadomienia
            var allNotifications = new List<Notification>
            {
                new Notification { Title = "Nowe zadanie", Message = "Masz nowe zadanie do wykonania.", IsRead = false, fromId=1, toId = new List<int>{5,7} },
                new Notification { Title = "Spotkanie", Message = "Przypomnienie o spotkaniu jutro o 10:00.", IsRead = true, fromId=2, toId = new List<int>{3,5} },
                new Notification { Title = "Aktualizacja", Message = "System został zaktualizowany.", IsRead = false, fromId=1, toId = new List<int>{4} }
            };

            Notifications = new ObservableCollection<Notification>(
                allNotifications.Where(n => n.toId.Contains(currentUserId))
            );

            DataContext = this;
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
