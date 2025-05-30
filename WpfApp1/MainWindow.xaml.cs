using System.Windows;
using System.Windows.Controls;
using WpfApp1.Views;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private bool _isAdmin = false;

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new LoginPage(MainFrame));

        }
     
    }

}
