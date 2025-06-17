using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.Data;
using WpfApp1.Model;
using WpfApp1.Models;

namespace WpfApp1.Views
{
    public partial class MeetingPage : Page
    {
        public readonly Frame _mainFrame;
        public readonly User _user;
        public readonly SyzyfContext _context;
        public DateTime _currentWeekStart;
        public List<Meeting> _weeklyMeetings;
        public List<ParticipantItem> _availableParticipants;

        // Kolory dla różnych typów spotkań
        public readonly Brush[] _meetingColors =
        {
            new SolidColorBrush(Color.FromRgb(98, 100, 167)),  // Fioletowy
            new SolidColorBrush(Color.FromRgb(0, 120, 212)),   // Niebieski
            new SolidColorBrush(Color.FromRgb(16, 124, 16)),   // Zielony
            new SolidColorBrush(Color.FromRgb(196, 89, 17)),   // Pomarańczowy
            new SolidColorBrush(Color.FromRgb(164, 38, 44))    // Czerwony
        };

        public MeetingPage(Frame mainFrame, User user, DateTime selectedDate, SyzyfContext context)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _user = user;
            _context = context;

            TopMenu.Initialize(_mainFrame, _user);

            // Ustaw aktualny tydzień na podstawie wybranej daty
            _currentWeekStart = GetWeekStart(selectedDate);
            SideCalendar.SelectedDate = selectedDate;
            if (_user.Employee == null)
            {
                AddMeetingButton.Visibility = Visibility.Collapsed;
            }
            LoadAvailableParticipants();
            UpdateCalendarView();
        }

        public void LoadAvailableParticipants()
        {
            _availableParticipants = new List<ParticipantItem>();

            // Add employees
            var employees = _context.Employees.ToList();
            _availableParticipants.AddRange(employees.Select(e => new ParticipantItem
            {
                Id = e.Id,
                Name = $"{e.FirstName} {e.LastName}",
                Type = "(Pracownik)",
                Entity = e
            }));

            // Add candidates
            var candidates = _context.Candidates.ToList();
            _availableParticipants.AddRange(candidates.Select(c => new ParticipantItem
            {
                Id = c.Id,
                Name = $"{c.FirstName} {c.LastName}",
                Type = "(Kandydat)",
                Entity = c
            }));

            // Add clients
            var clients = _context.Clients.ToList();
            _availableParticipants.AddRange(clients.Select(c => new ParticipantItem
            {
                Id = c.Id,
                Name = c.Company,
                Type = "(Klient)",
                Entity = c
            }));

            ParticipantsListBox.ItemsSource = _availableParticipants;
        }

        private static DateTime GetWeekStart(DateTime date)
        {
            int daysFromMonday = ((int)date.DayOfWeek - 1 + 7) % 7;
            return date.Date.AddDays(-daysFromMonday);
        }

        public void UpdateCalendarView()
        {
            LoadWeeklyMeetings();
            UpdateMonthYearDisplay();
            BuildCalendarGrid();
            AddMeetingsToGrid();
        }

        public void LoadWeeklyMeetings()
        {
            DateTime weekEnd = _currentWeekStart.AddDays(7).AddTicks(-1);

            long? employeeId = _user.Employee?.Id;
            long? candidateId = _user.Candidate?.Id;
            long? clientId = _user.Client?.Id;

            _weeklyMeetings = _context.Meetings
                .Include(m => m.Participants)
                    .ThenInclude(p => p.Employee)
                .Include(m => m.Participants)
                    .ThenInclude(p => p.Candidate)
                .Include(m => m.Participants)
                    .ThenInclude(p => p.Client)
                .Where(m => m.StartTime >= _currentWeekStart && m.StartTime <= weekEnd
                            && m.Participants.Any(p =>
                                (employeeId != null && p.EmployeeId == employeeId) ||
                                (candidateId != null && p.CandidateId == candidateId) ||
                                (clientId != null && p.ClientId == clientId)
                            ))
                .OrderBy(m => m.StartTime)
                .ToList();
        }


        public void UpdateMonthYearDisplay()
        {
            var culture = new CultureInfo("pl-PL");
            string monthName = _currentWeekStart.ToString("MMMM yyyy", culture);
            CurrentMonthYear.Text = char.ToUpper(monthName[0]) + monthName.Substring(1);
        }

