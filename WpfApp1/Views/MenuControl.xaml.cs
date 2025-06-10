using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Model;
using WpfApp1.Data;

namespace WpfApp1.Views
{
    public partial class MenuControl : UserControl
    {
        private Frame _mainFrame;
        private User _user;
        private SyzyfContext _context;

        public MenuControl()
        {
            InitializeComponent();
        }

        public void Initialize(Frame mainFrame, User user)
        {
            _mainFrame = mainFrame;
            _user = user;
            _context = App.ServiceProvider.GetRequiredService<SyzyfContext>();

            bool isAdmin = _user?.Employee?.PositionId == 1;
            AddEmployeeButton.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame?.Navigate(new AddEmployeePage(_mainFrame, _user, _context));
        }

        private void ProjectsButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame?.Navigate(new ProjectPage(_mainFrame, _user, _context));
        }

        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame?.Navigate(new NotificationPage(_mainFrame, _user, _context));
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame?.Navigate(new AccountPage(_mainFrame, _user, _context));
        }

        private void Logo_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            _mainFrame?.Navigate(new HomePage(_mainFrame, _user, _context));
        }
    }
}
