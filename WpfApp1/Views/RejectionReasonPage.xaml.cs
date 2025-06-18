using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Data;
using WpfApp1.Model;

namespace WpfApp1.Views
{
    public partial class RejectionReasonPage : Page
    {
        private readonly Frame _mainFrame;
        private readonly User _user;
        private readonly SyzyfContext _context;
        private readonly ProjectCard _projectCard;

        public RejectionReasonPage(Frame mainFrame, User user, SyzyfContext context, ProjectCard projectCard)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _user = user;
            _context = context;
            _projectCard = projectCard;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.GoBack();
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ReasonTextBox.Text))
            {
                MessageBox.Show("Pole z powodem odrzucenia nie może być puste.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SendButton.IsEnabled = false;

            try
            {
                var notification = new Notification
                {
                    FromId = _user.Id,
                    ToId = _projectCard.Client.User.Id,
                    Title = $"Karta projektu '{_projectCard.JobTitle}' wymaga poprawy",
                    Message = ReasonTextBox.Text.Trim(),
                    Tag = "rejection",
                    ProjectCardId = _projectCard.Id,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                };

                _context.Notifications.Add(notification);

                _projectCard.Status = ProjectCardStatus.Rejected;
                _context.ProjectCards.Update(_projectCard);

                await _context.SaveChangesAsync();

                MessageBox.Show("Wiadomość do klienta została wysłana.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                if (_mainFrame.CanGoBack) _mainFrame.GoBack();
                if (_mainFrame.CanGoBack) _mainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas wysyłania powiadomienia: {ex.Message}", "Błąd krytyczny", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                SendButton.IsEnabled = true;
            }
        }
    }
}