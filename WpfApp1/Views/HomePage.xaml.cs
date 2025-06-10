using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Model;
using WpfApp1.Models;
using WpfApp1.Data;

namespace WpfApp1.Views
{
    public partial class HomePage : Page
    {
        private Frame _mainFrame;
        private User _user;
        private SyzyfContext _context;

        public HomePage(Frame mainFrame, User user, SyzyfContext context)
        {
            InitializeComponent();
            _mainFrame = mainFrame;
            _user = user;
            _context = context;

            TopMenu.Initialize(_mainFrame, _user);

            CheckUserTypeAndPrompt();
        }

        private void MainCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainCalendar.SelectedDate.HasValue)
            {
                DateTime selectedDate = MainCalendar.SelectedDate.Value;
                var meetingsPage = new MeetingPage(_mainFrame, _user, selectedDate, _context);
                _mainFrame.Navigate(meetingsPage);
            }
        }

        private void CheckUserTypeAndPrompt()
        {
            if (_user.EmployeeId == null && _user.ClientId == null && _user.CandidateId == null)
            {
                var promptPanel = new StackPanel
                {
                    Margin = new Thickness(20),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                promptPanel.Children.Add(new TextBlock
                {
                    Text = "Wybierz typ konta:",
                    FontSize = 18,
                    Margin = new Thickness(0, 0, 0, 20),
                    TextAlignment = TextAlignment.Center
                });

                var clientButton = new Button
                {
                    Content = "Zarejestruj jako Klient",
                    Width = 200,
                    Margin = new Thickness(0, 0, 0, 10)
                };

                var candidateButton = new Button
                {
                    Content = "Zarejestruj jako Kandydat",
                    Width = 200
                };

                clientButton.Click += (s, e) => ShowClientForm(promptPanel);
                candidateButton.Click += (s, e) => ShowCandidateForm(promptPanel);

                promptPanel.Children.Add(clientButton);
                promptPanel.Children.Add(candidateButton);

                MainContentArea.Content = promptPanel;
            }
        }

        private void ShowClientForm(StackPanel container)
        {
            container.Children.Clear();

            var grid = new Grid();
            grid.Margin = new Thickness(10);
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300) });

            // Labels and textboxes
            var fields = new (string Label, TextBox TextBox)[]
            {
                ("NIP:", new TextBox()),
                ("Adres:", new TextBox()),
                ("Firma:", new TextBox()),
                ("Email kontaktowy:", new TextBox()),
                ("Telefon kontaktowy:", new TextBox()),
                ("Email osoby kontaktowej:", new TextBox()),
                ("Telefon osoby kontaktowej:", new TextBox())
            };

            for (int i = 0; i < fields.Length; i++)
            {
                var label = new TextBlock
                {
                    Text = fields[i].Label,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 5, 5, 5)
                };
                Grid.SetRow(label, i);
                Grid.SetColumn(label, 0);
                grid.Children.Add(label);

                var tb = fields[i].TextBox;
                tb.Margin = new Thickness(0, 5, 0, 5);
                Grid.SetRow(tb, i);
                Grid.SetColumn(tb, 1);
                grid.Children.Add(tb);
            }

            var saveButton = new Button
            {
                Content = "Zapisz",
                Width = 100,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Grid.SetRow(saveButton, fields.Length);
            Grid.SetColumn(saveButton, 1);

            saveButton.Click += (s, e) =>
            {
                var newClient = new Client
                {
                    Nip = fields[0].TextBox.Text,
                    Address = fields[1].TextBox.Text,
                    Company = fields[2].TextBox.Text,
                    ContactEmail = fields[3].TextBox.Text,
                    ContactNumber = fields[4].TextBox.Text,
                    ContactPersonEmail = fields[5].TextBox.Text,
                    ContactPersonNumber = fields[6].TextBox.Text
                };

                _context.Clients.Add(newClient);
                _context.SaveChanges();

                _user.ClientId = newClient.Id;
                _user.Client = newClient;

                _context.Users.Update(_user);
                _context.SaveChanges();

                MessageBox.Show("Zarejestrowano jako klient.");
                ReloadPage();
            };

            grid.Children.Add(saveButton);

            container.Children.Add(grid);
        }

        private void ShowCandidateForm(StackPanel container)
        {
            container.Children.Clear();

            var grid = new Grid();
            grid.Margin = new Thickness(10);
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(300) });

            var fields = new (string Label, TextBox TextBox)[]
            {
                ("Imię:", new TextBox()),
                ("Nazwisko:", new TextBox()),
                ("Adres:", new TextBox()),
                ("Email:", new TextBox()),
                ("Telefon:", new TextBox()),
                ("Stanowiska:", new TextBox())
            };

            for (int i = 0; i < fields.Length; i++)
            {
                var label = new TextBlock
                {
                    Text = fields[i].Label,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 5, 5, 5)
                };
                Grid.SetRow(label, i);
                Grid.SetColumn(label, 0);
                grid.Children.Add(label);

                var tb = fields[i].TextBox;
                tb.Margin = new Thickness(0, 5, 0, 5);
                Grid.SetRow(tb, i);
                Grid.SetColumn(tb, 1);
                grid.Children.Add(tb);
            }

            var saveButton = new Button
            {
                Content = "Zapisz",
                Width = 100,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Grid.SetRow(saveButton, fields.Length);
            Grid.SetColumn(saveButton, 1);

            saveButton.Click += (s, e) =>
            {
                var newCandidate = new Candidate
                {
                    FirstName = fields[0].TextBox.Text,
                    LastName = fields[1].TextBox.Text,
                    Address = fields[2].TextBox.Text,
                    Email = fields[3].TextBox.Text,
                    PhoneNumber = fields[4].TextBox.Text,
                    Positions = fields[5].TextBox.Text
                };

                _context.Candidates.Add(newCandidate);
                _context.SaveChanges();

                _user.CandidateId = newCandidate.Id;
                _user.Candidate = newCandidate; 

                _context.Users.Update(_user);
                _context.SaveChanges();

                MessageBox.Show("Zarejestrowano jako kandydat.");
                ReloadPage();
            };

            grid.Children.Add(saveButton);

            container.Children.Add(grid);
        }


        private void ReloadPage()
        {
            _mainFrame.Navigate(new HomePage(_mainFrame, _user, _context));
        }
    }
}
