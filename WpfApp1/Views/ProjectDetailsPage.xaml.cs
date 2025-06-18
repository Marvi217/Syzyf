using MaterialDesignThemes.Wpf;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Cms;
using System;
using System.Linq;
using System.Threading.Tasks;
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
            HideAllActionButtons();

            if (_user?.EmployeeId == null)
            {
                InitializeUIForClient();
            }
            else
            {
                await InitializeUIForEmployee();
            }

            DisplayDetails();
        }

        private void HideAllActionButtons()
        {
            EditButton.Visibility = Visibility.Collapsed;
            RejectButton.Visibility = Visibility.Collapsed;
            RejectButton2.Visibility = Visibility.Collapsed;
            AcceptButton.Visibility = Visibility.Collapsed;
            AcceptButton2.Visibility = Visibility.Collapsed;
            AssignRecruiterButton.Visibility = Visibility.Collapsed;
        }

        private void InitializeUIForClient()
        {
            if (_projectForActions?.Status == ProjectStatus.InProgress)
            {
                return;
            }

            if (_projectData is ProjectCard card && card.Status == ProjectCardStatus.Canceled)
            {
                return;
            }

            if (_projectData is ProjectCard projectCard && projectCard.ProjectAcceptance?.AcceptedByClient == true)
            {
                return;
            }

            AcceptButton.Visibility = Visibility.Visible;
            FullRejectButton.Visibility = Visibility.Visible;
        }

        private async Task InitializeUIForEmployee()
        {
            if (_projectForActions?.Status == ProjectStatus.InProgress)
            {
                return;
            }

            var positionName = _user.Employee.Position.PositionName;
            ProjectAcceptance acceptance = null;
            if (_projectData is ProjectCard projectCard)
            {
                acceptance = await _context.ProjectAcceptances
                    .FirstOrDefaultAsync(pa => pa.ProjectCardId == projectCard.Id);
            }

            switch (positionName)
            {
                case "Handlowiec":
                    break;

                case "Rekruter":
                    HandleUIVisibilityForRecruiter(acceptance);
                    break;

                case "Wsparcie":
                    HandleUIVisibilityForSupport(acceptance);
                    break;

                case "Admin":
                    RejectButton.Visibility = Visibility.Visible;
                    break;
            }
        }

        // W pliku ProjectDetailsPage.xaml.cs

        // W pliku ProjectDetailsPage.xaml.cs

        private void HandleUIVisibilityForRecruiter(ProjectAcceptance acceptance)
        {
            if (acceptance?.AcceptedByRecruiter == true)
            {
                return;
            }

            if (_projectData is ProjectCard card)
            {
                if (card.RecruiterId == null)
                {
                    // Karta nie jest przypisana, rekruter może ją przyjąć lub odrzucić (zanim przyjmie)
                    AcceptButton2.Visibility = Visibility.Visible;
                    RejectButton2.Visibility = Visibility.Visible; // Użyjemy nowego przycisku RejectButton2
                }
                else
                {
                    // Karta jest przypisana. Rekruter może ją zaakceptować, edytować lub odrzucić.
                    if (_projectCardData == null || !_projectCardData.IsAcceptedDb)
                    {
                        AcceptButton.Visibility = Visibility.Visible;
                        EditButton.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void HandleUIVisibilityForSupport(ProjectAcceptance acceptance)
        {
            bool isAcceptedBySupport = acceptance?.AcceptedBySupport == true;
            bool recruiterAssigned = (_projectData as ProjectCard)?.RecruiterId != null;

            if (isAcceptedBySupport)
            {
                AssignRecruiterButton.Visibility = Visibility.Visible;
            }
            else
            {
                AcceptButton.Visibility = Visibility.Visible;
                RejectButton.Visibility = Visibility.Visible;
            }

            if (recruiterAssigned)
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

            // Krok 1: Zablokuj UI, aby zapobiec wielokrotnym kliknięciom
            AcceptButton.IsEnabled = false;

            // Używamy transakcji dla spójności danych
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Krok 2: Pobierz lub utwórz encję akceptacji
                var projectAcceptance = await GetOrCreateProjectAcceptanceAsync(card.Id);

                // Krok 3: Spróbuj zaktualizować status akceptacji na podstawie roli użytkownika
                if (!TryUpdateAcceptanceStatus(projectAcceptance))
                {
                    // Jeśli nie podjęto żadnej akcji (brak uprawnień lub już zaakceptowano)
                    MessageBox.Show("Nie masz uprawnień do akceptacji lub już zaakceptowałeś ten projekt.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                    await transaction.RollbackAsync(); // Wycofaj pustą transakcję
                    return;
                }

                // Krok 4: Zapisz wszystkie zmiany w jednej operacji i zatwierdź transakcję
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var projectAcceptance2 = await GetOrCreateProjectAcceptanceAsync(card.Id);

                if(projectAcceptance2.AcceptedBySupport && projectAcceptance2.AcceptedByRecruiter && projectAcceptance2.AcceptedByClient)
                {
                    card.IsAcceptedDb = true;
                    _context.ProjectCards.Update(card);
                    await _context.SaveChangesAsync();
                }

                // Krok 5: Obsłuż konsekwencje akceptacji (powiadomienia, tworzenie projektu)
                await HandleConsequencesOfAcceptanceAsync(card, projectAcceptance);

                // Krok 6: Poinformuj użytkownika i odśwież widok
                MessageBox.Show("Akceptacja zapisana.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
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
            finally
            {
                // Krok 7: Zawsze odblokuj UI na końcu
                AcceptButton.IsEnabled = true;
            }
        }


        private async Task<ProjectAcceptance> GetOrCreateProjectAcceptanceAsync(long cardId)
        {
            var projectAcceptance = await _context.ProjectAcceptances
                .FirstOrDefaultAsync(pa => pa.ProjectCardId == cardId);

            if (projectAcceptance == null)
            {
                projectAcceptance = new ProjectAcceptance { ProjectCardId = cardId };
                _context.ProjectAcceptances.Add(projectAcceptance);
            }
            return projectAcceptance;
        }

        private bool TryUpdateAcceptanceStatus(ProjectAcceptance projectAcceptance)
        {
            var positionName = _user?.Employee?.Position?.PositionName;

            if (positionName == "Wsparcie" && !projectAcceptance.AcceptedBySupport)
            {
                projectAcceptance.SupportId = _user.Id;
                projectAcceptance.AcceptedBySupport = true;
                projectAcceptance.SupportAcceptedAt = DateTime.Now;
                return true;
            }

            if (positionName == "Rekruter" && !projectAcceptance.AcceptedByRecruiter)
            {
                projectAcceptance.AcceptedByRecruiter = true;
                projectAcceptance.RecruiterAcceptedAt = DateTime.Now;
                return true;
            }

            if (_user?.ClientId != null && !projectAcceptance.AcceptedByClient)
            {
                projectAcceptance.AcceptedByClient = true;
                projectAcceptance.ClientAcceptedAt = DateTime.Now;
                return true;
            }

            return false;
        }

        private async Task HandleConsequencesOfAcceptanceAsync(ProjectCard card, ProjectAcceptance acceptance)
        {
            // 1. Wyślij powiadomienie, jeśli akceptacji dokonało "Wsparcie"
            if (_user?.Employee?.Position?.PositionName == "Wsparcie")
            {
                card.Status = ProjectCardStatus.Processed;
                _context.ProjectCards.Update(card);

                var clientUserId = await _context.Clients
                    .Where(c => c.Id == card.ClientId)
                    .Select(c => c.User.Id)
                    .FirstOrDefaultAsync();

                if (clientUserId != 0)
                {
                    var message = $"Dzień dobry,\n\nKarta projektu {card.JobTitle} została wstępnie zaakceptowana. Proszę czekać na dalsze powiadomienia.";
                    _context.Notifications.Add(new Notification
                    {
                        FromId = _user.Id,
                        ToId = clientUserId,
                        Title = "Karta Projektu",
                        Tag = "firstAccepted",
                        Message = message,
                        IsRead = false,
                        CreatedAt = DateTime.Now
                    });
                }
            }

            // Musimy odświeżyć stan karty z bazy, aby mieć pewność, że `IsAccepted` jest aktualne
            var cardInDb = await _context.ProjectCards
                .Include(c => c.ProjectAcceptance)
                .AsNoTracking() // Używamy AsNoTracking, aby nie powodować konfliktu ze śledzonym `card`
                .FirstOrDefaultAsync(c => c.Id == card.Id);

            // Jeśli karta jest w pełni zaakceptowana (po naszej zmianie), utwórz projekt
            if (cardInDb != null && cardInDb.IsAcceptedDb)
            {
                card.IsAcceptedDb = true;
                card.Status = ProjectCardStatus.Accepted;
                _context.ProjectCards.Update(card);

                bool projectExists = await _context.Projects.AnyAsync(p => p.ProjectCardId == card.Id);
                if (!projectExists)
                {
                    // Poniżej znajduje się pełne mapowanie, nic nie zostało pominięte
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
                }
            }
        }

        private async void Accept_Click2(object sender, RoutedEventArgs e)
        {
            if (_user?.EmployeeId == null || _projectData is not ProjectCard card) return;

            AcceptButton2.IsEnabled = false;
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                card.RecruiterId = _user.EmployeeId;
                _context.ProjectCards.Update(card);

                var message = $"Dzień dobry, jestem {_user.Employee.FirstName} {_user.Employee.LastName}. Będę prowadził tę kartę projektu: {card.JobTitle}.";

                // Powiadomienie do klienta - bez zmian
                var clientNotification = new Notification
                {
                    FromId = _user.Id,
                    ToId = card.Client.User.Id, // Zakładam, że card.Client.User jest załadowane
                    Title = "Przydzielenie karty projektu",
                    Message = message,
                    Tag = "projectAssignmentReplay",
                    ProjectCardId = card.Id,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                };
                _context.Notifications.Add(clientNotification);

                // --- POPRAWIONE ZAPYTANIE Z UŻYCIEM JOIN ---
                var supportUser = await (
                    from n in _context.Notifications
                    join u in _context.Users on n.FromId equals u.Id
                    join emp in _context.Employees on u.EmployeeId equals emp.Id
                    join p in _context.Positions on emp.PositionId equals p.Id
                    where
                        n.ProjectCardId == card.Id &&
                        p.PositionName == "Wsparcie"
                    select u // Wybierz cały obiekt User
                ).FirstOrDefaultAsync();
                // ------------------------------------------

                if (supportUser != null)
                {
                    var supportNotification = new Notification
                    {
                        FromId = _user.Id,
                        ToId = supportUser.Id, // Teraz mamy poprawne ID
                        Title = "Przydzielenie karty projektu",
                        Message = message,
                        Tag = "projectAssignmentReplay",
                        ProjectCardId = card.Id,
                        IsRead = false,
                        CreatedAt = DateTime.Now
                    };
                    _context.Notifications.Add(supportNotification);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                MessageBox.Show("Przyjęto kartę projektu.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                _mainFrame.GoBack();

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
            finally
            {
                AcceptButton2.IsEnabled = true;
            }
        }

        private void RefreshUIAfterAcceptance(ProjectAcceptance acceptance)
        {
            AcceptButton.Visibility = Visibility.Collapsed;
            RejectButton.Visibility = Visibility.Collapsed;

            var acceptedText = new TextBlock
            {
                Text = $"Zaakceptowano: {DateTime.Now:g}", // Użyj DateTime.Now dla spójności
                Foreground = System.Windows.Media.Brushes.Green,
                Margin = new Thickness(10, 5, 0, 0),
                FontWeight = FontWeights.Bold
            };
            DetailsContainer.Children.Add(acceptedText);

            // Pokaż przycisk do przydzielenia rekrutera tylko jeśli akceptacja była przez Wsparcie
            if (_user?.Employee?.Position?.PositionName == "Wsparcie")
            {
                AssignRecruiterButton.Visibility = Visibility.Visible;
            }
        }

        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            // Upewnij się, że mamy do czynienia z ProjectCard, a nie z Project
            if (_projectData is ProjectCard card)
            {
                // Przekaż wszystkie potrzebne dane do nowej strony
                _mainFrame.Navigate(new RejectionReasonPage(_mainFrame, _user, _context, card));
            }
            else
            {
                MessageBox.Show("Ta akcja jest dostępna tylko dla kart projektu, a nie dla aktywnych projektów.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // W pliku ProjectDetailsPage.xaml.cs

        private async void Reject_Click2(object sender, RoutedEventArgs e)
        {
            // Upewnij się, że mamy do czynienia z kartą projektu
            if (_projectData is not ProjectCard card) return;

            // Pytanie o potwierdzenie
            var result = MessageBox.Show("Czy na pewno chcesz odrzucić przyjęcie tej karty projektu? Zostanie ona zwrócona do pracownika wsparcia.",
                                        "Potwierdzenie odrzucenia",
                                        MessageBoxButton.YesNo,
                                        MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                return;
            }

            // Zablokuj przyciski na czas operacji
            RejectButton2.IsEnabled = false;
            AcceptButton2.IsEnabled = false;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Znajdź pracownika wsparcia, który ostatnio wysłał powiadomienie o tej karcie.
                // Zakładamy, że to on ją przydzielił.
                var supportUser = await (
                    from n in _context.Notifications
                    join u in _context.Users on n.FromId equals u.Id
                    join emp in _context.Employees on u.EmployeeId equals emp.Id
                    where
                        n.ProjectCardId == card.Id &&
                        emp.Position.PositionName == "Wsparcie"
                    orderby n.CreatedAt descending
                    select u
                ).FirstOrDefaultAsync();

                if (supportUser == null)
                {
                    MessageBox.Show("Nie można zidentyfikować pracownika wsparcia, który przydzielił tę kartę. Skontaktuj się z administratorem.", "Błąd krytyczny", MessageBoxButton.OK, MessageBoxImage.Error);
                    await transaction.RollbackAsync();
                    return;
                }

                // 2. Utwórz powiadomienie zwrotne do pracownika wsparcia
                var rejectionMessage = $"Rekruter {_user.Employee.FirstName} {_user.Employee.LastName} odrzucił przyjęcie karty projektu '{card.JobTitle}'. Karta wymaga ponownego przydzielenia.";
                var notification = new Notification
                {
                    FromId = _user.Id,
                    ToId = supportUser.Id,
                    Title = "Karta projektu została odrzucona przez rekrutera",
                    Message = rejectionMessage,
                    Tag = "recruiter_rejection", // Nowy, unikalny tag
                    ProjectCardId = card.Id,
                    IsRead = false,
                    CreatedAt = DateTime.Now
                };
                _context.Notifications.Add(notification);

                // 3. Zmień status karty i usuń przypisanie rekrutera
                card.RecruiterId = null; // Usuwamy przypisanie
                card.Status = ProjectCardStatus.Pending; // Wracamy do statusu oczekującego na przydział
                _context.ProjectCards.Update(card);

                // 4. Zapisz zmiany
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                MessageBox.Show("Karta projektu została odrzucona i zwrócona do pracownika wsparcia.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                // Wróć do poprzedniej strony
                _mainFrame.GoBack();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                MessageBox.Show($"Wystąpił błąd podczas odrzucania karty: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                RejectButton2.IsEnabled = true;
                AcceptButton2.IsEnabled = true;
            }
        }

        private async void Reject_Click_Full(object sender, RoutedEventArgs e)
        {
            if (_projectData is not ProjectCard card)
            {
                MessageBox.Show("Ta akcja jest dostępna tylko dla kart projektu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Krok 1: Wyświetl dodatkowe, poważne ostrzeżenie
            var confirmationResult = MessageBox.Show(
                "Czy na pewno chcesz całkowicie zrezygnować z tego projektu? Karta projektu zostanie anulowana i nie będzie można wznowić prac. Ta akcja jest nieodwracalna.",
                "Potwierdzenie Anulowania Projektu",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirmationResult == MessageBoxResult.No)
            {
                return; // Użytkownik się rozmyślił
            }

            // Zablokuj przycisk na czas operacji
            var button = sender as Button;
            if (button != null) button.IsEnabled = false;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Krok 2: Zmień status karty projektu
                card.Status = ProjectCardStatus.Canceled; // Użyjmy statusu "Anulowana"
                _context.ProjectCards.Update(card);

                // Krok 3: Przygotuj listę odbiorców powiadomienia
                var recipients = new List<long>();

                // a) Dodaj przypisanego rekrutera, jeśli istnieje
                if (card.RecruiterId.HasValue)
                {
                    var recruiterUser = await _context.Users.FirstOrDefaultAsync(u => u.EmployeeId == card.RecruiterId.Value);
                    if (recruiterUser != null)
                    {
                        recipients.Add(recruiterUser.Id);
                    }
                }

                // Krok 4: Wyślij powiadomienia do wszystkich zidentyfikowanych odbiorców
                var message = $"Klient '{card.Client.Company}' całkowicie zrezygnował z dalszych prac nad kartą projektu dla stanowiska: '{card.JobTitle}'. Projekt został anulowany.";

                foreach (var recipientId in recipients)
                {
                    var notification = new Notification
                    {
                        FromId = _user.Id,
                        ToId = recipientId,
                        Title = "Projekt anulowany przez klienta",
                        Message = message,
                        Tag = "project_canceled", // Nowy, unikalny tag
                        ProjectCardId = card.Id,
                        IsRead = false,
                        CreatedAt = DateTime.Now
                    };
                    _context.Notifications.Add(notification);
                }

                // Krok 5: Zapisz wszystkie zmiany
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                MessageBox.Show("Projekt został pomyślnie anulowany.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);

                // Odśwież UI, aby ukryć wszystkie przyciski
                InitializeUI();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                MessageBox.Show($"Wystąpił błąd podczas anulowania projektu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (button != null) button.IsEnabled = true;
            }
        }

        private void AssignRecruiterButton_Click(object sender, RoutedEventArgs e)
        {
            _mainFrame.Navigate(new AssignRecruiterPage(_mainFrame, _user, _context, _projectData));
        }

        private void EditProject_Click(object sender, RoutedEventArgs e)
        {
            if (_user.EmployeeId != null)
            {
                if (_projectData.RecruiterId == _user.EmployeeId || _user.Employee.Position.PositionName == "Admin")
                {
                    _mainFrame.Navigate(new ProjectCardFormPage(_mainFrame, _user, _context, _projectCardData));
                    return;
                }
            }
            else if (_user.ClientId != null)
            {
                _mainFrame.Navigate(new ProjectCardFormPage(_mainFrame, _user, _context, _projectCardData));
                return;
            }

            MessageBox.Show("Nie masz uprawnień do edycji tego projektu.",
                            "Brak uprawnień", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}