using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Data;
using WpfApp1.Model;
using WpfApp1.Models;

namespace WpfApp1.Views
{
    public partial class ProjectPage : Page, INotifyPropertyChanged
    {
        private Frame _mainFrame;
        private User _user;
        private readonly SyzyfContext _context;
        private Project _selectedProject;
        private string _currentFilter = "My";

        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Project> FilteredProjects { get; set; }
        public ObservableCollection<ProjectCard> ProjectCards { get; set; }
        public ObservableCollection<ProjectCard> FilteredProjectCards { get; set; } = new();


        public Project SelectedProject
        {
            get => _selectedProject;
            set
            {
                if (_selectedProject != value)
                {
                    _selectedProject = value;
                    OnPropertyChanged();
                }
            }
        }

        public ProjectPage(Frame mainFrame, User user, SyzyfContext context)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _user = user;
            _context = context;

            TopMenu.Initialize(_mainFrame, _user);

            Projects = new ObservableCollection<Project>();
            FilteredProjects = new ObservableCollection<Project>();
            ProjectCards = new ObservableCollection<ProjectCard>();

            this.Loaded += ProjectPage_Loaded;
            DataContext = this;
        }

        private async Task CheckClientAcceptanceDelaysAsync()
        {
            try
            {
                var cards = await _context.ProjectCards
                    .Include(pc => pc.ProjectAcceptance)
                    .Include(pc => pc.Client)
                    .Include(pc => pc.Recruiter)
                    .Include(pc => pc.Notifications)
                    .Where(pc => pc.Status == ProjectCardStatus.Processed)
                    .ToListAsync();

                foreach (var card in cards)
                {
                    var acceptance = card.ProjectAcceptance;
                    var latestNotification = card.Notifications
                        .Where(n => n.Tag == "ProjectSubmission")
                        .OrderByDescending(n => n.CreatedAt)
                        .FirstOrDefault();

                    if (acceptance == null || latestNotification == null)
                        continue;

                    if (acceptance.AcceptedByClient)
                        continue;

                    var daysPassed = (DateTime.Now - latestNotification.CreatedAt).TotalDays;

                    // 3 dni – przypomnienie klientowi
                    if (daysPassed > 3 && !card.Notifications.Any(n => n.Tag == "Reminder3"))
                    {
                        var reminderToClient = new Notification
                        {
                            Title = "Przypomnienie o akceptacji projektu",
                            Message = "Prosimy o zaakceptowanie projektu.",
                            Tag = "Reminder3",
                            ProjectCardId = card.Id,
                            FromId = _user.Id,
                            ToId = card.Client.User.Id,
                            IsRead = false,
                            CreatedAt = DateTime.Now
                        };
                        _context.Notifications.Add(reminderToClient);
                    }

                    // 6 dni – powiadomienie wsparcia
                    if (daysPassed > 6 && !card.Notifications.Any(n => n.Tag == "Reminder6"))
                    {
                        var support = card.ProjectAcceptance?.SupportId;
                        if (support != null)
                        {
                            var reminderToSupport = new Notification
                            {
                                Title = "Brak akceptacji projektu przez klienta",
                                Message = $"Skontaktuj się z klientem w sprawie akceptacji projektu: {card.Id}.",
                                Tag = "Reminder6",
                                ProjectCardId = card.Id,
                                FromId = _user.Id,
                                ToId = support.Value,
                                IsRead = false,
                                CreatedAt = DateTime.Now
                            };
                            _context.Notifications.Add(reminderToSupport);
                        }
                    }

                    // 15 dni – anuluj kartę
                    if (daysPassed > 15)
                    {
                        card.Status = ProjectCardStatus.Canceled;
                    }
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //Nic nie rób, błędy są ignorowane, ponieważ to tylko przypomnienia
            }
        }

        private async void ProjectPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Pokaż przycisk "Nowy projekt" tylko dla użytkowników Wsparcia
                if (_user.EmployeeId == null)
                {
                    // Brak pracownika - ukryj przycisk
                    NewProjectButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    // Sprawdź czy Employee i Position nie są null
                    if (_user.Employee != null && _user.Employee.Position != null)
                    {
                        if (_user.Employee.Position.PositionName == "Wsparcie")
                        {
                            // Wsparcie - pokaż przycisk
                            NewProjectButton.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            // Inne pozycje (w tym handlowcy) - ukryj przycisk
                            NewProjectButton.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        // Brak danych o pozycji - ukryj przycisk
                        NewProjectButton.Visibility = Visibility.Collapsed;
                    }
                }

                await LoadProjectsAsync();
                await LoadProjectCardsAsync();
                await CheckClientAcceptanceDelaysAsync();
                ApplyFilter(); // Zastosuj domyślny filtr po załadowaniu danych
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania strony: {ex.Message}",
                              "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadProjectCardsAsync()
        {
            try
            {
                List<ProjectCard> cards;

                if (_user.Employee != null && _user.Employee.Position != null &&
                    _user.Employee.Position.PositionName == "Wsparcie")
                {
                    // Wsparcie widzi wszystkie karty
                    cards = await _context.ProjectCards
                        .Include(pc => pc.Client)
                        .Include(pc => pc.Recruiter)
                        .ToListAsync();
                }
                else if (_user.EmployeeId != null)
                {
                    // Rekruter widzi tylko swoje karty
                    cards = await _context.ProjectCards
                        .Include(pc => pc.Client)
                        .Include(pc => pc.Recruiter)
                        .Where(pc => pc.RecruiterId == _user.EmployeeId.Value)
                        .ToListAsync();
                }
                else if (_user.ClientId != null)
                {
                    cards = await _context.ProjectCards
                        .Include(pc => pc.Client)
                        .Include(pc => pc.Recruiter)
                        .Where(pc => pc.ClientId == _user.ClientId.Value)
                        .ToListAsync();
                }
                else
                {
                    // Pozostali nie widzą kart
                    cards = new List<ProjectCard>();
                }

                ProjectCards.Clear();
                foreach (var card in cards)
                    ProjectCards.Add(card);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania kart projektów: {ex.Message}",
                                "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadProjectsAsync()
        {
            try
            {
                List<Project> projects = new List<Project>();

                if (_user.EmployeeId != null)
                {
                    if (_user.Employee?.Position?.PositionName == "Rekruter")
                    {
                        projects = await _context.Projects
                            .Include(p => p.Client)
                            .Include(p => p.ProjectCard)
                            .Where(p => p.ProjectCard.RecruiterId == _user.EmployeeId.Value)
                            .ToListAsync();
                    }
                    else if (_user.Employee?.Position?.PositionName == "Wsparcie")
                    {
                        projects = await _context.Projects
                            .Include(p => p.Client)
                            .Include(p => p.ProjectCard)
                            .ToListAsync();
                    }
                }
                else if (_user.ClientId != null)
                {
                    projects = await _context.Projects
                        .Include(p => p.Client)
                        .Include(p => p.ProjectCard)
                        .Where(p => p.ClientId == _user.ClientId.Value)
                        .ToListAsync();
                }

                Projects.Clear();
                foreach (var project in projects)
                    Projects.Add(project);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania projektów: {ex.Message}",
                                "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void ApplyFilter()
        {
            try
            {
                FilteredProjects.Clear();
                FilteredProjectCards?.Clear();

                if (_currentFilter == "Planned")
                {
                    ProjectsListView.Visibility = Visibility.Collapsed;
                    ProjectCardsListView.Visibility = Visibility.Visible;

                    foreach (var card in ProjectCards)
                        FilteredProjectCards.Add(card);
                }
                else
                {
                    ProjectsListView.Visibility = Visibility.Visible;
                    ProjectCardsListView.Visibility = Visibility.Collapsed;

                    IEnumerable<Project> filtered = Projects;

                    switch (_currentFilter)
                    {
                        case "Active":
                            filtered = Projects.Where(p => p.Status == ProjectStatus.InProgress);
                            break;
                        case "Completed":
                            filtered = Projects.Where(p => p.Status == ProjectStatus.Completed);
                            break;
                        case "My":
                        default:
                            if (_user.EmployeeId != null)
                                filtered = Projects;
                            else if (_user.ClientId != null)
                                filtered = Projects.Where(p => p.ClientId == _user.ClientId.Value);
                            break;
                    }

                    foreach (var project in filtered)
                        FilteredProjects.Add(project);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas filtrowania: {ex.Message}",
                                "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void FilterPlanned_Click(object sender, RoutedEventArgs e)
        {
            _currentFilter = "Planned";
            ApplyFilter();
        }

        private void FilterActive_Click(object sender, RoutedEventArgs e)
        {
            _currentFilter = "Active";
            ApplyFilter();
        }

        private void FilterCompleted_Click(object sender, RoutedEventArgs e)
        {
            _currentFilter = "Completed";
            ApplyFilter();
        }

        private void FilterMy_Click(object sender, RoutedEventArgs e)
        {
            _currentFilter = "My";
            ApplyFilter();
        }


        private void AddProject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var createProjectPage = new CreateProjectPage(_mainFrame, _user, _context);
                _mainFrame.Navigate(createProjectPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nie można otworzyć strony tworzenia projektu: {ex.Message}",
                              "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender is Expander expander && expander.DataContext is Project project)
            {
                SelectedProject = project;
            }
        }
        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Project project)
            {
                try
                {
                    var detailsPage = new ProjectDetailsPage(_mainFrame, _user, _context, project);
                    _mainFrame.Navigate(detailsPage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Nie można otworzyć szczegółów projektu: {ex.Message}",
                                   "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void ViewCardDetails_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is ProjectCard project)
            {
                try
                {
                    var detailsPage = new ProjectDetailsPage(_mainFrame, _user, _context, project);
                    _mainFrame.Navigate(detailsPage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Nie można otworzyć szczegółów projektu: {ex.Message}",
                                   "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}