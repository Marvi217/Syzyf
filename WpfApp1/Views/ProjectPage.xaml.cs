using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private string _currentFilter = "All";

        public ObservableCollection<Project> Projects { get; set; }
        public ObservableCollection<Project> FilteredProjects { get; set; }

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

            LoadProjects();
            DataContext = this;
        }

        private async void LoadProjects()
        {
            try
            {
                var projects = await _context.Projects
                    .Include(p => p.Client)
                    .Include(p => p.ProjectEmployees)
                        .ThenInclude(pe => pe.Employee)
                    .ToListAsync();

                Projects.Clear();
                foreach (var project in projects)
                {
                    Projects.Add(project);
                }

                ApplyFilter();
                OnPropertyChanged(nameof(Projects));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas ładowania projektów: {ex.Message}",
                               "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyFilter()
        {
            FilteredProjects.Clear();
            IEnumerable<Project> filtered = Projects;

            switch (_currentFilter)
            {
                case "Planned":
                    filtered = Projects.Where(p => p.Status == ProjectStatus.Planned);
                    break;
                case "Active":
                    filtered = Projects.Where(p => p.Status == ProjectStatus.InProgress);
                    break;
                case "Completed":
                    filtered = Projects.Where(p => p.Status == ProjectStatus.Completed);
                    break;
                case "My":
                    long employeeId = _user.EmployeeId ?? 0;
                    filtered = Projects.Where(p => p.ProjectEmployees.Any(pe => pe.EmployeeId == employeeId));
                    break;
                case "All":
                default:
                    filtered = Projects;
                    break;
            }

            foreach (var project in filtered)
            {
                FilteredProjects.Add(project);
            }

            OnPropertyChanged(nameof(FilteredProjects));
        }

        private void FilterAll_Click(object sender, RoutedEventArgs e)
        {
            _currentFilter = "All";
            ApplyFilter();
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
            var createProjectPage = new CreateProjectPage(_mainFrame, _user, _context);
            _mainFrame.Navigate(createProjectPage);
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

        private void EditProject_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Project project)
            {
                try
                {
                    long employeeId = _user.EmployeeId ?? 0;
                    bool isAssignedToProject = project.ProjectEmployees.Any(pe => pe.EmployeeId == employeeId);

                    if (isAssignedToProject || _user.Employee.Position.PositionName == "Admin")
                    {
                        var editPage = new EditProjectPage(_mainFrame, _user, _context, project);
                        _mainFrame.Navigate(editPage);
                    }
                    else
                    {
                        MessageBox.Show("Nie masz uprawnień do edycji tego projektu.",
                                       "Brak uprawnień", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Nie można otworzyć edycji projektu: {ex.Message}",
                                   "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
