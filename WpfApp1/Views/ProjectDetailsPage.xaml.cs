using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Data;
using WpfApp1.Model;
using WpfApp1.Models;

namespace WpfApp1.Views
{
    public partial class ProjectDetailsPage : Page
    {
        private readonly Frame _mainFrame;
        private readonly User _user;
        private readonly SyzyfContext _context;
        private readonly IProjectBase _projectData;
        private readonly Project _projectForActions;

        public ProjectDetailsPage(Frame mainFrame, User user, SyzyfContext context, ProjectCard card)
        {
            InitializeComponent();

            _mainFrame = mainFrame;
            _user = user;
            _context = context;
            _projectData = card;
            _projectForActions = null;

            InitializeUI();
        }

        public ProjectDetailsPage(Frame mainFrame, User user, SyzyfContext context, Project project)
        {
            InitializeComponent();

            _mainFrame = mainFrame;
            _user = user;
            _context = context;
            _projectData = project;
            _projectForActions = project;

            InitializeUI();
        }

        private void InitializeUI()
        {
            TopMenu.Initialize(_mainFrame, _user);

            if (_user?.EmployeeId == null || _projectForActions == null)
            {
                AcceptButton.Visibility = Visibility.Collapsed;
                RejectButton.Visibility = Visibility.Collapsed; 
            }
            DisplayDetails();
        }

        private void DisplayDetails()
        {
            if (_projectData == null)
            {
                MessageBox.Show("Brak danych do wyświetlenia.");
                return;
            }

            AddSectionHeader("1. Informacje o ofercie");
            AddTextBlock("Liczba osób:", _projectData.NumberOfPeople.ToString());
            AddTextBlock("Wynagrodzenie widoczne:", _projectData.IsSalaryVisible ? "Tak" : "Nie");

            AddSectionHeader("2. Informacje o stanowisku");
            AddTextBlock("Nazwa stanowiska:", _projectData.JobTitle);
            AddTextBlock("Poziom stanowiska:", _projectData.JobLevels);
            AddTextBlock("Dział:", _projectData.Department);

            AddSectionHeader("Zakres obowiązków");
            AddTextBlock("Główne obowiązki:", _projectData.MainDuties);
            AddTextBlock("Dodatkowe obowiązki:", _projectData.AdditionalDuties);
            AddTextBlock("Planowana data zatrudnienia:", _projectData.PlannedHiringDate.ToShortDateString());

            AddSectionHeader("3. Wymagania dotyczące kandydata");
            AddTextBlock("Wykształcenie:", _projectData.Education);
            AddTextBlock("Preferowane kierunki studiów:", _projectData.PreferredStudyFields);
            AddTextBlock("Dodatkowe uprawnienia:", _projectData.AdditionalCertifications);
            AddTextBlock("Wymagane doświadczenie:", _projectData.RequiredExperience);
            AddTextBlock("Mile widziane doświadczenie:", _projectData.PreferredExperience);
            AddTextBlock("Wymagane umiejętności:", _projectData.RequiredSkills);
            AddTextBlock("Mile widziane umiejętności:", _projectData.PreferredSkills);
            AddTextBlock("Wymagane języki:", _projectData.RequiredLanguages);
            AddTextBlock("Mile widziane języki:", _projectData.PreferredLanguages);

            AddSectionHeader("4. Warunki pracy");
            AddTextBlock("Forma zatrudnienia:", _projectData.EmploymentsForms);
            AddTextBlock("Wynagrodzenie brutto:", _projectData.IsSalaryVisible ? _projectData.GrossSalary : "Ukryte");
            AddTextBlock("System premiowy:", _projectData.BonusSystem ? "Tak" : "Nie");
            AddTextBlock("Dodatkowe benefity:", _projectData.AdditionalBenefits);
            AddTextBlock("Narzędzia pracy:", _projectData.WorkTools);
            AddTextBlock("Miejsce pracy:", _projectData.WorkPlace);
            AddTextBlock("Godziny pracy:", _projectData.WorkingHours);
            AddTextBlock("Tryb pracy:", _projectData.WorkModes);

            AddSectionHeader("5. Inne uwagi");
            AddTextBlock("Pozostałe informacje:", _projectData.OtherRemarks);
        }

