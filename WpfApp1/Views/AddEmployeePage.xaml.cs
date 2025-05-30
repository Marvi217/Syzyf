using System.Windows.Controls;

namespace WpfApp1.Views
{
    public partial class AddEmployeePage : Page
    {
        private Frame _mainFrame;

        public AddEmployeePage(Frame mainFrame)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
        }
    }
}
