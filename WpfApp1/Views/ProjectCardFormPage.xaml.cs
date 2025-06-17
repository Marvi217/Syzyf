using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly Notification _notification;
        private readonly ProjectCard _project;

        public ProjectCardFormPage(Frame mainFrame, User user, SyzyfContext context, Notification notification)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _user = user;
            _context = context;
            _notification = notification;
            _project = null;

            InitializeUI();
            InitializeEventHandlers();
        }

        public ProjectCardFormPage(Frame mainFrame, User user, SyzyfContext context, ProjectCard project)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _user = user;
            _context = context;
            _project = project;

            InitializeUI();
            FillFormWithProjectData();
            InitializeEventHandlers();
        }

        private void InitializeUI()
        {
            // Inicjalizacja komponentów UI, menu, przycisków
            TopMenu.Initialize(_mainFrame, _user);
            ConfigureButtonVisibility();
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
            await SaveProjectCard(false);
        }

        private async void SaveProject_Click(object sender, RoutedEventArgs e)
        {
            await SaveProjectCard(true);
        }

        private async Task SaveProjectCard(bool isProjectSave)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                long currentUserId = _user.Id;
                long projectCardId;

                if (!ValidateRequiredFields())
                {
                    return;
                }

                var formData = GetFormData();

                if (_project == null)
                {
                    var newCard = new ProjectCard
                    {
                        ClientId = _user.ClientId ?? 0,
                        NumberOfPeople = formData.NumberOfPeople,
                        IsSalaryVisible = formData.IsSalaryVisible,
                        JobTitle = formData.JobTitle,
                        JobLevels = formData.JobLevels,
                        Department = formData.Department,
                        MainDuties = formData.MainDuties,
                        AdditionalDuties = formData.AdditionalDuties,
                        PlannedHiringDate = formData.PlannedHiringDate,
                        Education = formData.Education,
                        PreferredStudyFields = formData.PreferredStudyFields,
                        AdditionalCertifications = formData.AdditionalCertifications,
                        RequiredExperience = formData.RequiredExperience,
                        PreferredExperience = formData.PreferredExperience,
                        RequiredSkills = formData.RequiredSkills,
                        PreferredSkills = formData.PreferredSkills,
                        RequiredLanguages = formData.RequiredLanguages,
                        PreferredLanguages = formData.PreferredLanguages,
                        EmploymentsForms = formData.EmploymentsForms,
                        GrossSalary = formData.GrossSalary,
                        BonusSystem = formData.BonusSystem,
                        AdditionalBenefits = formData.AdditionalBenefits,
                        WorkTools = formData.WorkTools,
                        WorkPlace = formData.WorkPlace,
                        WorkModes = formData.WorkModes,
                        WorkingHours = formData.WorkingHours,
                        OtherRemarks = formData.OtherRemarks,
                        Status = ProjectCardStatus.Pending,
                        IsAcceptedDb = isProjectSave
                    };
                    await _context.ProjectCards.AddAsync(newCard);
                    await _context.SaveChangesAsync();

                    projectCardId = newCard.Id;

                    var lastOrder = await _context.Orders
                        .Where(o => o.ClientId == _user.ClientId)
                        .OrderByDescending(o => o.CreatedDate)
                        .FirstOrDefaultAsync();

                    if (lastOrder != null)
                    {
                        lastOrder.ProjectCardId = newCard.Id;
                        await _context.SaveChangesAsync();
                    }

                    if (_notification != null)
                    {
                        var responseNotification = new Notification
                        {
                            FromId = currentUserId,
                            ToId = _notification.FromId,
                            Title = "Nowa karta projektu",
                            Tag = "fulfilled",
                            Message = $"Klient {_user.Client?.Company} utworzył nową kartę projektu '{formData.JobTitle}'.",
                            IsRead = false,
                            ProjectCardId = projectCardId
                        };
                        await _context.Notifications.AddAsync(responseNotification);
                    }
                }
                else
                {
                    var existingCard = await _context.ProjectCards.FindAsync(_project.Id);
                    if (existingCard == null)
                        throw new Exception("Nie znaleziono karty projektu do aktualizacji");

                    UpdateProjectCard(existingCard, formData);
                    existingCard.IsAcceptedDb = isProjectSave;
                    _context.ProjectCards.Update(existingCard);
                    projectCardId = existingCard.Id;

                    var lastNotification = await _context.Notifications
                        .Where(n => n.ToId == currentUserId && n.ProjectCardId == projectCardId)
                        .OrderByDescending(n => n.CreatedAt)
                        .FirstOrDefaultAsync();

                    if (lastNotification != null)
                    {
                        var updateNotification = new Notification
                        {
                            FromId = currentUserId,
                            ToId = lastNotification.FromId,
                            Title = "Edytowana karta projektu",
                            Tag = "reply",
                            Message = $"Użytkownik {_user.Employee?.FirstName} {_user.Employee?.LastName} zaktualizował kartę projektu '{formData.JobTitle}'.",
                            IsRead = false,
                            ProjectCardId = projectCardId,
                            CreatedAt = DateTime.Now
                        };
                        await _context.Notifications.AddAsync(updateNotification);
                    }
                }

                await _context.SaveChangesAsync();

                var updatedCard = await _context.ProjectCards.FindAsync(projectCardId);
                if (updatedCard != null && updatedCard.IsAcceptedDb)
                {
                    await CreateProjectFromCard(projectCardId);
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

        private ProjectCard GetFormData()
        {
            return new ProjectCard
            {
                NumberOfPeople = GetIntFromTextBox(NumberOfPeopleBox),
                IsSalaryVisible = GetBoolFromRadioButtonGroup(SalaryVisibilityPanel),
                JobTitle = GetTextFromTextBox(JobTitleBox),
                JobLevels = GetCheckedValuesFromPanel(JobLevelsPanel),
                Department = GetTextFromTextBox(DepartmentBox),
                MainDuties = GetTextFromTextBox(MainDutiesBox),
                AdditionalDuties = GetTextFromTextBox(AdditionalDutiesBox),
                PlannedHiringDate = PlannedHiringDatePicker.SelectedDate,
                Education = GetCheckedValuesFromPanel(EducationPanel),
                PreferredStudyFields = GetTextFromTextBox(PreferredStudyFieldsBox),
                AdditionalCertifications = GetTextFromTextBox(AdditionalCertificationsBox),
                RequiredExperience = GetTextFromTextBox(RequiredExperienceBox),
                PreferredExperience = GetTextFromTextBox(PreferredExperienceBox),
                RequiredSkills = GetTextFromTextBox(RequiredSkillsBox),
                PreferredSkills = GetTextFromTextBox(PreferredSkillsBox),
                RequiredLanguages = GetTextFromTextBox(RequiredLanguagesBox),
                PreferredLanguages = GetTextFromTextBox(PreferredLanguagesBox),
                EmploymentsForms = GetCheckedValuesFromPanel(EmploymentFormsPanel),
                GrossSalary = GetTextFromTextBox(GrossSalaryBox),
                BonusSystem = GetBoolFromRadioButtonGroup(BonusPanel),
                AdditionalBenefits = GetTextFromTextBox(AdditionalBenefitsBox),
                WorkTools = GetTextFromTextBox(WorkToolsBox),
                WorkPlace = GetTextFromTextBox(WorkPlaceBox),
                WorkModes = GetCheckedValuesFromPanel(WorkModesPanel),
                WorkingHours = GetTextFromTextBox(WorkingHoursBox),
                OtherRemarks = GetTextFromTextBox(OtherRemarksBox)
            };
        }

        private string GetTextFromTextBox(TextBox textBox)
        {
            return textBox?.Text?.Trim() ?? "";
        }

        private int GetIntFromTextBox(TextBox textBox)
        {
            if (int.TryParse(GetTextFromTextBox(textBox), out int result))
                return result;
            return 0;
        }

        private bool GetBoolFromRadioButtonGroup(StackPanel panel)
        {
            var radio = panel.Children.OfType<RadioButton>().FirstOrDefault(rb => rb.IsChecked == true);
            if (radio != null)
                return radio.Content.ToString() == "Tak";
            return false;
        }

        private string GetCheckedValuesFromPanel(StackPanel panel)
        {
            if (panel == null) return "";
            var selected = panel.Children.OfType<CheckBox>().Where(cb => cb.IsChecked == true)
                .Select(cb => cb.Content.ToString());
            return string.Join(", ", selected);
        }

        private void UpdateProjectCard(ProjectCard existing, ProjectCard data)
        {
            existing.NumberOfPeople = data.NumberOfPeople;
            existing.IsSalaryVisible = data.IsSalaryVisible;
            existing.JobTitle = data.JobTitle;
            existing.JobLevels = data.JobLevels;
            existing.Department = data.Department;
            existing.MainDuties = data.MainDuties;
            existing.AdditionalDuties = data.AdditionalDuties;
            existing.PlannedHiringDate = data.PlannedHiringDate;
            existing.Education = data.Education;
            existing.PreferredStudyFields = data.PreferredStudyFields;
            existing.AdditionalCertifications = data.AdditionalCertifications;
            existing.RequiredExperience = data.RequiredExperience;
            existing.PreferredExperience = data.PreferredExperience;
            existing.RequiredSkills = data.RequiredSkills;
            existing.PreferredSkills = data.PreferredSkills;
            existing.RequiredLanguages = data.RequiredLanguages;
            existing.PreferredLanguages = data.PreferredLanguages;
            existing.EmploymentsForms = data.EmploymentsForms;
            existing.GrossSalary = data.GrossSalary;
            existing.BonusSystem = data.BonusSystem;
            existing.AdditionalBenefits = data.AdditionalBenefits;
            existing.WorkTools = data.WorkTools;
            existing.WorkPlace = data.WorkPlace;
            existing.WorkModes = data.WorkModes;
            existing.WorkingHours = data.WorkingHours;
            existing.OtherRemarks = data.OtherRemarks;
        }

        private async Task CreateProjectFromCard(long projectCardId)
        {
            if (await _context.Projects.AnyAsync(p => p.ProjectCardId == projectCardId))
                return;

            var projectCard = await _context.ProjectCards
                .Include(pc => pc.Client)
                .FirstOrDefaultAsync(pc => pc.Id == projectCardId);
            if (projectCard == null) return;

            var newProject = new Project
            {
                ProjectCardId = projectCardId,
                ClientId = projectCard.ClientId,
                NumberOfPeople = projectCard.NumberOfPeople,
                JobLevels = projectCard.JobLevels,
                IsSalaryVisible = projectCard.IsSalaryVisible,
                JobTitle = projectCard.JobTitle,
                Department = projectCard.Department,
                MainDuties = projectCard.MainDuties,
                AdditionalDuties = projectCard.AdditionalDuties,
                PlannedHiringDate = projectCard.PlannedHiringDate,
                Education = projectCard.Education,
                PreferredStudyFields = projectCard.PreferredStudyFields,
                AdditionalCertifications = projectCard.AdditionalCertifications,
                RequiredExperience = projectCard.RequiredExperience,
                PreferredExperience = projectCard.PreferredExperience,
                RequiredSkills = projectCard.RequiredSkills,
                PreferredSkills = projectCard.PreferredSkills,
                RequiredLanguages = projectCard.RequiredLanguages,
                PreferredLanguages = projectCard.PreferredLanguages,
                EmploymentsForms = projectCard.EmploymentsForms,
                GrossSalary = projectCard.GrossSalary,
                BonusSystem = projectCard.BonusSystem,
                AdditionalBenefits = projectCard.AdditionalBenefits,
                WorkTools = projectCard.WorkTools,
                WorkPlace = projectCard.WorkPlace,
                WorkModes = projectCard.WorkModes,
                WorkingHours = projectCard.WorkingHours,
                OtherRemarks = projectCard.OtherRemarks,
                Status = ProjectStatus.InProgress,
                RecruiterId = projectCard.RecruiterId
            };
            await _context.Projects.AddAsync(newProject);

            var notification = new Notification
            {
                Title = "Nowy projekt utworzony",
                Message = $"Projekt '{newProject.JobTitle}' został automatycznie utworzony po akceptacji karty projektu.",
                CreatedAt = DateTime.Now,
                ProjectCardId = projectCardId,
                Tag = "project_created"
            };
            await _context.Notifications.AddAsync(notification);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy na pewno chcesz anulować wypełnianie formularza?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _mainFrame.GoBack();
            }
        }

        private void FillFormWithProjectData()
        {
            if (_project == null) return;

            NumberOfPeopleBox.Text = _project.NumberOfPeople > 0 ? _project.NumberOfPeople.ToString() : "";
            JobTitleBox.Text = _project.JobTitle ?? "";
            DepartmentBox.Text = _project.Department ?? "";
            MainDutiesBox.Text = _project.MainDuties ?? "";
            AdditionalDutiesBox.Text = _project.AdditionalDuties ?? "";
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

            if (_project.PlannedHiringDate.HasValue)
                PlannedHiringDatePicker.SelectedDate = _project.PlannedHiringDate;

            SetCheckBoxesFromCommaSeparated(JobLevelsPanel, _project.JobLevels);
            SetCheckBoxesFromCommaSeparated(EducationPanel, _project.Education);
            SetCheckBoxesFromCommaSeparated(EmploymentFormsPanel, _project.EmploymentsForms);
            SetCheckBoxesFromCommaSeparated(WorkModesPanel, _project.WorkModes);
            SetRadioButtonGroup(SalaryVisibilityPanel, _project.IsSalaryVisible ? "Tak" : "Nie");
            SetRadioButtonGroup(BonusPanel, _project.BonusSystem ? "Tak" : "Nie");
        }

        private void SetRadioButtonGroup(StackPanel panel, string value)
        {
            if (panel == null) return;
            foreach (var rb in panel.Children.OfType<RadioButton>())
            {
                rb.IsChecked = rb.Content.ToString() == value;
            }
        }

        private void SetCheckBoxesFromCommaSeparated(StackPanel panel, string values)
        {
            if (panel == null || string.IsNullOrWhiteSpace(values)) return;
            var list = values.Split(',').Select(s => s.Trim());
            foreach (var cb in panel.Children.OfType<CheckBox>())
            {
                cb.IsChecked = list.Contains(cb.Content.ToString());
            }
        }

        private void InitializeEventHandlers()
        {
            // Przykład obsługi zdarzeń do czyszczenia błędów
            NumberOfPeopleBox.TextChanged += (s, e) => ClearError(NumberOfPeopleError, NumberOfPeopleBox);
            JobTitleBox.TextChanged += (s, e) => ClearError(JobTitleError, JobTitleBox);
            DepartmentBox.TextChanged += (s, e) => ClearError(DepartmentError, DepartmentBox);
            MainDutiesBox.TextChanged += (s, e) => ClearError(MainDutiesError, MainDutiesBox);
            RequiredExperienceBox.TextChanged += (s, e) => ClearError(RequiredExperienceError, RequiredExperienceBox);
            RequiredSkillsBox.TextChanged += (s, e) => ClearError(RequiredSkillsError, RequiredSkillsBox);
            RequiredLanguagesBox.TextChanged += (s, e) => ClearError(RequiredLanguagesError, RequiredLanguagesBox);
            GrossSalaryBox.TextChanged += (s, e) => ClearError(GrossSalaryError, GrossSalaryBox);
            WorkPlaceBox.TextChanged += (s, e) => ClearError(WorkPlaceError, WorkPlaceBox);
            WorkingHoursBox.TextChanged += (s, e) => ClearError(WorkingHoursError, WorkingHoursBox);

            PlannedHiringDatePicker.SelectedDateChanged += (s, e) => ClearError(PlannedHiringDateError, PlannedHiringDatePicker);

            foreach (var rb in SalaryVisibilityPanel.Children.OfType<RadioButton>())
            {
                rb.Checked += (s, e) => ClearError(SalaryVisibilityError);
            }

            foreach (var rb in BonusPanel.Children.OfType<RadioButton>())
            {
                rb.Checked += (s, e) => ClearError(BonusError);
            }

            foreach (var cb in JobLevelsPanel.Children.OfType<CheckBox>())
            {
                cb.Checked += (s, e) => ClearError(JobLevelsError);
                cb.Unchecked += (s, e) =>
                {
                    if (!JobLevelsPanel.Children.OfType<CheckBox>().Any(c => c.IsChecked == true))
                        JobLevelsError.Visibility = Visibility.Visible;
                    else
                        JobLevelsError.Visibility = Visibility.Collapsed;
                };
            }

            foreach (var cb in EducationPanel.Children.OfType<CheckBox>())
            {
                cb.Checked += (s, e) => ClearError(EducationError);
                cb.Unchecked += (s, e) =>
                {
                    if (!EducationPanel.Children.OfType<CheckBox>().Any(c => c.IsChecked == true))
                        EducationError.Visibility = Visibility.Visible;
                    else
                        EducationError.Visibility = Visibility.Collapsed;
                };
            }

            foreach (var cb in EmploymentFormsPanel.Children.OfType<CheckBox>())
            {
                cb.Checked += (s, e) => ClearError(EmploymentFormsError);
                cb.Unchecked += (s, e) =>
                {
                    if (!EmploymentFormsPanel.Children.OfType<CheckBox>().Any(c => c.IsChecked == true))
                        EmploymentFormsError.Visibility = Visibility.Visible;
                    else
                        EmploymentFormsError.Visibility = Visibility.Collapsed;
                };
            }

            foreach (var cb in WorkModesPanel.Children.OfType<CheckBox>())
            {
                cb.Checked += (s, e) => ClearError(WorkModesError);
                cb.Unchecked += (s, e) =>
                {
                    if (!WorkModesPanel.Children.OfType<CheckBox>().Any(c => c.IsChecked == true))
                        WorkModesError.Visibility = Visibility.Visible;
                    else
                        WorkModesError.Visibility = Visibility.Collapsed;
                };
            }
        }

        private void ClearError(TextBlock errorBlock, Control control = null)
        {
            errorBlock.Visibility = Visibility.Collapsed;
            if (control != null)
            {
                control.ClearValue(TextBox.BorderBrushProperty);
                control.ClearValue(TextBox.BorderThicknessProperty);
                if (control is DatePicker dp)
                {
                    dp.ClearValue(DatePicker.BorderBrushProperty);
                }
            }
        }

        private bool ValidateRequiredFields()
        {
            bool isValid = true;
            // Ukryj wszystkie komunikaty
            HideAllErrors();

            // Walidacja pola liczba osób
            if (string.IsNullOrWhiteSpace(NumberOfPeopleBox.Text))
            {
                NumberOfPeopleError.Text = "Pole jest wymagane.";
                NumberOfPeopleError.Visibility = Visibility.Visible;
                NumberOfPeopleBox.BorderBrush = Brushes.Red;
                NumberOfPeopleBox.BorderThickness = new Thickness(1);
                isValid = false;
            }
            else if (!int.TryParse(NumberOfPeopleBox.Text, out int numPeople) || numPeople <= 0)
            {
                NumberOfPeopleError.Text = "Podaj liczbę większą od zera.";
                NumberOfPeopleError.Visibility = Visibility.Visible;
                NumberOfPeopleBox.BorderBrush = Brushes.Red;
                NumberOfPeopleBox.BorderThickness = new Thickness(1);
                isValid = false;
            }

            // Walidacja daty
            if (PlannedHiringDatePicker.SelectedDate == null)
            {
                PlannedHiringDateError.Text = "Wybierz datę.";
                PlannedHiringDateError.Visibility = Visibility.Visible;
                PlannedHiringDatePicker.BorderBrush = Brushes.Red;
                PlannedHiringDatePicker.BorderThickness = new Thickness(1);
                isValid = false;
            }
            else if (PlannedHiringDatePicker.SelectedDate < DateTime.Now)
            {
                PlannedHiringDateError.Text = "Data nie może być w przeszłości.";
                PlannedHiringDateError.Visibility = Visibility.Visible;
                PlannedHiringDatePicker.BorderBrush = Brushes.Red;
                PlannedHiringDatePicker.BorderThickness = new Thickness(1);
                isValid = false;
            }

            // Walidacja innych pól tekstowych
            ValidateTextBoxField(JobTitleBox, JobTitleError, "Nazwa stanowiska");
            ValidateTextBoxField(DepartmentBox, DepartmentError, "Dział");
            ValidateTextBoxField(MainDutiesBox, MainDutiesError, "Główne obowiązki");
            ValidateTextBoxField(RequiredExperienceBox, RequiredExperienceError, "Wymagane doświadczenie");
            ValidateTextBoxField(RequiredSkillsBox, RequiredSkillsError, "Wymagane umiejętności");
            ValidateTextBoxField(RequiredLanguagesBox, RequiredLanguagesError, "Wymagane języki");
            ValidateTextBoxField(GrossSalaryBox, GrossSalaryError, "Wynagrodzenie brutto");
            ValidateTextBoxField(WorkPlaceBox, WorkPlaceError, "Miejsce pracy");
            ValidateTextBoxField(WorkingHoursBox, WorkingHoursError, "Godziny pracy");

            // Walidacja grup radiobutton
            if (!IsRadioGroupChecked(SalaryVisibilityPanel))
            {
                SalaryVisibilityError.Visibility = Visibility.Visible;
                isValid = false;
            }
            if (!IsRadioGroupChecked(BonusPanel))
            {
                BonusError.Visibility = Visibility.Visible;
                isValid = false;
            }

            // Walidacja grup checkbox
            if (!IsAnyChecked(JobLevelsPanel))
            {
                JobLevelsError.Visibility = Visibility.Visible;
                isValid = false;
            }
            if (!IsAnyChecked(EducationPanel))
            {
                EducationError.Visibility = Visibility.Visible;
                isValid = false;
            }
            if (!IsAnyChecked(EmploymentFormsPanel))
            {
                EmploymentFormsError.Visibility = Visibility.Visible;
                isValid = false;
            }
            if (!IsAnyChecked(WorkModesPanel))
            {
                WorkModesError.Visibility = Visibility.Visible;
                isValid = false;
            }

            if (!isValid)
            {
                ScrollToFirstError();
            }

            return isValid;
        }

        private void ValidateTextBoxField(TextBox textBox, TextBlock errorBlock, string fieldName)
        {
            // Resetuj styl błędu
            errorBlock.Visibility = Visibility.Collapsed;
            textBox.ClearValue(Border.BorderBrushProperty);
            textBox.ClearValue(Border.BorderThicknessProperty);

            string text = textBox.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(text))
            {
                errorBlock.Text = $"Pole '{fieldName}' jest wymagane.";
                errorBlock.Visibility = Visibility.Visible;
                textBox.BorderBrush = Brushes.Red;
                textBox.BorderThickness = new Thickness(1);
            }
        }
        private void HideAllErrors()
        {
            var errorBlocks = new[]
            {
                NumberOfPeopleError, JobTitleError, DepartmentError, MainDutiesError,
                RequiredExperienceError, RequiredSkillsError, RequiredLanguagesError,
                GrossSalaryError, WorkPlaceError, WorkingHoursError,
                PlannedHiringDateError, SalaryVisibilityError, BonusError,
                JobLevelsError, EducationError, EmploymentFormsError, WorkModesError
            };

            foreach (var err in errorBlocks)
            {
                err.Visibility = Visibility.Collapsed;
            }

            // Czyszczenie podświetleń
            NumberOfPeopleBox.ClearValue(Border.BorderBrushProperty);
            JobTitleBox.ClearValue(Border.BorderBrushProperty);
            DepartmentBox.ClearValue(Border.BorderBrushProperty);
            MainDutiesBox.ClearValue(Border.BorderBrushProperty);
            RequiredExperienceBox.ClearValue(Border.BorderBrushProperty);
            RequiredSkillsBox.ClearValue(Border.BorderBrushProperty);
            RequiredLanguagesBox.ClearValue(Border.BorderBrushProperty);
            GrossSalaryBox.ClearValue(Border.BorderBrushProperty);
            WorkPlaceBox.ClearValue(Border.BorderBrushProperty);
            WorkingHoursBox.ClearValue(Border.BorderBrushProperty);
            PlannedHiringDatePicker.ClearValue(Border.BorderBrushProperty);
        }

        private void ScrollToFirstError()
        {
            var errors = new[]
            {
                NumberOfPeopleError, JobTitleError, DepartmentError, MainDutiesError,
                RequiredExperienceError, RequiredSkillsError, RequiredLanguagesError,
                GrossSalaryError, WorkPlaceError, WorkingHoursError,
                PlannedHiringDateError, SalaryVisibilityError, BonusError,
                JobLevelsError, EducationError, EmploymentFormsError, WorkModesError
            };
            var firstError = errors.FirstOrDefault(e => e.Visibility == Visibility.Visible);
            firstError?.BringIntoView();
        }

        private bool IsRadioGroupChecked(StackPanel panel)
        {
            return panel.Children.OfType<RadioButton>().Any(rb => rb.IsChecked == true);
        }

        private bool IsAnyChecked(StackPanel panel)
        {
            return panel.Children.OfType<CheckBox>().Any(cb => cb.IsChecked == true);
        }
    }
}