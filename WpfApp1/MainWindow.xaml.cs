using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Data;
using WpfApp1.Services;
using WpfApp1.Views;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var authService = App.ServiceProvider.GetRequiredService<AuthService>();
            MainFrame.Navigate(new LoginPage(MainFrame, authService));

        }
     
    }

}