        private void AddSectionHeader(string header)
        {
            DetailsContainer.Children.Add(new TextBlock
            {
                Text = header,
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Margin = new Thickness(0, 20, 0, 5)
            });
        }

        private void AddTextBlock(string label, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;

            DetailsContainer.Children.Add(new TextBlock
            {
                Text = label,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 5, 0, 0)
            });

            DetailsContainer.Children.Add(new TextBlock
            {
                Text = value,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 10)
            });
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
        private async void Accept_Click(object sender, RoutedEventArgs e)
        {
            if (_user.EmployeeId == null) return;

            bool isSupport = _user.Employee.Position.PositionName == "Wsparcie";
            bool isSales = _user.Employee.Position.PositionName == "Handlowiec";

            if (_projectData is ProjectCard card)
            {
                if (isSales)
                    card.IsAcceptedBySales = true;
                else if (isSupport)
                    card.IsAcceptedBySupport = true;

                if (card.IsAcceptedBySales && card.IsAcceptedBySupport && isSupport)
                {
                    var newProject = new Project
                    {
                        ClientId = card.ClientId,
                        NumberOfPeople = card.NumberOfPeople,
                        JobLevels = card.JobLevels,
                        IsSalaryVisible = card.IsSalaryVisible,
                        JobTitle = card.JobTitle,
                        Department = card.Department,
                        MainDuties = card.MainDuties,
                        AdditionalDuties = card.AdditionalDuties,
                        DevelopmentOpportunities = card.DevelopmentOpportunities,
                        PlannedHiringDate = card.PlannedHiringDate,
                        Education = card.Education,
                        PreferredStudyFields = card.PreferredStudyFields,
                        AdditionalCertifications = card.AdditionalCertifications,
                        RequiredExperience = card.RequiredExperience,
                        PreferredExperience = card.PreferredExperience,
                        RequiredSkills = card.RequiredSkills,
                        PreferredSkills = card.PreferredSkills,
                        RequiredLanguages = card.RequiredLanguages,
                        PreferredLanguages = card.PreferredLanguages,
                        EmploymentsForms = card.EmploymentsForms,
                        GrossSalary = card.GrossSalary,
                        BonusSystem = card.BonusSystem,
                        AdditionalBenefits = card.AdditionalBenefits,
                        WorkTools = card.WorkTools,
                        WorkPlace = card.WorkPlace,
                        WorkModes = card.WorkModes,
                        WorkingHours = card.WorkingHours,
                        OtherRemarks = card.OtherRemarks,
                        Status = ProjectStatus.Planned,
                    };

                    _context.Projects.Add(newProject);
                    await _context.SaveChangesAsync();

                    var projectEmployee = new ProjectEmployee
                    {
                        ProjectId = newProject.Id,
                        EmployeeId = _user.EmployeeId
                    };

                    _context.ProjectEmployees.Add(projectEmployee);
                }



                try
                {
                    await _context.SaveChangesAsync();
                    MessageBox.Show("Projekt zaakceptowany.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Błąd podczas zapisu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }      
        
        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Projekt został odrzucony.");
        }
        private void EditProject_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                long employeeId = _user.EmployeeId ?? 0;
                bool isAssignedToProject = _projectForActions.ProjectEmployees.Any(pe => pe.EmployeeId == employeeId);
                long isClient = _user.ClientId ?? 0;
                if (employeeId != 0)
                {
                    if (isAssignedToProject || _user.Employee.Position.PositionName == "Admin")
                    {
                        _mainFrame.Navigate(new ProjectCardFormPage(_mainFrame, _user, _context, _projectForActions));
                    }
                }
                else if(isClient != 0)
                {
                    _mainFrame.Navigate(new ProjectCardFormPage(_mainFrame, _user, _context, _projectForActions));
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
}
