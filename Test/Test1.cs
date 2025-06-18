using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Model;
using WpfApp1.Models;
using WpfApp1.Views;

namespace Test
{
    [STATestClass]
    public sealed class Test1
    {
        #region MeetingPage Tests

        [TestMethod]
        [DataRow("2024-05-20", "2024-05-20")]
        [DataRow("2024-05-22", "2024-05-20")]
        [DataRow("2024-05-26", "2024-05-20")]
        public void GetWeekStart_ForVariousDays_ReturnsCorrectMonday_Static(string inputDateStr, string expectedDateStr)
        {
            var methodInfo = typeof(MeetingPage).GetMethod("GetWeekStart", BindingFlags.NonPublic | BindingFlags.Static);

            if (methodInfo == null)
            {
                Assert.Inconclusive("Nie znaleziono metody 'GetWeekStart'. Upewnij się, że jest ona 'private static' w klasie MeetingPage.");
                return;
            }

            var inputDate = DateTime.Parse(inputDateStr);
            var expectedDate = DateTime.Parse(expectedDateStr);

            var result = (DateTime)methodInfo.Invoke(null, new object[] { inputDate });

            Assert.AreEqual(expectedDate.Date, result.Date);
        }

        #endregion

        #region ProjectCard Tests

        [TestMethod]
        public void ProjectCard_IsAccepted_WhenProjectAcceptanceIsNull_ShouldReturnFalse()
        {
            var projectCard = new ProjectCard();
            var isAccepted = projectCard.IsAccepted;
            Assert.IsFalse(isAccepted, "IsAccepted powinno zwrócić false, gdy ProjectAcceptance jest null.");
        }

        [TestMethod]
        public void ProjectCard_IsAccepted_WhenClientHasNotAccepted_ShouldReturnFalse()
        {
            var projectCard = new ProjectCard { ProjectAcceptance = new ProjectAcceptance { AcceptedByClient = false, AcceptedByRecruiter = true, AcceptedBySupport = true } };
            var isAccepted = projectCard.IsAccepted;
            Assert.IsFalse(isAccepted, "IsAccepted powinno zwrócić false, gdy Klient nie zaakceptował.");
        }

        [TestMethod]
        public void ProjectCard_IsAccepted_WhenRecruiterHasNotAccepted_ShouldReturnFalse()
        {
            var projectCard = new ProjectCard { ProjectAcceptance = new ProjectAcceptance { AcceptedByClient = true, AcceptedByRecruiter = false, AcceptedBySupport = true } };
            var isAccepted = projectCard.IsAccepted;
            Assert.IsFalse(isAccepted, "IsAccepted powinno zwrócić false, gdy Rekruter nie zaakceptował.");
        }

        [TestMethod]
        public void ProjectCard_IsAccepted_WhenSupportHasNotAccepted_ShouldReturnFalse()
        {
            var projectCard = new ProjectCard { ProjectAcceptance = new ProjectAcceptance { AcceptedByClient = true, AcceptedByRecruiter = true, AcceptedBySupport = false } };
            var isAccepted = projectCard.IsAccepted;
            Assert.IsFalse(isAccepted, "IsAccepted powinno zwrócić false, gdy Support nie zaakceptował.");
        }

        [TestMethod]
        public void ProjectCard_IsAccepted_WhenAllPartiesHaveAccepted_ShouldReturnTrue()
        {
            var projectCard = new ProjectCard { ProjectAcceptance = new ProjectAcceptance { AcceptedByClient = true, AcceptedByRecruiter = true, AcceptedBySupport = true } };
            var isAccepted = projectCard.IsAccepted;
            Assert.IsTrue(isAccepted, "IsAccepted powinno zwrócić true, gdy wszystkie strony zaakceptowały.");
        }

        #endregion

        #region Project Tests

        [TestMethod]
        public void Project_Constructor_ShouldInitializeCandidateSelections()
        {
            var project = new Project();
            Assert.IsNotNull(project.CandidateSelections, "Kolekcja CandidateSelections nie powinna być null po utworzeniu obiektu Project.");
        }

        #endregion

        #region Meeting Tests

        [TestMethod]
        public void Meeting_Constructor_ShouldInitializeParticipants()
        {
            var meeting = new Meeting();
            Assert.IsNotNull(meeting.Participants, "Kolekcja Participants nie powinna być null po utworzeniu obiektu Meeting.");
        }

