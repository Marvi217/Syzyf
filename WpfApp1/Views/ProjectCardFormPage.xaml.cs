using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.Data;
using WpfApp1.Model;
using WpfApp1.Models;

namespace WpfApp1.Views
{
    public partial class ProjectCardFormPage : Page
    {
        private readonly Frame _mainFrame;
        private readonly User _user;
        private readonly SyzyfContext _context;
        private Notification _notification;
        private Project _project;

        // Konstruktor do tworzenia nowej karty projektu (bez istniejącego projektu)
        public ProjectCardFormPage(Frame mainFrame, User user, SyzyfContext context, Notification notification)
        {
            InitializeComponent();

            _mainFrame = mainFrame;
            _user = user;
            _context = context;
            _notification = notification;
            _project = null;

            TopMenu.Initialize(_mainFrame, _user);
            ConfigureButtonVisibility();
            SetInitialHintColors();
        }

        // Konstruktor do edycji istniejącego projektu
        public ProjectCardFormPage(Frame mainFrame, User user, SyzyfContext context, Project project)
        {
            InitializeComponent();

            _mainFrame = mainFrame;
            _user = user;
            _context = context;
            _project = project;

            TopMenu.Initialize(_mainFrame, _user);
            ConfigureButtonVisibility();
            SetInitialHintColors();
            FillFormWithProjectData();
        }

