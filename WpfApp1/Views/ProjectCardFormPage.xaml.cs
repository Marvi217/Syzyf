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
        private Frame _mainFrame;
        private User _user;
        private SyzyfContext _context;
        private Notification _notification;
        private Project _project;

        public ProjectCardFormPage(Frame mainFrame, User user, SyzyfContext context, Notification notification)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _user = user;
            _context = context;
            _notification = notification;

            TopMenu.Initialize(_mainFrame, _user);
            SetInitialHintColors();
        }

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
            bool isProjectContext = _project != null;

            SaveCardButton.Visibility = isProjectContext ? Visibility.Collapsed : Visibility.Visible;
            CancelCardButton.Visibility = isProjectContext ? Visibility.Collapsed : Visibility.Visible;

            SaveProjectButton.Visibility = isProjectContext ? Visibility.Visible : Visibility.Collapsed;
            CancelProjectButton.Visibility = isProjectContext ? Visibility.Visible : Visibility.Collapsed;
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
                var salaryVsibility = GetCheckedValuesFromPanel("SalaryVisibilityPanel");
                var bonusSystem = GetCheckedValuesFromPanel("BonusPanel");

                bool salary = salaryVsibility?.Contains("Tak") == true;
                bool bonus = bonusSystem?.Contains("Tak") == true;

                var project = new ProjectCard
                {
                    ClientId = _user.ClientId ?? 0,
                    NumberOfPeople = int.TryParse(NumberOfPeopleBox.Text, out var num) ? num : 0,
                    IsSalaryVisible = salary,
                    JobTitle = JobTitleBox.Text,
                    JobLevels = jobLevels,
                    Department = DepartmentBox.Text,
                    MainDuties = MainDutiesBox.Text,
                    AdditionalDuties = AdditionalDutiesBox.Text,
                    PlannedHiringDate = PlannedHiringDatePicker.SelectedDate ?? DateTime.Now,
                    Education = education,
                    PreferredStudyFields = PreferredStudyFieldsBox.Text,
                    AdditionalCertifications = AdditionalCertificationsBox.Text,
                    RequiredExperience = RequiredExperienceBox.Text,
                    PreferredExperience = PreferredExperienceBox.Text,
                    RequiredSkills = RequiredSkillsBox.Text,
                    PreferredSkills = PreferredSkillsBox.Text,
                    RequiredLanguages = RequiredLanguagesBox.Text,
                    PreferredLanguages = PreferredLanguagesBox.Text,
                    EmploymentsForms = employmentForms,
                    GrossSalary = GrossSalaryBox.Text,
                    BonusSystem = bonus,
                    AdditionalBenefits = AdditionalBenefitsBox.Text,
                    WorkTools = WorkToolsBox.Text,
                    WorkPlace = WorkPlaceBox.Text,
                    WorkModes = workModes,
                    WorkingHours = WorkingHoursBox.Text,
                    OtherRemarks = OtherRemarksBox.Text,
                };

                await _context.ProjectCards.AddAsync(project);
                await _context.SaveChangesAsync();

                if (_notification != null)
                {
                    _notification.Tag = "filled";

                    var response = new Notification
                    {
                        ProjectCardId = project.Id,
                        Title = "Karta projektu",
                        Message = $"Projekt '{project.JobTitle}' został zapisany przez użytkownika {_user.Login} w dniu {DateTime.Now:yyyy-MM-dd HH:mm}.",
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
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (_project == null)
                {
                    MessageBox.Show("Brak projektu do aktualizacji.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var project = await _context.Projects.FindAsync(_project.Id);
                if (project == null)
                {
                    MessageBox.Show("Projekt nie został znaleziony w bazie danych.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                project.NumberOfPeople = int.TryParse(NumberOfPeopleBox.Text, out var num) ? num : 0;
                project.IsSalaryVisible = GetCheckedValuesFromPanel("SalaryVisibilityPanel")?.Contains("Tak") == true;
                project.JobTitle = JobTitleBox.Text;
                project.JobLevels = GetCheckedValuesFromPanel("JobLevelsPanel");
                project.Department = DepartmentBox.Text;
                project.MainDuties = MainDutiesBox.Text;
                project.AdditionalDuties = AdditionalDutiesBox.Text;
                project.PlannedHiringDate = PlannedHiringDatePicker.SelectedDate ?? DateTime.Now;
                project.Education = GetCheckedValuesFromPanel("EducationPanel");
                project.PreferredStudyFields = PreferredStudyFieldsBox.Text;
                project.AdditionalCertifications = AdditionalCertificationsBox.Text;
                project.RequiredExperience = RequiredExperienceBox.Text;
                project.PreferredExperience = PreferredExperienceBox.Text;
                project.RequiredSkills = RequiredSkillsBox.Text;
                project.PreferredSkills = PreferredSkillsBox.Text;
                project.RequiredLanguages = RequiredLanguagesBox.Text;
                project.PreferredLanguages = PreferredLanguagesBox.Text;
                project.EmploymentsForms = GetCheckedValuesFromPanel("EmploymentFormsPanel");
                project.GrossSalary = GrossSalaryBox.Text;
                project.BonusSystem = GetCheckedValuesFromPanel("BonusPanel")?.Contains("Tak") == true;
                project.AdditionalBenefits = AdditionalBenefitsBox.Text;
                project.WorkTools = WorkToolsBox.Text;
                project.WorkPlace = WorkPlaceBox.Text;
                project.WorkModes = GetCheckedValuesFromPanel("WorkModesPanel");
                project.WorkingHours = WorkingHoursBox.Text;
                project.OtherRemarks = OtherRemarksBox.Text;

                _context.Projects.Update(project);
                await _context.SaveChangesAsync();

                Notification response = null;
                if (_user.ClientId == null)
                {
                    var clientUser = _context.Users.FirstOrDefault(u => u.ClientId == project.ClientId);

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
                else if (_user.EmployeeId == null)
                {
                    var employeeUser = _context.ProjectEmployees
                        .Where(pe => pe.ProjectId == project.Id)
                        .Select(pe => pe.Employee)
                        .FirstOrDefault(e => e.Position.PositionName == "Wsparcie");

                    response = new Notification
                    {
                        ProjectId = project.Id,
                        Title = "Projekt",
                        Message = $"Projekt '{project.JobTitle}' został zmieniony przez {employeeUser.FirstName + " "+ employeeUser.LastName} w dniu {DateTime.Now:yyyy-MM-dd HH:mm}.",
                        FromId = _user.Id,
                        Tag = "changed",
                        ToId = employeeUser.Id,
                        IsRead = false
                    };
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

        private void SetHintForTextBox(TextBox tb, string hint = null)
        {
            if (hint == null)
            {
                switch (tb.Name)
                {
                    case "NumberOfPeopleBox": hint = "Liczba osób"; break;
                    case "JobTitleBox": hint = "Nazwa stanowiska"; break;
                    case "DepartmentBox": hint = "Dział"; break;
                    case "MainDutiesBox": hint = "Główne obowiązki"; break;
                    case "AdditionalDutiesBox": hint = "Dodatkowe obowiązki"; break;
                    case "PreferredStudyFieldsBox": hint = "Preferowane kierunki studiów"; break;
                    case "AdditionalCertificationsBox": hint = "Dodatkowe uprawnienia"; break;
                    case "RequiredExperienceBox": hint = "Wymagane doświadczenie"; break;
                    case "PreferredExperienceBox": hint = "Mile widziane doświadczenie"; break;
                    case "RequiredSkillsBox": hint = "Wymagane umiejętności"; break;
                    case "PreferredSkillsBox": hint = "Mile widziane umiejętności"; break;
                    case "RequiredLanguagesBox": hint = "Wymagane języki"; break;
                    case "PreferredLanguagesBox": hint = "Mile widziane języki"; break;
                    case "GrossSalaryBox": hint = "Wynagrodzenie brutto"; break;
                    case "AdditionalBenefitsBox": hint = "Dodatkowe benefity"; break;
                    case "WorkToolsBox": hint = "Narzędzia pracy"; break;
                    case "WorkPlaceBox": hint = "Miejsce pracy"; break;
                    case "WorkingHoursBox": hint = "Godziny pracy"; break;
                    case "OtherRemarksBox": hint = "Pozostałe informacje"; break;
                    default: hint = ""; break;
                }
            }

            tb.Text = hint;
            tb.Foreground = Brushes.Gray;
        }

        private void FillFormWithProjectData()
        {
            if (_project == null)
                return;

            NumberOfPeopleBox.Text = _project.NumberOfPeople > 0 ? _project.NumberOfPeople.ToString() : "Liczba osób";

            JobTitleBox.Text = string.IsNullOrWhiteSpace(_project.JobTitle) ? "Nazwa stanowiska" : _project.JobTitle;

            DepartmentBox.Text = string.IsNullOrWhiteSpace(_project.Department) ? "Dział" : _project.Department;

            MainDutiesBox.Text = string.IsNullOrWhiteSpace(_project.MainDuties) ? "Główne obowiązki" : _project.MainDuties;

            AdditionalDutiesBox.Text = string.IsNullOrWhiteSpace(_project.AdditionalDuties) ? "Dodatkowe obowiązki" : _project.AdditionalDuties;

            if (PlannedHiringDatePicker.SelectedDate == null)
                PlannedHiringDatePicker.SelectedDate = _project.PlannedHiringDate;

            PreferredStudyFieldsBox.Text = string.IsNullOrWhiteSpace(_project.PreferredStudyFields) ? "Preferowane kierunki studiów" : _project.PreferredStudyFields;
            AdditionalCertificationsBox.Text = string.IsNullOrWhiteSpace(_project.AdditionalCertifications) ? "Dodatkowe uprawnienia" : _project.AdditionalCertifications;
            RequiredExperienceBox.Text = string.IsNullOrWhiteSpace(_project.RequiredExperience) ? "Wymagane doświadczenie" : _project.RequiredExperience;
            PreferredExperienceBox.Text = string.IsNullOrWhiteSpace(_project.PreferredExperience) ? "Mile widziane doświadczenie" : _project.PreferredExperience;
            RequiredSkillsBox.Text = string.IsNullOrWhiteSpace(_project.RequiredSkills) ? "Wymagane umiejętności" : _project.RequiredSkills;
            PreferredSkillsBox.Text = string.IsNullOrWhiteSpace(_project.PreferredSkills) ? "Mile widziane umiejętności" : _project.PreferredSkills;
            RequiredLanguagesBox.Text = string.IsNullOrWhiteSpace(_project.RequiredLanguages) ? "Wymagane języki" : _project.RequiredLanguages;
            PreferredLanguagesBox.Text = string.IsNullOrWhiteSpace(_project.PreferredLanguages) ? "Mile widziane języki" : _project.PreferredLanguages;

            GrossSalaryBox.Text = string.IsNullOrWhiteSpace(_project.GrossSalary) ? "Wynagrodzenie brutto" : _project.GrossSalary;
            AdditionalBenefitsBox.Text = string.IsNullOrWhiteSpace(_project.AdditionalBenefits) ? "Dodatkowe benefity" : _project.AdditionalBenefits;
            WorkToolsBox.Text = string.IsNullOrWhiteSpace(_project.WorkTools) ? "Narzędzia pracy" : _project.WorkTools;
            WorkPlaceBox.Text = string.IsNullOrWhiteSpace(_project.WorkPlace) ? "Miejsce pracy" : _project.WorkPlace;
            WorkingHoursBox.Text = string.IsNullOrWhiteSpace(_project.WorkingHours) ? "Godziny pracy" : _project.WorkingHours;
            OtherRemarksBox.Text = string.IsNullOrWhiteSpace(_project.OtherRemarks) ? "Pozostałe informacje" : _project.OtherRemarks;

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

            var selectedItems = values.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (CheckBox cb in panel.Children.OfType<CheckBox>())
            {
                cb.IsChecked = selectedItems.Contains(cb.Content.ToString());
            }
        }

        private void SetCheckboxGroup(StackPanel panel, string valueToCheck)
        {
            if (panel == null)
                return;

            foreach (CheckBox cb in panel.Children.OfType<CheckBox>())
            {
                cb.IsChecked = cb.Content.ToString() == valueToCheck;
            }
        }


        private void HintBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Foreground == Brushes.Gray)
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void HintBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && string.IsNullOrWhiteSpace(tb.Text))
            {
                SetHintForTextBox(tb);
            }
        }
    }
}
