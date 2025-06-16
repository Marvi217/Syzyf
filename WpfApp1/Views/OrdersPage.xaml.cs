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
using WpfApp1.Models;

namespace WpfApp1.Views
{
    public partial class OrdersPage : Page, INotifyPropertyChanged
    {
        private Frame _mainFrame;
        private User _user;
        private readonly SyzyfContext _context;
        private Order _selectedOrder;

        public ObservableCollection<Order> Orders { get; set; }
        public ObservableCollection<Order> FilteredOrders { get; set; }

        public Order SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (_selectedOrder != value)
                {
                    _selectedOrder = value;
                    OnPropertyChanged();
                }
            }
        }

        public OrdersPage(Frame mainFrame, User user, SyzyfContext context)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _user = user;
            _context = context;

            TopMenu.Initialize(_mainFrame, _user);

            Orders = new ObservableCollection<Order>();
            FilteredOrders = new ObservableCollection<Order>();

            this.Loaded += OrdersPage_Loaded;

            DataContext = this;
        }

        private async void OrdersPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Przycisk "Nowe zlecenie" widoczny tylko dla pracowników (nie klientów)
            if (_user.EmployeeId == null)
            {
                NewOrderButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                NewOrderButton.Visibility = Visibility.Visible;
            }

            await LoadOrdersAsync();
        }

        private async Task LoadOrdersAsync()
        {
            try
            {
                List<Order> orders;

                if (_user.EmployeeId != null)
                {
                    orders = await _context.Orders
                        .Include(o => o.Client)
                        .Include(o => o.Sales)
                        .Where(o => o.SalesId == _user.EmployeeId)
                        .OrderByDescending(o => o.CreatedDate)
                        .ToListAsync();
                }
                else if (_user.ClientId != null)
                {
                    orders = await _context.Orders
                        .Include(o => o.Client)
                        .Include(o => o.Sales)
                        .Where(o => o.ClientId == _user.ClientId)
                        .OrderByDescending(o => o.CreatedDate)
                        .ToListAsync();
                }
                else
                {
                    orders = new List<Order>();
                }

                Orders.Clear();
                foreach (var order in orders)
                {
                    Orders.Add(order);
                }

                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania zleceń: {ex.Message}",
                               "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyFilter()
        {
            FilteredOrders.Clear();

            // Pokazuj wszystkie zlecenia
            foreach (var order in Orders)
            {
                FilteredOrders.Add(order);
            }
        }

        private void FilterAll_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilter();
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
            var createOrderPage = new CreateOrderPage(_mainFrame, _user, _context);
            _mainFrame.Navigate(createOrderPage);
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender is Expander expander && expander.DataContext is Order order)
            {
                SelectedOrder = order;
            }
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Order order)
            {
                try
                {
                    var detailsPage = new OrderFormPage(_mainFrame, _user, _context, order);
                    _mainFrame.Navigate(detailsPage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Nie można otworzyć szczegółów zlecenia: {ex.Message}",
                                   "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}