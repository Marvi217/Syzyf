using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Cms;
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
        private readonly ProjectCard _projectCardData;
        private readonly Project _projectForActions;

        public ProjectDetailsPage(Frame mainFrame, User user, SyzyfContext context, ProjectCard card)
        {
            InitializeComponent();

            _mainFrame = mainFrame;
            _user = user;
            _context = context;
            _projectData = card;
            _projectCardData = card;
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

        private async void InitializeUI()
        {
            TopMenu.Initialize(_mainFrame, _user);

            if (_user?.EmployeeId == null)
            {
                if (_projectForActions != null && _projectForActions.Status.Equals(ProjectStatus.InProgress))
                {
                    EditButton.Visibility = Visibility.Collapsed;
                    RejectButton.Visibility = Visibility.Collapsed;
                    AcceptButton.Visibility = Visibility.Collapsed;
                    AcceptButton2.Visibility = Visibility.Collapsed;
                    AssignRecruiterButton.Visibility = Visibility.Collapsed;
                }
                else if (_projectData is ProjectCard projectCard && projectCard.ProjectAcceptance?.AcceptedByClient == true)
                {
                    AcceptButton2.Visibility = Visibility.Collapsed;
                    AcceptButton.Visibility = Visibility.Collapsed;
                    RejectButton.Visibility = Visibility.Collapsed;
                    EditButton.Visibility = Visibility.Collapsed;
                    AssignRecruiterButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    AcceptButton2.Visibility = Visibility.Collapsed;
                    AcceptButton.Visibility = Visibility.Visible;
                    RejectButton.Visibility = Visibility.Collapsed;
                    EditButton.Visibility = Visibility.Collapsed;
                    AssignRecruiterButton.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                var positionName = _user.Employee.Position.PositionName;

                if (positionName == "Handlowiec")
                {
                    AcceptButton.Visibility = Visibility.Collapsed;
                    RejectButton.Visibility = Visibility.Collapsed;
                    EditButton.Visibility = Visibility.Collapsed;
                }
                else if (positionName == "Rekruter")
                {
                    if (_projectData is ProjectCard recruiterCard && recruiterCard.RecruiterId == null)
                    {
                        AcceptButton2.Visibility = Visibility.Visible;
                        AcceptButton.Visibility = Visibility.Collapsed;
                        EditButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        AcceptButton.Visibility = Visibility.Visible;
                        AcceptButton2.Visibility = Visibility.Collapsed;
                        if (_projectCardData != null && _projectCardData.IsAcceptedDb)
                        {
                            EditButton.Visibility = Visibility.Collapsed;
                            RejectButton.Visibility = Visibility.Collapsed;
                        }
                        else if (_projectCardData != null && !_projectCardData.IsAcceptedDb)
                        {
                            EditButton.Visibility = Visibility.Visible;
                            RejectButton.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            EditButton.Visibility = Visibility.Collapsed;
                            RejectButton.Visibility = Visibility.Collapsed;
                        }

                    }
                    AssignRecruiterButton.Visibility = Visibility.Collapsed;
                }
                else
                {
                    EditButton.Visibility = Visibility.Collapsed;
                    AcceptButton2.Visibility = Visibility.Collapsed;
                }

                bool isAcceptedBySupport = false;
                bool isAcceptedByRecruiter = false;
                if (_projectData is ProjectCard projectCard)
                {
                    var acceptance = await _context.ProjectAcceptances
                        .FirstOrDefaultAsync(pa => pa.ProjectCardId == projectCard.Id);

                    isAcceptedBySupport = acceptance?.AcceptedBySupport == true;
                    isAcceptedByRecruiter = acceptance?.AcceptedByRecruiter == true;

                    bool recruiterAssigned = projectCard.RecruiterId != null && projectCard.RecruiterId != 0;

                    if (recruiterAssigned && _user.Employee.Position.PositionName == "Wsparcie")
                    {
                        var assignedText = new TextBlock
                        {
                            Text = "Rekruter przydzielony",
                            Foreground = System.Windows.Media.Brushes.Green,
                            FontWeight = FontWeights.Bold,
                            Margin = new Thickness(0, 10, 0, 0)
                        };
                        DetailsContainer.Children.Add(assignedText);
                    }
                    else
                    {
                        EditButton.Visibility = Visibility.Collapsed;
                    }
                }

                if (isAcceptedBySupport && positionName == "Wsparcie")
                {
                    AcceptButton.Visibility = Visibility.Collapsed;
                    RejectButton.Visibility = Visibility.Collapsed;
                    AssignRecruiterButton.Visibility = Visibility.Visible;
                }
                else
                {
                    AssignRecruiterButton.Visibility = Visibility.Collapsed;
                }
                if (_projectCardData != null && isAcceptedByRecruiter == true)
                {
                    AcceptButton2.Visibility = Visibility.Collapsed;
                    AcceptButton.Visibility = Visibility.Collapsed;
                    EditButton.Visibility = Visibility.Collapsed;

                }
                else if (_projectCardData != null && isAcceptedByRecruiter == false)
                {
                    EditButton.Visibility = Visibility.Visible;
                }
                if (_projectForActions != null && _projectForActions.Status.Equals(ProjectStatus.InProgress))
                {
                    EditButton.Visibility = Visibility.Collapsed;
                    RejectButton.Visibility = Visibility.Collapsed;
                    AcceptButton.Visibility = Visibility.Collapsed;
                    AcceptButton2.Visibility = Visibility.Collapsed;
                }
                RejectButton.Visibility = _user.Employee.Position.PositionName == "Admin" ? Visibility.Visible : Visibility.Collapsed;
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
            AddTextBlock("Planowana data zatrudnienia:",
                _projectData.PlannedHiringDate?.ToShortDateString() ?? "Brak danych"
            );



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
            if (_projectData is not ProjectCard card) return;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Załaduj ProjectAcceptance z bazy lub utwórz nowy
                var projectAcceptance = await _context.ProjectAcceptances
                    .FirstOrDefaultAsync(pa => pa.ProjectCardId == card.Id);

                if (projectAcceptance == null)
                {
                    projectAcceptance = new ProjectAcceptance
                    {
                        ProjectCardId = card.Id
                    };
                    _context.ProjectAcceptances.Add(projectAcceptance);
                    await _context.SaveChangesAsync();
                }

                // Ustal rolę użytkownika i ustaw odpowiednią akceptację
                var positionName = _user?.Employee?.Position?.PositionName;

                if (positionName == "Wsparcie" && !projectAcceptance.AcceptedBySupport)
                {
                    projectAcceptance.SupportId = _user.Id;
                    projectAcceptance.AcceptedBySupport = true;
                    projectAcceptance.SupportAcceptedAt = DateTime.Now;
                }
                else if (positionName == "Rekruter" && !projectAcceptance.AcceptedByRecruiter)
                {
                    projectAcceptance.AcceptedByRecruiter = true;
                    projectAcceptance.RecruiterAcceptedAt = DateTime.Now;
                }
                else if (_user?.ClientId != null && !projectAcceptance.AcceptedByClient)
                {
                    projectAcceptance.AcceptedByClient = true;
                    projectAcceptance.ClientAcceptedAt = DateTime.Now;
                }
                else
                {
                    MessageBox.Show("Nie masz uprawnień do akceptacji lub już zaakceptowałeś ten projekt.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                _context.ProjectAcceptances.Update(projectAcceptance);
                await _context.SaveChangesAsync();

                // Jeśli akceptacja przez Wsparcie to wyślij powiadomienie do klienta
                if (positionName == "Wsparcie")
                {
                    var clientUserId = await _context.Clients
                        .Where(c => c.Id == card.ClientId)
                        .Select(c => c.User.Id)
                        .FirstOrDefaultAsync();

                    if (clientUserId != 0)
                    {
                        var message = $"Dzień dobry,\n\nKarta projektu {card.JobTitle} została wstępnie zaakceptowana przez pracowników (wsparcia). \nProszę czekać na dalsze powiadomienia związane z jej zatwierdzeniem.";
                        var notification = new Notification
                        {
                            FromId = _user.Id,
                            ToId = clientUserId,
                            Title = "Karta Projektu",
                            Tag = "firstAccepted",
                            Message = message,
                            IsRead = false,
                            CreatedAt = DateTime.Now
                        };
                        _context.Notifications.Add(notification);
                        card.Status = ProjectCardStatus.Processed;
                        _context.ProjectCards.Update(card);
                        await _context.SaveChangesAsync();
                    }
                }

                // Przeładuj kartę projektu z bazy, aby mieć aktualne dane
                var cardInDb = await _context.ProjectCards
                    .Include(c => c.ProjectAcceptance)
                    .FirstOrDefaultAsync(c => c.Id == card.Id);

                if (cardInDb == null)
                {
                    MessageBox.Show("Karta projektu nie została odnaleziona.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Sprawdź czy karta jest w pełni zaakceptowana
                if (cardInDb.IsAccepted)
                {
                    cardInDb.IsAcceptedDb = true;
                    cardInDb.Status = ProjectCardStatus.Accepted;
                    _context.ProjectCards.Update(cardInDb);

                    bool projectExists = await _context.Projects
                        .AnyAsync(p => p.ClientId == cardInDb.ClientId && p.JobTitle == cardInDb.JobTitle && p.RecruiterId == cardInDb.RecruiterId);

                    if (!projectExists)
                    {
                        var newProject = new Project
                        {
                            ClientId = cardInDb.ClientId,
                            NumberOfPeople = cardInDb.NumberOfPeople,
                            JobLevels = cardInDb.JobLevels,
                            IsSalaryVisible = cardInDb.IsSalaryVisible,
                            JobTitle = cardInDb.JobTitle,
                            Department = cardInDb.Department,
                            MainDuties = cardInDb.MainDuties,
                            AdditionalDuties = cardInDb.AdditionalDuties,
                            DevelopmentOpportunities = cardInDb.DevelopmentOpportunities,
                            PlannedHiringDate = cardInDb.PlannedHiringDate,
                            Education = cardInDb.Education,
                            PreferredStudyFields = cardInDb.PreferredStudyFields,
                            AdditionalCertifications = cardInDb.AdditionalCertifications,
                            RequiredExperience = cardInDb.RequiredExperience,
                            PreferredExperience = cardInDb.PreferredExperience,
                            RequiredSkills = cardInDb.RequiredSkills,
                            PreferredSkills = cardInDb.PreferredSkills,
                            RequiredLanguages = cardInDb.RequiredLanguages,
                            PreferredLanguages = cardInDb.PreferredLanguages,
                            EmploymentsForms = cardInDb.EmploymentsForms,
                            GrossSalary = cardInDb.GrossSalary,
                            BonusSystem = cardInDb.BonusSystem,
                            AdditionalBenefits = cardInDb.AdditionalBenefits,
                            WorkTools = cardInDb.WorkTools,
                            WorkPlace = cardInDb.WorkPlace,
                            WorkModes = cardInDb.WorkModes,
                            WorkingHours = cardInDb.WorkingHours,
                            OtherRemarks = cardInDb.OtherRemarks,
                            Status = ProjectStatus.InProgress,
                            RecruiterId = cardInDb.RecruiterId,
                            ProjectCardId = cardInDb.Id
                        };

                        _context.Projects.Add(newProject);
                        await _context.SaveChangesAsync();
                    }
                }

                await transaction.CommitAsync();

                MessageBox.Show("Akceptacja zapisana.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                // Odśwież widok lub UI w zależności od potrzeb (np. ukryj przyciski i pokaż status)
                RefreshUIAfterAcceptance(projectAcceptance);
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                MessageBox.Show("Dane zostały zmienione przez innego użytkownika. Proszę odświeżyć i spróbować ponownie.", "Konflikt danych", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                MessageBox.Show($"Błąd podczas zapisu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Accept_Click2(object sender, RoutedEventArgs e)
        {
            if (_user?.EmployeeId == null || _projectData is not ProjectCard card) return;

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                card.RecruiterId = _user.EmployeeId;
                _context.ProjectCards.Update(card);

                var message = $"Dzień dobry, jestem {_user.Employee.FirstName} {_user.Employee.LastName}. Będę prowadził tę kartę projektu: {card.JobTitle}.";
                var notification = new Notification
                {
                    FromId = _user.Id,
                    ToId = card.Client.User.Id,
                    Title = "Przydzielenie karty projektu",
                    Message = message,
                    Tag = "projectAssignmentReplay",
                    ProjectCardId = card.Id,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                };
                _context.Notifications.Add(notification);
                var users = await (
                    from u in _context.Users
                    join n in _context.Notifications
                        on u.EmployeeId equals n.FromId
                    where n.ProjectCardId == card.Id && u.EmployeeId != null
                    select u
                )
                .Include(u => u.Employee)
                .FirstOrDefaultAsync();

                var notification2 = new Notification
                {
                    FromId = _user.Id,
                    ToId = users.Id,
                    Title = "Przydzielenie karty projektu",
                    Message = message,
                    Tag = "projectAssignmentReplay",
                    ProjectCardId = card.Id,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                };
                _context.Notifications.Add(notification2);
                await _context.SaveChangesAsync();

                _mainFrame.GoBack();

                await transaction.CommitAsync();

                MessageBox.Show("Przyjęto kartę projektu.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                MessageBox.Show("Dane zostały zmienione przez innego użytkownika. Proszę odświeżyć i spróbować ponownie.", "Konflikt danych", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                MessageBox.Show($"Błąd podczas zapisu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshUIAfterAcceptance(ProjectAcceptance acceptance)
        {
            // Ukryj przyciski Akceptuj/Odrzuć
            AcceptButton.Visibility = Visibility.Collapsed;
            RejectButton.Visibility = Visibility.Collapsed;

            // Pokaż zielony napis "Zaakceptowano" z timestampem
            var acceptedText = new TextBlock
            {
                Text = $"Zaakceptowano: {acceptance.SupportAcceptedAt?.ToString("g")}",
                Foreground = System.Windows.Media.Brushes.Green,
                Margin = new Thickness(10, 5, 0, 0),
                FontWeight = FontWeights.Bold
            };
            DetailsContainer.Children.Add(acceptedText);

            // Pokaż przycisk do przydzielenia rekrutera
            AssignRecruiterButton.Visibility = Visibility.Visible;
            AssignRecruiterButton.Click += (s, e) =>
            {
                // Przekierowanie do strony przydzielania rekrutera, np.
                _mainFrame.Navigate(new AssignRecruiterPage(_mainFrame, _user, _context, _projectData));
            };
        }


        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Projekt został odrzucony.");
        }

        private void AssignRecruiterButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new AssignRecruiterPage(_mainFrame, _user, _context, _projectData));
        }
        private void EditProject_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                long employeeId = _user.EmployeeId ?? 0;
                long isClient = _user.ClientId ?? 0;
                if (employeeId != 0)
                {
                    if (_projectData.RecruiterId == _user.EmployeeId ||_user.Employee.Position.PositionName == "Admin")
                    {
                        _mainFrame.Navigate(new ProjectCardFormPage(_mainFrame, _user, _context, _projectCardData));
                    }
                }
                else if(isClient != 0)
                {
                    _mainFrame.Navigate(new ProjectCardFormPage(_mainFrame, _user, _context, _projectCardData));
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
