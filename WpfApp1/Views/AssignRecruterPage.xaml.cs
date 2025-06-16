using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Data;
using WpfApp1.Model;
using WpfApp1.Models;

namespace WpfApp1.Views
{
    public partial class AssignRecruiterPage : Page
    {
        private readonly Frame _mainFrame;
        private readonly User _user;
        private readonly SyzyfContext _context;
        private readonly IProjectBase _projectCard;
        private System.Collections.Generic.List<Employee> _recruiters;

        public AssignRecruiterPage(Frame mainFrame, User user, SyzyfContext context, IProjectBase projectCard)
        {
            InitializeComponent();

            _mainFrame = mainFrame;
            _user = user;
            _context = context;
            _projectCard = projectCard;
            TopMenu.Initialize(_mainFrame, _user);
            LoadRecruiters();
        }

        private async void LoadRecruiters()
        {
            try
            {
                // Załaduj listę rekruterów (np. pracownicy z pozycją "Handlowiec" lub inną)
                _recruiters = await _context.Employees
                    .Include(e => e.Position)
                    .Include(e => e.User)
                    .Where(e => e.Position.PositionName == "Rekruter")
                    .ToListAsync();

                RecruiterComboBox.ItemsSource = _recruiters;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania rekruterów: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AssignButton_Click(object sender, RoutedEventArgs e)
        {
            if (RecruiterComboBox.SelectedItem is not Employee selectedRecruiter)
            {
                MessageBox.Show("Proszę wybrać rekrutera.", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Przypisz wybranego rekrutera do projektu (_projectCard)
                if (_projectCard is ProjectCard card)
                {
                    // Przykład: po zaakceptowaniu karty, wysyłasz powiadomienie do rekrutera:
                    var message = $"Dzień dobry, jestem {_user.Employee.FirstName} {_user.Employee.LastName}. Chciałbym Ci przydzielić tę kartę projektu: {card.JobTitle}. Proszę o podgląd, przyjęcie lub odrzucenie.";
                    var notification = new Notification
                    {
                        FromId = _user.Id,
                        ToId = selectedRecruiter.User.Id,
                        Title = "Przydzielenie karty projektu",
                        Message = message,
                        Tag = "projectAssignmentRequest",
                        ProjectCardId = card.Id,
                        IsRead = false
                    };
                    _context.Notifications.Add(notification);
                    await _context.SaveChangesAsync();

                    // Opcjonalnie wróć do poprzedniej strony
                    _mainFrame.GoBack();
                }
                else
                {
                    MessageBox.Show("Niepoprawny typ karty projektu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas przypisywania rekrutera: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
