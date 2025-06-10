using System.Windows.Controls;
using WpfApp1.Data;
using WpfApp1.Model;

namespace WpfApp1.Views
{
    public partial class ProjectCardFormPage : Page
    {
        private Frame _mainFrame;
        private User _user;
        private SyzyfContext _context;
        private Notification _notification;

        public ProjectCardFormPage(Frame mainFrame, User user, SyzyfContext context, Notification notification)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _user = user;
            _context = context;
            _notification = notification;

            // Można tu dodać logikę pobierania powiązanego projektu itd.
        }
    }
}
