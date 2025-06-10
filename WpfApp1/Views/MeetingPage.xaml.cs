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

namespace WpfApp1.Views
{
    public partial class MeetingPage : Page
    {
        private readonly Frame _mainFrame;
        private readonly User _user;
        private readonly SyzyfContext _context;
        private DateTime _currentWeekStart;
        private List<Meeting> _weeklyMeetings;

        // Kolory dla różnych typów spotkań
        private readonly Brush[] _meetingColors =
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
            UpdateCalendarView();
        }

        private DateTime GetWeekStart(DateTime date)
        {
            int daysFromMonday = ((int)date.DayOfWeek - 1 + 7) % 7;
            return date.Date.AddDays(-daysFromMonday);
        }

        private void UpdateCalendarView()
        {
            LoadWeeklyMeetings();
            UpdateMonthYearDisplay();
            BuildCalendarGrid();
        }

        private void LoadWeeklyMeetings()
        {
            DateTime weekEnd = _currentWeekStart.AddDays(7).AddTicks(-1);

            _weeklyMeetings = _context.Meetings
                .Include(m => m.Participants)
                    .ThenInclude(p => p.Employee)
                .Include(m => m.Participants)
                    .ThenInclude(p => p.Candidate)
                .Include(m => m.Participants)
                    .ThenInclude(p => p.Client)
                .Where(m => m.StartTime >= _currentWeekStart && m.StartTime <= weekEnd)
                .OrderBy(m => m.StartTime)
                .ToList();

        }

        private void UpdateMonthYearDisplay()
        {
            var culture = new CultureInfo("pl-PL");
            string monthName = _currentWeekStart.ToString("MMMM yyyy", culture);
            CurrentMonthYear.Text = char.ToUpper(monthName[0]) + monthName.Substring(1);
        }

        private void BuildCalendarGrid()
        {
            CalendarGrid.Children.Clear();
            CalendarGrid.RowDefinitions.Clear();

            // Tworzenie wierszy dla godzin (6:00 - 22:00)
            for (int hour = 6; hour <= 22; hour++)
            {
                CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });

                // Etykieta godziny
                var timeLabel = new TextBlock
                {
                    Text = $"{hour:00}:00",
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 12,
                    Foreground = new SolidColorBrush(Colors.Gray),
                    Margin = new Thickness(0, 5, 0, 0)
                };
                Grid.SetColumn(timeLabel, 0);
                Grid.SetRow(timeLabel, hour - 6);
                CalendarGrid.Children.Add(timeLabel);

                // Linie poziome
                for (int day = 1; day <= 7; day++)
                {
                    var border = new Border
                    {
                        BorderBrush = new SolidColorBrush(Color.FromRgb(225, 229, 233)),
                        BorderThickness = new Thickness(0, 0, 1, 1),
                        Background = new SolidColorBrush(Colors.White)
                    };
                    Grid.SetColumn(border, day);
                    Grid.SetRow(border, hour - 6);
                    CalendarGrid.Children.Add(border);
                }
            }

            // Dodawanie spotkań do siatki
            AddMeetingsToGrid();
        }

        private void AddMeetingsToGrid()
        {
            foreach (var meeting in _weeklyMeetings)
            {
                var meetingBlock = CreateMeetingBlock(meeting);

                int dayColumn = ((int)meeting.StartTime.Value.DayOfWeek + 6) % 7 + 1;
                int hourRow = meeting.StartTime.Value.Hour - 6;


                if (hourRow >= 0 && hourRow < CalendarGrid.RowDefinitions.Count)
                {
                    Grid.SetColumn(meetingBlock, dayColumn);
                    Grid.SetRow(meetingBlock, hourRow);
                    CalendarGrid.Children.Add(meetingBlock);
                }
            }
        }

        private Border CreateMeetingBlock(Meeting meeting)
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

        private void ShowMeetingDetails(Meeting meeting)
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



        private void PrevWeek_Click(object sender, RoutedEventArgs e)
        {
            _currentWeekStart = _currentWeekStart.AddDays(-7);
            UpdateCalendarView();
        }

        private void NextWeek_Click(object sender, RoutedEventArgs e)
        {
            _currentWeekStart = _currentWeekStart.AddDays(7);
            UpdateCalendarView();
        }

        private void NewMeeting_Click(object sender, RoutedEventArgs e)
        {
            // Ustaw domyślną datę na dziś lub pierwszy dzień aktualnego tygodnia
            MeetingDatePicker.SelectedDate = DateTime.Today;
            NewMeetingPopup.IsOpen = true;
        }

        private void CancelMeeting_Click(object sender, RoutedEventArgs e)
        {
            NewMeetingPopup.IsOpen = false;
            ClearForm();
        }

        private void SaveMeeting_Click(object sender, RoutedEventArgs e)
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


        private bool ValidateForm()
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

        private void ClearForm()
        {
            TitleTextBox.Text = "";
            MeetingDatePicker.SelectedDate = null;
            StartTimeTextBox.Text = "09:00";
            EndTimeTextBox.Text = "10:00";
        }

        // Metoda publiczna do ustawiania konkretnego tygodnia (np. z głównego kalendarza)
        public void NavigateToWeek(DateTime date)
        {
            _currentWeekStart = GetWeekStart(date);
            UpdateCalendarView();
        }
    }
}