        #endregion

        #region ProjectCardFormPage Validation Tests

        private void SetAllFieldsToValid(ProjectCardFormPage page)
        {
            page.NumberOfPeopleBox.Text = "5";
            page.JobTitleBox.Text = "Deweloper C#";
            page.DepartmentBox.Text = "IT";
            page.MainDutiesBox.Text = "Tworzenie aplikacji";
            page.RequiredExperienceBox.Text = "Minimum 3 lata";
            page.RequiredSkillsBox.Text = "C#, .NET, WPF";
            page.RequiredLanguagesBox.Text = "Angielski B2";
            page.GrossSalaryBox.Text = "10000 - 15000 PLN";
            page.WorkPlaceBox.Text = "Zdalnie / Biuro w Warszawie";
            page.WorkingHoursBox.Text = "8:00 - 16:00";
            page.PlannedHiringDatePicker.SelectedDate = DateTime.Now.AddMonths(1);
            (page.SalaryVisibilityPanel.Children[0] as RadioButton).IsChecked = true;
            (page.BonusPanel.Children[0] as RadioButton).IsChecked = true;
            (page.JobLevelsPanel.Children[0] as CheckBox).IsChecked = true;
            (page.EducationPanel.Children[0] as CheckBox).IsChecked = true;
            (page.EmploymentFormsPanel.Children[0] as CheckBox).IsChecked = true;
            (page.WorkModesPanel.Children[0] as CheckBox).IsChecked = true;
        }

        [TestMethod]
        public void ProjectCardFormPage_Validate_WithAllValidData_ReturnsTrue()
        {
            var page = new ProjectCardFormPage();
            SetAllFieldsToValid(page);

            bool isValid = page.ValidateRequiredFields();

            Assert.IsTrue(isValid, "Walidacja powinna zakończyć się sukcesem dla poprawnych danych.");
            Assert.AreEqual(Visibility.Collapsed, page.JobTitleError.Visibility);
            Assert.AreEqual(Visibility.Collapsed, page.NumberOfPeopleError.Visibility);
            Assert.AreEqual(Visibility.Collapsed, page.PlannedHiringDateError.Visibility);
        }

        [TestMethod]
        public void ProjectCardFormPage_Validate_WithInvalidNumberOfPeople_ReturnsFalseAndShowsError()
        {
            var page = new ProjectCardFormPage();
            SetAllFieldsToValid(page);
            page.NumberOfPeopleBox.Text = "0";

            bool isValid = page.ValidateRequiredFields();

            Assert.IsFalse(isValid, "Walidacja powinna zakończyć się błędem dla liczby osób <= 0.");
            Assert.AreEqual(Visibility.Visible, page.NumberOfPeopleError.Visibility);
            Assert.AreEqual("Podaj liczbę większą od zera.", page.NumberOfPeopleError.Text);
        }

        [TestMethod]
        public void ProjectCardFormPage_Validate_WithPastHiringDate_ReturnsFalseAndShowsError()
        {
            var page = new ProjectCardFormPage();
            SetAllFieldsToValid(page);
            page.PlannedHiringDatePicker.SelectedDate = DateTime.Now.AddDays(-1);

            bool isValid = page.ValidateRequiredFields();

            Assert.IsFalse(isValid, "Walidacja powinna zakończyć się błędem dla daty w przeszłości.");
            Assert.AreEqual(Visibility.Visible, page.PlannedHiringDateError.Visibility);
            Assert.AreEqual("Data nie może być w przeszłości.", page.PlannedHiringDateError.Text);
        }


        [TestMethod]
        public void ProjectCardFormPage_Validate_WithNoBonusSystemSelected_ReturnsFalseAndShowsError()
        {
            var page = new ProjectCardFormPage();
            SetAllFieldsToValid(page);
            (page.BonusPanel.Children[0] as RadioButton).IsChecked = false;

            bool isValid = page.ValidateRequiredFields();

            Assert.IsFalse(isValid, "Walidacja powinna zakończyć się błędem, gdy nie wybrano opcji systemu premiowego.");
            Assert.AreEqual(Visibility.Visible, page.BonusError.Visibility);
        }

        #endregion
    }
}