        public void BuildCalendarGrid()
        {
            CalendarGrid.Children.Clear();
            CalendarGrid.RowDefinitions.Clear();

            // Dodaj osobny wiersz na nagłówki dni tygodnia
            CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Dodaj wiersze dla godzin (8:00 - 18:00)
            for (int hour = 8; hour <= 18; hour++)
            {
                CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
            }

            // Nagłówki dni tygodnia
            var daysOfWeek = new[] { "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota", "Niedziela" };
            for (int i = 0; i < daysOfWeek.Length; i++)
            {
                var dayHeader = new TextBlock
                {
                    Text = daysOfWeek[i],
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5),
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Colors.DarkSlateGray)
                };
                Grid.SetColumn(dayHeader, i + 1);  // Kolumny 1-7
                Grid.SetRow(dayHeader, 0);        // Wiersz 0 (nagłówki)
                CalendarGrid.Children.Add(dayHeader);
            }

            // Etykiety godzin i komórki kalendarza
            for (int hour = 8; hour <= 18; hour++)
            {
                // Etykieta godziny (lewa kolumna)
                var timeLabel = new TextBlock
                {
                    Text = $"{hour:00}:00",
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 12,
                    Foreground = new SolidColorBrush(Colors.Gray),
                    Margin = new Thickness(5, 10, 5, 0)
                };
                Grid.SetColumn(timeLabel, 0);
                Grid.SetRow(timeLabel, hour - 7);  // Wiersze 1-11
                CalendarGrid.Children.Add(timeLabel);

                // Komórki kalendarza
                for (int day = 1; day <= 7; day++)
                {
                    var border = new Border
                    {
                        BorderBrush = new SolidColorBrush(Color.FromRgb(225, 229, 233)),
                        BorderThickness = new Thickness(0, 0, 1, 1),
                        Background = new SolidColorBrush(Colors.White),
                        Margin = new Thickness(1)
                    };
                    Grid.SetColumn(border, day);
                    Grid.SetRow(border, hour - 7);  // Wiersze 1-11
                    CalendarGrid.Children.Add(border);
                }
            }
        }

        public void AddMeetingsToGrid()
        {
            foreach (var meeting in _weeklyMeetings)
            {
                if (meeting.StartTime.Value.Hour < 8 || meeting.StartTime.Value.Hour >= 18)
                    continue;

                var meetingBlock = CreateMeetingBlock(meeting);

                int dayColumn = ((int)meeting.StartTime.Value.DayOfWeek + 6) % 7 + 1;
                int hourRow = meeting.StartTime.Value.Hour - 7;

                if (hourRow >= 0 && hourRow < CalendarGrid.RowDefinitions.Count)
                {
                    Grid.SetColumn(meetingBlock, dayColumn);
                    Grid.SetRow(meetingBlock, hourRow);
                    CalendarGrid.Children.Add(meetingBlock);
                }
            }
        }

        public Border CreateMeetingBlock(Meeting meeting)
        {
            var colorIndex = Math.Abs(meeting.Title.GetHashCode()) % _meetingColors.Length;
            var bgColor = _meetingColors[colorIndex];

            var border = new Border
            {
                Background = bgColor,
                CornerRadius = new CornerRadius(4),
                Margin = new Thickness(2, 2, 2, 2),
                Padding = new Thickness(6, 4, 6, 4),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            var stackPanel = new StackPanel();

            var titleBlock = new TextBlock
            {
                Text = meeting.Title,
                Foreground = new SolidColorBrush(Colors.White),
                FontWeight = FontWeights.SemiBold,
                FontSize = 12,
                TextWrapping = TextWrapping.Wrap
            };

            var timeBlock = new TextBlock
            {
                Text = meeting.StartTime.Value.ToString("HH:mm", CultureInfo.InvariantCulture),
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 11,
                Opacity = 0.9
            };

            stackPanel.Children.Add(titleBlock);
            stackPanel.Children.Add(timeBlock);
            border.Child = stackPanel;

            // Obsługa kliknięcia
            border.MouseLeftButtonUp += (s, e) => ShowMeetingDetails(meeting);

            return border;
        }

        public void ShowMeetingDetails(Meeting meeting)
        {
            string details = $"Spotkanie: {meeting.Title}\n" +
                             $"Data: {meeting.StartTime:dd.MM.yyyy HH:mm}\n";

            var candidates = meeting.Participants
                .Where(p => p.Candidate != null)
                .Select(p => $"{p.Candidate.FirstName} {p.Candidate.LastName}")
                .Distinct()
                .ToList();

            var clients = meeting.Participants
                .Where(p => p.Client != null)
                .Select(p => p.Client.Company)
                .Distinct()
                .ToList();

            if (candidates.Any())
            {
                details += "Kandydaci:\n";
                foreach (var c in candidates)
                {
                    details += $"- {c}\n";
                }
            }

            if (clients.Any())
            {
                details += "Klienci:\n";
                foreach (var c in clients)
                {
                    details += $"- {c}\n";
                }
            }

            MessageBox.Show(details, "Szczegóły spotkania", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void PrevWeek_Click(object sender, RoutedEventArgs e)
        {
            _currentWeekStart = _currentWeekStart.AddDays(-7);
            SideCalendar.SelectedDate = _currentWeekStart;
            UpdateCalendarView();
        }

        public void NextWeek_Click(object sender, RoutedEventArgs e)
        {
            _currentWeekStart = _currentWeekStart.AddDays(7);
            SideCalendar.SelectedDate = _currentWeekStart;
            UpdateCalendarView();
        }

        public void SideCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SideCalendar.SelectedDate.HasValue)
            {
                _currentWeekStart = GetWeekStart(SideCalendar.SelectedDate.Value);
                UpdateCalendarView();
            }
        }

        public void NewMeeting_Click(object sender, RoutedEventArgs e)
        {
            MeetingDatePicker.SelectedDate = SideCalendar.SelectedDate ?? DateTime.Today;
            NewMeetingPopup.IsOpen = true;
        }

        public void CancelMeeting_Click(object sender, RoutedEventArgs e)
        {
            NewMeetingPopup.IsOpen = false;
            ClearForm();
        }

        public void SaveMeeting_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            if (!TimeSpan.TryParse(StartTimeTextBox.Text.Trim(), out TimeSpan startTime))
            {
                MessageBox.Show("Nieprawidłowy format godziny rozpoczęcia (użyj HH:mm).", "Błąd walidacji");
                return;
            }
            if (!TimeSpan.TryParse(EndTimeTextBox.Text.Trim(), out TimeSpan endTime))
            {
                MessageBox.Show("Nieprawidłowy format godziny zakończenia (użyj HH:mm).", "Błąd walidacji");
                return;
            }

            // Sprawdź czy spotkanie jest w godzinach 8:00-18:00
            if (startTime.Hours < 8 || startTime.Hours >= 18 ||
                endTime.Hours < 8 || endTime.Hours > 18 ||
                (endTime.Hours == 18 && endTime.Minutes > 0))
            {
                MessageBox.Show("Spotkania mogą się odbywać tylko w godzinach 8:00-18:00.", "Błąd walidacji");
                return;
            }

            if (endTime <= startTime)
            {
                MessageBox.Show("Godzina zakończenia musi być późniejsza niż rozpoczęcia.", "Błąd walidacji");
                return;
            }

            try
            {
                var meetingDate = MeetingDatePicker.SelectedDate.Value.Date;

                var newMeeting = new Meeting
                {
                    Title = TitleTextBox.Text.Trim(),
                    StartTime = meetingDate + startTime,
                    EndTime = meetingDate + endTime,
                };

                // Dodaj twórcę spotkania jako uczestnika
                var currentUser = _user;
                if (currentUser != null)
                {
                    var creatorParticipant = new MeetingParticipant();

                    if (_user.Employee != null)
                    {
                        creatorParticipant.EmployeeId = _user.Employee.Id;
                    }
                    else if (_user.Candidate != null)
                    {
                        creatorParticipant.CandidateId = _user.Candidate.Id;
                    }
                    else if (_user.Client != null)
                    {
                        creatorParticipant.ClientId = _user.Client.Id;
                    }

                    newMeeting.Participants.Add(creatorParticipant);
                }


                // Dodaj wybranych uczestników
                foreach (ParticipantItem selected in ParticipantsListBox.SelectedItems)
                {
                    var participant = new MeetingParticipant();

                    if (selected.Entity is Employee employee)
                    {
                        participant.EmployeeId = employee.Id;
                    }
                    else if (selected.Entity is Candidate candidate)
                    {
                        participant.CandidateId = candidate.Id;
                    }
                    else if (selected.Entity is Client client)
                    {
                        participant.ClientId = client.Id;
                    }

                    newMeeting.Participants.Add(participant);
                }

                _context.Meetings.Add(newMeeting);
                _context.SaveChanges();

                NewMeetingPopup.IsOpen = false;
                ClearForm();
                UpdateCalendarView();

                MessageBox.Show("Spotkanie zostało dodane!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd podczas zapisywania: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Podaj tytuł spotkania.", "Błąd walidacji");
                return false;
            }

            if (!MeetingDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Wybierz datę spotkania.", "Błąd walidacji");
                return false;
            }

            if (!TimeSpan.TryParse(StartTimeTextBox.Text, out _))
            {
                MessageBox.Show("Nieprawidłowy format godziny rozpoczęcia (użyj HH:mm).", "Błąd walidacji");
                return false;
            }

            if (!TimeSpan.TryParse(EndTimeTextBox.Text, out _))
            {
                MessageBox.Show("Nieprawidłowy format godziny zakończenia (użyj HH:mm).", "Błąd walidacji");
                return false;
            }

            return true;
        }

        public void ClearForm()
        {
            TitleTextBox.Text = "";
            MeetingDatePicker.SelectedDate = null;
            StartTimeTextBox.Text = "09:00";
            EndTimeTextBox.Text = "10:00";
            ParticipantsListBox.SelectedItems.Clear();
        }

        public void NavigateToWeek(DateTime date)
        {
            _currentWeekStart = GetWeekStart(date);
            SideCalendar.SelectedDate = date;
            UpdateCalendarView();
        }
    }
}