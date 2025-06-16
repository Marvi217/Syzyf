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
        private ProjectCard _project;

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
        public ProjectCardFormPage(Frame mainFrame, User user, SyzyfContext context, ProjectCard project)
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

                if (_project == null)
                {
                    var newCard = new ProjectCard
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
                        OtherRemarks = OtherRemarksBox.Text.Trim(),
                        Status = ProjectCardStatus.Pending
                    };

                    await _context.ProjectCards.AddAsync(newCard);
                }
                else
                {
                    var existingCard = await _context.ProjectCards.FindAsync(_project.Id);

                    existingCard.NumberOfPeople = int.TryParse(NumberOfPeopleBox.Text, out var num) ? num : 0;
                    existingCard.IsSalaryVisible = salaryVisible;
                    existingCard.JobTitle = JobTitleBox.Text.Trim();
                    existingCard.JobLevels = jobLevels;
                    existingCard.Department = DepartmentBox.Text.Trim();
                    existingCard.MainDuties = MainDutiesBox.Text.Trim();
                    existingCard.AdditionalDuties = AdditionalDutiesBox.Text.Trim();
                    existingCard.PlannedHiringDate = PlannedHiringDatePicker.SelectedDate ?? DateTime.Now;
                    existingCard.Education = education;
                    existingCard.PreferredStudyFields = PreferredStudyFieldsBox.Text.Trim();
                    existingCard.AdditionalCertifications = AdditionalCertificationsBox.Text.Trim();
                    existingCard.RequiredExperience = RequiredExperienceBox.Text.Trim();
                    existingCard.PreferredExperience = PreferredExperienceBox.Text.Trim();
                    existingCard.RequiredSkills = RequiredSkillsBox.Text.Trim();
                    existingCard.PreferredSkills = PreferredSkillsBox.Text.Trim();
                    existingCard.RequiredLanguages = RequiredLanguagesBox.Text.Trim();
                    existingCard.PreferredLanguages = PreferredLanguagesBox.Text.Trim();
                    existingCard.EmploymentsForms = employmentForms;
                    existingCard.GrossSalary = GrossSalaryBox.Text.Trim();
                    existingCard.BonusSystem = bonus;
                    existingCard.AdditionalBenefits = AdditionalBenefitsBox.Text.Trim();
                    existingCard.WorkTools = WorkToolsBox.Text.Trim();
                    existingCard.WorkPlace = WorkPlaceBox.Text.Trim();
                    existingCard.WorkModes = workModes;
                    existingCard.WorkingHours = WorkingHoursBox.Text.Trim();
                    existingCard.OtherRemarks = OtherRemarksBox.Text.Trim();
                    existingCard.Status = ProjectCardStatus.Pending;

                    _context.ProjectCards.Update(existingCard);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                MessageBox.Show("Karta projektu została zapisana pomyślnie.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                _mainFrame.GoBack();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                MessageBox.Show($"Błąd podczas zapisu karty projektu:\n{ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
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

                    if (clientUser == null)
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
            if (_project == null) return;

            // Dla pól liczbowych
            NumberOfPeopleBox.Text = _project.NumberOfPeople > 0 ? _project.NumberOfPeople.ToString() : "";
            if (!string.IsNullOrEmpty(NumberOfPeopleBox.Text))
                NumberOfPeopleBox.Foreground = Brushes.Black;

            // Dla pól tekstowych
            SetTextBoxIfNotEmpty(JobTitleBox, _project.JobTitle);
            SetTextBoxIfNotEmpty(DepartmentBox, _project.Department);
            SetTextBoxIfNotEmpty(MainDutiesBox, _project.MainDuties);
            SetTextBoxIfNotEmpty(AdditionalDutiesBox, _project.AdditionalDuties);
            SetTextBoxIfNotEmpty(PreferredStudyFieldsBox, _project.PreferredStudyFields);
            SetTextBoxIfNotEmpty(AdditionalCertificationsBox, _project.AdditionalCertifications);
            SetTextBoxIfNotEmpty(RequiredExperienceBox, _project.RequiredExperience);
            SetTextBoxIfNotEmpty(PreferredExperienceBox, _project.PreferredExperience);
            SetTextBoxIfNotEmpty(RequiredSkillsBox, _project.RequiredSkills);
            SetTextBoxIfNotEmpty(PreferredSkillsBox, _project.PreferredSkills);
            SetTextBoxIfNotEmpty(RequiredLanguagesBox, _project.RequiredLanguages);
            SetTextBoxIfNotEmpty(PreferredLanguagesBox, _project.PreferredLanguages);
            SetTextBoxIfNotEmpty(GrossSalaryBox, _project.GrossSalary);
            SetTextBoxIfNotEmpty(AdditionalBenefitsBox, _project.AdditionalBenefits);
            SetTextBoxIfNotEmpty(WorkToolsBox, _project.WorkTools);
            SetTextBoxIfNotEmpty(WorkPlaceBox, _project.WorkPlace);
            SetTextBoxIfNotEmpty(WorkingHoursBox, _project.WorkingHours);
            SetTextBoxIfNotEmpty(OtherRemarksBox, _project.OtherRemarks);

            PlannedHiringDatePicker.SelectedDate = _project.PlannedHiringDate;

            SetCheckBoxesFromCommaSeparated(JobLevelsPanel, _project.JobLevels);
            SetCheckBoxesFromCommaSeparated(EducationPanel, _project.Education);
            SetCheckBoxesFromCommaSeparated(EmploymentFormsPanel, _project.EmploymentsForms);
            SetCheckBoxesFromCommaSeparated(WorkModesPanel, _project.WorkModes);

            SetRadioButtonGroup(SalaryVisibilityPanel, _project.IsSalaryVisible ? "Tak" : "Nie");
            SetRadioButtonGroup(BonusPanel, _project.BonusSystem ? "Tak" : "Nie");
        }

        private void SetRadioButtonGroup(StackPanel panel, string valueToCheck)
        {
            if (panel == null) return;

            foreach (var rb in panel.Children.OfType<RadioButton>())
            {
                rb.IsChecked = rb.Content.ToString() == valueToCheck;
            }
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
                // Ustawiamy Tag z tekstem podpowiedzi
                tb.Tag = GetHintTextForTextBox(tb);

                // Jeśli TextBox jest pusty, ustawiamy podpowiedź
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = (string)tb.Tag;
                    tb.Foreground = Brushes.Gray;
                }
                else
                {
                    tb.Foreground = Brushes.Black;
                }
            }
        }

        private string GetHintTextForTextBox(TextBox tb)
        {
            return tb.Name switch
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

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Tag is string hint)
            {
                if (tb.Text == hint)
                {
                    tb.Text = "";
                    tb.Foreground = Brushes.Black;
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb && tb.Tag is string hint)
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = hint;
                    tb.Foreground = Brushes.Gray;
                }
            }
        }

        private void TextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is TextBox tb && tb.Tag is string hint)
            {
                if (tb.Text == hint)
                {
                    tb.Text = "";
                    tb.Foreground = Brushes.Black;
                    tb.Focus(); // Ustawiamy fokus na TextBox
                    e.Handled = true; // Zapobiegamy domyślnej akcji
                }
            }
        }

        private void SetTextBoxIfNotEmpty(TextBox textBox, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                textBox.Text = value;
                textBox.Foreground = Brushes.Black;
            }
            else
            {
                textBox.Text = (string)textBox.Tag;
                textBox.Foreground = Brushes.Gray;
            }
        }

    }

}
