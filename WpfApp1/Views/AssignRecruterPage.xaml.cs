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

            LoadRecruiters();
        }

        private async void LoadRecruiters()
        {
            try
            {
                // Załaduj listę rekruterów (np. pracownicy z pozycją "Handlowiec" lub inną)
                _recruiters = await _context.Employees
                    .Include(e => e.Position)
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
                    card.RecruiterId = selectedRecruiter.Id;

                    _context.ProjectCards.Update(card);
                    await _context.SaveChangesAsync();

                    MessageBox.Show($"Rekruter {selectedRecruiter.FirstName+ " " + selectedRecruiter.LastName} został przypisany do projektu.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

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
