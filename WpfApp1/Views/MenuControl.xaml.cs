using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Views
{
    public partial class MenuControl : UserControl
    {
        private Frame _mainFrame;
        private bool isAdmin; // Zmienna do przechowywania informacji o uprawnieniach administratora

        public MenuControl()
        {
            InitializeComponent();
        }

        // Metoda do ustawienia Frame i widoczności przycisku
        public void Initialize(Frame mainFrame, bool isAdmin)
        {
            _mainFrame = mainFrame;
            this.isAdmin = isAdmin; // Przechowujemy informację o uprawnieniach administratora
            AddEmployeeButton.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame?.Navigate(new AddEmployeePage(_mainFrame));
        }

        private void ProjectsButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Projekty kliknięte");
            // lub _mainFrame.Navigate(...) jeśli masz stronę Projektów
        }

        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame?.Navigate(new NotificationPage(_mainFrame));
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Konto kliknięte");
            // lub _mainFrame.Navigate(...) jeśli masz stronę Konta
        }
        private void Logo_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            _mainFrame?.Navigate(new HomePage(_mainFrame, isAdmin));
        }

    }
}
