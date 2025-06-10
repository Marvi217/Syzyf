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

        private async void SaveProject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var jobLevels = GetCheckedValuesFromPanel("JobLevelsPanel");
                var education = GetCheckedValuesFromPanel("EducationPanel");
                var employmentForms = GetCheckedValuesFromPanel("EmploymentFormsPanel");
                var workModes = GetCheckedValuesFromPanel("WorkModesPanel");
                var salaryVsibility = GetCheckedValuesFromPanel("SalaryVisibilityPanel");
                var bonusSystem = GetCheckedValuesFromPanel("BonusPanel");

                bool salary = false;
                if (salaryVsibility != null) {
                    if (salaryVsibility.Contains("Tak"))
                    {
                        salary = true;
                    }
                    else if (salaryVsibility.Contains("Nie"))
                    {
                        salary = false;
                    }
                   
                }

                bool bonus = false;
                if (bonusSystem != null)
                {
                    if (bonusSystem.Contains("Tak"))
                    {
                        bonus = true;
                    }
                    else if (bonusSystem.Contains("Nie"))
                    {
                        bonus = false;
                    }

                }

                var project = new Project
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
                    Status = ProjectStatus.InProgress
                };

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                if (_notification != null)
                {
                    var response = new Notification
                    {
                        Title = "Karta projektu",
                        Message = $"Projekt '{project.JobTitle}' został zapisany przez użytkownika {_user.Login} w dniu {DateTime.Now:yyyy-MM-dd HH:mm}.",
                        FromId = _user.Id,
                        Tag = "fulfilled",
                        ToId = new List<long> { _notification.FromId },
                        IsRead = false
                    };
                    _context.Notifications.Update(response);
                    await _context.SaveChangesAsync();
                }



                MessageBox.Show("Projekt został zapisany pomyślnie.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                _mainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas zapisu projektu:\n{ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
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
                tb.Foreground = Brushes.Gray;
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
                tb.Foreground = Brushes.Gray;
                switch (tb.Name)
                {
                    case "NumberOfPeopleBox": tb.Text = "Liczba osób"; break;
                    case "JobTitleBox": tb.Text = "Nazwa stanowiska"; break;
                    case "DepartmentBox": tb.Text = "Dział"; break;
                    case "MainDutiesBox": tb.Text = "Główne obowiązki"; break;
                    case "AdditionalDutiesBox": tb.Text = "Dodatkowe obowiązki"; break;
                    case "PreferredStudyFieldsBox": tb.Text = "Preferowane kierunki studiów"; break;
                    case "AdditionalCertificationsBox": tb.Text = "Dodatkowe uprawnienia"; break;
                    case "RequiredExperienceBox": tb.Text = "Wymagane doświadczenie"; break;
                    case "PreferredExperienceBox": tb.Text = "Mile widziane doświadczenie"; break;
                    case "RequiredSkillsBox": tb.Text = "Wymagane umiejętności"; break;
                    case "PreferredSkillsBox": tb.Text = "Mile widziane umiejętności"; break;
                    case "RequiredLanguagesBox": tb.Text = "Wymagane języki"; break;
                    case "PreferredLanguagesBox": tb.Text = "Mile widziane języki"; break;
                    case "GrossSalaryBox": tb.Text = "Wynagrodzenie brutto"; break;
                    case "AdditionalBenefitsBox": tb.Text = "Dodatkowe benefity"; break;
                    case "WorkToolsBox": tb.Text = "Narzędzia pracy"; break;
                    case "WorkPlaceBox": tb.Text = "Miejsce pracy"; break;
                    case "WorkingHoursBox": tb.Text = "Godziny pracy"; break;
                    case "OtherRemarksBox": tb.Text = "Pozostałe informacje"; break;
                }
            }
        }
    }
}