        private void ConfigureButtonVisibility()
        {
            bool isEditMode = _project != null;

            SaveCardButton.Visibility = isEditMode ? Visibility.Collapsed : Visibility.Visible;
            CancelCardButton.Visibility = isEditMode ? Visibility.Collapsed : Visibility.Visible;

            SaveProjectButton.Visibility = isEditMode ? Visibility.Visible : Visibility.Collapsed;
            CancelProjectButton.Visibility = isEditMode ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void SaveProjectCard_Click(object sender, RoutedEventArgs e)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var jobLevels = GetCheckedValuesFromPanel("JobLevelsPanel");
                var education = GetCheckedValuesFromPanel("EducationPanel");
                var employmentForms = GetCheckedValuesFromPanel("EmploymentFormsPanel");
                var workModes = GetCheckedValuesFromPanel("WorkModesPanel");
                var salaryVisibility = GetCheckedValuesFromPanel("SalaryVisibilityPanel");
                var bonusSystem = GetCheckedValuesFromPanel("BonusPanel");

                bool salaryVisible = salaryVisibility?.Contains("Tak") == true;
                bool bonus = bonusSystem?.Contains("Tak") == true;

                var projectCard = new ProjectCard
                {
                    ClientId = _user.ClientId ?? 0,
                    NumberOfPeople = int.TryParse(NumberOfPeopleBox.Text, out var num) ? num : 0,
                    IsSalaryVisible = salaryVisible,
                    JobTitle = JobTitleBox.Text.Trim(),
                    JobLevels = jobLevels,
                    Department = DepartmentBox.Text.Trim(),
                    MainDuties = MainDutiesBox.Text.Trim(),
                    AdditionalDuties = AdditionalDutiesBox.Text.Trim(),
                    PlannedHiringDate = PlannedHiringDatePicker.SelectedDate ?? DateTime.Now,
                    Education = education,
                    PreferredStudyFields = PreferredStudyFieldsBox.Text.Trim(),
                    AdditionalCertifications = AdditionalCertificationsBox.Text.Trim(),
                    RequiredExperience = RequiredExperienceBox.Text.Trim(),
                    PreferredExperience = PreferredExperienceBox.Text.Trim(),
                    RequiredSkills = RequiredSkillsBox.Text.Trim(),
                    PreferredSkills = PreferredSkillsBox.Text.Trim(),
                    RequiredLanguages = RequiredLanguagesBox.Text.Trim(),
                    PreferredLanguages = PreferredLanguagesBox.Text.Trim(),
                    EmploymentsForms = employmentForms,
                    GrossSalary = GrossSalaryBox.Text.Trim(),
                    BonusSystem = bonus,
                    AdditionalBenefits = AdditionalBenefitsBox.Text.Trim(),
                    WorkTools = WorkToolsBox.Text.Trim(),
                    WorkPlace = WorkPlaceBox.Text.Trim(),
                    WorkModes = workModes,
                    WorkingHours = WorkingHoursBox.Text.Trim(),
                    OtherRemarks = OtherRemarksBox.Text.Trim()
                };

                await _context.ProjectCards.AddAsync(projectCard);
                await _context.SaveChangesAsync();

                if (_notification != null)
                {
                    _notification.Tag = "filled";

                    var response = new Notification
                    {
                        ProjectCardId = projectCard.Id,
                        Title = "Karta projektu",
                        Message = $"Karta projektu '{projectCard.JobTitle}' został zapisany przez użytkownika {_user.Login} w dniu {DateTime.Now:yyyy-MM-dd HH:mm}.",
                        FromId = _user.Id,
                        Tag = "fulfilled",
                        ToId = _notification.FromId,
                        IsRead = false
                    };

                    _context.Notifications.Add(response);
                    _context.Notifications.Update(_notification);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                MessageBox.Show("Projekt został zapisany pomyślnie.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                _mainFrame.GoBack();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                MessageBox.Show($"Wystąpił błąd podczas zapisu projektu:\n{ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SaveProject_Click(object sender, RoutedEventArgs e)
        {
            if (_project == null)
            {
                MessageBox.Show("Brak projektu do aktualizacji.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var project = await _context.Projects.FindAsync(_project.Id);

                if (project == null)
                {
                    MessageBox.Show("Projekt nie został znaleziony w bazie danych.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Aktualizacja pól projektu
                project.NumberOfPeople = int.TryParse(NumberOfPeopleBox.Text, out var num) ? num : 0;
                project.IsSalaryVisible = GetCheckedValuesFromPanel("SalaryVisibilityPanel")?.Contains("Tak") == true;
                project.JobTitle = JobTitleBox.Text.Trim();
                project.JobLevels = GetCheckedValuesFromPanel("JobLevelsPanel");
                project.Department = DepartmentBox.Text.Trim();
                project.MainDuties = MainDutiesBox.Text.Trim();
                project.AdditionalDuties = AdditionalDutiesBox.Text.Trim();
                project.PlannedHiringDate = PlannedHiringDatePicker.SelectedDate ?? DateTime.Now;
                project.Education = GetCheckedValuesFromPanel("EducationPanel");
                project.PreferredStudyFields = PreferredStudyFieldsBox.Text.Trim();
                project.AdditionalCertifications = AdditionalCertificationsBox.Text.Trim();
                project.RequiredExperience = RequiredExperienceBox.Text.Trim();
                project.PreferredExperience = PreferredExperienceBox.Text.Trim();
                project.RequiredSkills = RequiredSkillsBox.Text.Trim();
                project.PreferredSkills = PreferredSkillsBox.Text.Trim();
                project.RequiredLanguages = RequiredLanguagesBox.Text.Trim();
                project.PreferredLanguages = PreferredLanguagesBox.Text.Trim();
                project.EmploymentsForms = GetCheckedValuesFromPanel("EmploymentFormsPanel");
                project.GrossSalary = GrossSalaryBox.Text.Trim();
                project.BonusSystem = GetCheckedValuesFromPanel("BonusPanel")?.Contains("Tak") == true;
                project.AdditionalBenefits = AdditionalBenefitsBox.Text.Trim();
                project.WorkTools = WorkToolsBox.Text.Trim();
                project.WorkPlace = WorkPlaceBox.Text.Trim();
                project.WorkModes = GetCheckedValuesFromPanel("WorkModesPanel");
                project.WorkingHours = WorkingHoursBox.Text.Trim();
                project.OtherRemarks = OtherRemarksBox.Text.Trim();

                _context.Projects.Update(project);
                await _context.SaveChangesAsync();

                Notification response = null;

                if (_user.ClientId == null)
                {
                    // Jeżeli użytkownik nie ma ClientId — powiadom klienta
                    var clientUser = _context.Users.FirstOrDefault(u => u.ClientId == project.ClientId);

                    if (clientUser != null)
                    {
                        response = new Notification
                        {
                            ProjectId = project.Id,
                            Title = "Projekt",
                            Message = $"Projekt '{project.JobTitle}' został zmieniony przez użytkownika {_user.Login} w dniu {DateTime.Now:yyyy-MM-dd HH:mm}.",
                            FromId = _user.Id,
                            Tag = "changed",
                            ToId = clientUser.Id,
                            IsRead = false
                        };
                    }
                }
                else if (_user.EmployeeId == null)
                {
                    // Jeżeli użytkownik nie ma EmployeeId — powiadom wsparcie
                    var employee = _context.ProjectEmployees
                        .Include(pe => pe.Employee)
                            .ThenInclude(e => e.User)
                        .Include(pe => pe.Employee.Position)
                        .Where(pe => pe.ProjectId == project.Id && pe.Employee.Position.PositionName == "Wsparcie")
                        .Select(pe => pe.Employee)
                        .FirstOrDefault();

                    var employeeUser = employee?.User;

                    if (employeeUser != null)
                    {
                        response = new Notification
                        {
                            ProjectId = project.Id,
                            Title = "Projekt",
                            Message = $"Projekt '{project.JobTitle}' został zmieniony przez {employeeUser.Employee.FirstName} {employeeUser.Employee.LastName} w dniu {DateTime.Now:yyyy-MM-dd HH:mm}.",
                            FromId = _user.Id,
                            Tag = "changed",
                            ToId = employeeUser.Id,
                            IsRead = false
                        };
                    }
                }

                if (response != null)
                {
                    _context.Notifications.Add(response);
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                MessageBox.Show("Zmiany zostały zapisane.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                _mainFrame.GoBack();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                MessageBox.Show($"Błąd podczas aktualizacji projektu:\n{ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetCheckedValuesFromPanel(string panelName)
        {
            if (FindName(panelName) is StackPanel panel)
            {
                var selected = panel.Children
                    .OfType<CheckBox>()
                    .Where(cb => cb.IsChecked == true)
                    .Select(cb => cb.Content.ToString());

                return string.Join(", ", selected);
            }

            return string.Empty;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz anulować wypełnianie formularza?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _mainFrame.GoBack();
            }
        }

        private void SetInitialHintColors()
        {
            var boxes = new List<TextBox>
            {
                NumberOfPeopleBox, JobTitleBox, DepartmentBox, MainDutiesBox, AdditionalDutiesBox,
                PreferredStudyFieldsBox, AdditionalCertificationsBox, RequiredExperienceBox, PreferredExperienceBox,
                RequiredSkillsBox, PreferredSkillsBox, RequiredLanguagesBox, PreferredLanguagesBox,
                GrossSalaryBox, AdditionalBenefitsBox, WorkToolsBox, WorkPlaceBox, WorkingHoursBox, OtherRemarksBox
            };

            foreach (var tb in boxes)
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    SetHintForTextBox(tb);
                }
            }
        }

        private void SetHintForTextBox(TextBox tb, string ?hint = null)
        {
            if (hint == null)
            {
                hint = tb.Name switch
                {
                    "NumberOfPeopleBox" => "Liczba osób",
                    "JobTitleBox" => "Nazwa stanowiska",
                    "DepartmentBox" => "Dział",
                    "MainDutiesBox" => "Główne obowiązki",
                    "AdditionalDutiesBox" => "Dodatkowe obowiązki",
                    "PreferredStudyFieldsBox" => "Preferowane kierunki studiów",
                    "AdditionalCertificationsBox" => "Dodatkowe uprawnienia",
                    "RequiredExperienceBox" => "Wymagane doświadczenie",
                    "PreferredExperienceBox" => "Mile widziane doświadczenie",
                    "RequiredSkillsBox" => "Wymagane umiejętności",
                    "PreferredSkillsBox" => "Mile widziane umiejętności",
                    "RequiredLanguagesBox" => "Wymagane języki",
                    "PreferredLanguagesBox" => "Mile widziane języki",
                    "GrossSalaryBox" => "Wynagrodzenie brutto",
                    "AdditionalBenefitsBox" => "Dodatkowe benefity",
                    "WorkToolsBox" => "Narzędzia pracy",
                    "WorkPlaceBox" => "Miejsce pracy",
                    "WorkingHoursBox" => "Godziny pracy",
                    "OtherRemarksBox" => "Pozostałe informacje",
                    _ => ""
                };
            }

            tb.Text = hint;
            tb.Foreground = Brushes.Gray;
        }

        private void FillFormWithProjectData()
        {
            if (_project == null)
                return;

            NumberOfPeopleBox.Text = _project.NumberOfPeople > 0 ? _project.NumberOfPeople.ToString() : "";
            JobTitleBox.Text = _project.JobTitle ?? "";
            DepartmentBox.Text = _project.Department ?? "";
            MainDutiesBox.Text = _project.MainDuties ?? "";
            AdditionalDutiesBox.Text = _project.AdditionalDuties ?? "";
            PlannedHiringDatePicker.SelectedDate = _project.PlannedHiringDate;
            PreferredStudyFieldsBox.Text = _project.PreferredStudyFields ?? "";
            AdditionalCertificationsBox.Text = _project.AdditionalCertifications ?? "";
            RequiredExperienceBox.Text = _project.RequiredExperience ?? "";
            PreferredExperienceBox.Text = _project.PreferredExperience ?? "";
            RequiredSkillsBox.Text = _project.RequiredSkills ?? "";
            PreferredSkillsBox.Text = _project.PreferredSkills ?? "";
            RequiredLanguagesBox.Text = _project.RequiredLanguages ?? "";
            PreferredLanguagesBox.Text = _project.PreferredLanguages ?? "";
            GrossSalaryBox.Text = _project.GrossSalary ?? "";
            AdditionalBenefitsBox.Text = _project.AdditionalBenefits ?? "";
            WorkToolsBox.Text = _project.WorkTools ?? "";
            WorkPlaceBox.Text = _project.WorkPlace ?? "";
            WorkingHoursBox.Text = _project.WorkingHours ?? "";
            OtherRemarksBox.Text = _project.OtherRemarks ?? "";

            SetCheckBoxesFromCommaSeparated(JobLevelsPanel, _project.JobLevels);
            SetCheckBoxesFromCommaSeparated(EducationPanel, _project.Education);
            SetCheckBoxesFromCommaSeparated(EmploymentFormsPanel, _project.EmploymentsForms);
            SetCheckBoxesFromCommaSeparated(WorkModesPanel, _project.WorkModes);

            SetCheckboxGroup(SalaryVisibilityPanel, _project.IsSalaryVisible ? "Tak" : "Nie");
            SetCheckboxGroup(BonusPanel, _project.BonusSystem ? "Tak" : "Nie");
        }

        private void SetCheckBoxesFromCommaSeparated(StackPanel panel, string values)
        {
            if (panel == null || string.IsNullOrWhiteSpace(values))
                return;

            var selectedValues = values.Split(',').Select(s => s.Trim());

            foreach (var cb in panel.Children.OfType<CheckBox>())
            {
                cb.IsChecked = selectedValues.Contains(cb.Content.ToString());
            }
        }

        private void SetCheckboxGroup(StackPanel panel, string valueToCheck)
        {
            if (panel == null)
                return;

            foreach (var cb in panel.Children.OfType<CheckBox>())
            {
                cb.IsChecked = cb.Content.ToString() == valueToCheck;
            }
        }
        private void HintBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                // Przykład: czyścimy tekst podpowiedzi, gdy textbox zyskuje fokus
                if (tb.Text == tb.Tag?.ToString())
                {
                    tb.Text = "";
                    tb.Foreground = Brushes.Black; // np. zmiana koloru tekstu na czarny
                }
            }
        }

        private void HintBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                // Przykład: ustawiamy tekst podpowiedzi, jeśli pole jest puste
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = tb.Tag?.ToString(); // zakładam, że tekst podpowiedzi jest w Tagu
                    tb.Foreground = Brushes.Gray;  // np. szary kolor dla podpowiedzi
                }
            }
        }

    }

}
