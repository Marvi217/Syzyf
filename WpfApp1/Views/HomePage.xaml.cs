using System;
using System.Windows.Controls;
using WpfApp1.Model;
using WpfApp1.Data; // Upewnij się, że masz using do SyzyfContext

namespace WpfApp1.Views
{
    public partial class HomePage : Page
    {
        private Frame _mainFrame;
        private User _user;
        private SyzyfContext _context;
        public HomePage(Frame mainFrame, User user, SyzyfContext context)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _user = user;
            _context = context;

            TopMenu.Initialize(_mainFrame, _user);
        }

        private void MainCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainCalendar.SelectedDate.HasValue)
            {
                DateTime selectedDate = MainCalendar.SelectedDate.Value;

                var meetingsPage = new MeetingPage(_mainFrame, _user, selectedDate, _context);
                _mainFrame.Navigate(meetingsPage);
            }
        }
    }
}
