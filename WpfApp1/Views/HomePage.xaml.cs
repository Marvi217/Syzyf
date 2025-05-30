using System.Windows;
using System.Windows.Controls;

namespace WpfApp1.Views
{
    public partial class HomePage : Page
    {
        private Frame _mainFrame;

        public HomePage(Frame mainFrame, bool isAdmin)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            TopMenu.Initialize(_mainFrame, isAdmin: true);
        }
    }
}
