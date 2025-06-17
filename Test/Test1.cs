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
    [TestClass]
    public sealed class Test1
    {

        #region MeetingPage Tests

        /// <summary>
        /// Testuje metodę GetWeekStart dla różnych dni tygodnia.
        /// Zakładamy, że metoda GetWeekStart została zmieniona na statyczną.
        /// </summary>
        /// <param name="inputDateStr">Data wejściowa.</param>
        /// <param name="expectedDateStr">Oczekiwany poniedziałek dla tej daty.</param>
        [TestMethod]
        [DataRow("2024-05-20", "2024-05-20")] // Poniedziałek
        [DataRow("2024-05-22", "2024-05-20")] // Środa
        [DataRow("2024-05-26", "2024-05-20")] // Niedziela
        public void GetWeekStart_ForVariousDays_ReturnsCorrectMonday_Static(string inputDateStr, string expectedDateStr)
        {
            // Arrange
            // Pobieramy metodę statyczną z typu MeetingPage, a nie z instancji obiektu.
            var methodInfo = typeof(MeetingPage).GetMethod("GetWeekStart", BindingFlags.NonPublic | BindingFlags.Static);

            if (methodInfo == null)
            {
                Assert.Inconclusive("Nie znaleziono metody 'GetWeekStart'. Upewnij się, że jest ona 'private static' w klasie MeetingPage.");
                return;
            }

            var inputDate = DateTime.Parse(inputDateStr);
            var expectedDate = DateTime.Parse(expectedDateStr);

            // Act
            // Dla metod statycznych pierwszy argument Invoke to null, ponieważ nie ma instancji 'this'.
            var result = (DateTime)methodInfo.Invoke(null, new object[] { inputDate });

            // Assert
            Assert.AreEqual(expectedDate.Date, result.Date);
        }

        #endregion

        #region ProjectCard Tests

        /// <summary>
        /// Testuje, czy właściwość IsAccepted zwraca false, gdy ProjectAcceptance jest null.
        /// </summary>
        [TestMethod]
        public void ProjectCard_IsAccepted_WhenProjectAcceptanceIsNull_ShouldReturnFalse()
        {
            // Arrange
            var projectCard = new ProjectCard();

            // Act
            var isAccepted = projectCard.IsAccepted;

            // Assert
            Assert.IsFalse(isAccepted, "IsAccepted powinno zwrócić false, gdy ProjectAcceptance jest null.");
        }

        /// <summary>
        /// Testuje, czy właściwość IsAccepted zwraca false, gdy Klient nie zaakceptował projektu.
        /// </summary>
        [TestMethod]
        public void ProjectCard_IsAccepted_WhenClientHasNotAccepted_ShouldReturnFalse()
        {
            // Arrange
            var projectCard = new ProjectCard
            {
                ProjectAcceptance = new ProjectAcceptance
                {
                    AcceptedByClient = false,
                    AcceptedByRecruiter = true,
                    AcceptedBySupport = true
                }
            };

            // Act
            var isAccepted = projectCard.IsAccepted;

            // Assert
            Assert.IsFalse(isAccepted, "IsAccepted powinno zwrócić false, gdy Klient nie zaakceptował.");
        }

        /// <summary>
        /// Testuje, czy właściwość IsAccepted zwraca false, gdy Rekruter nie zaakceptował projektu.
        /// </summary>
        [TestMethod]
        public void ProjectCard_IsAccepted_WhenRecruiterHasNotAccepted_ShouldReturnFalse()
        {
            // Arrange
            var projectCard = new ProjectCard
            {
                ProjectAcceptance = new ProjectAcceptance
                {
                    AcceptedByClient = true,
                    AcceptedByRecruiter = false,
                    AcceptedBySupport = true
                }
            };

            // Act
            var isAccepted = projectCard.IsAccepted;

            // Assert
            Assert.IsFalse(isAccepted, "IsAccepted powinno zwrócić false, gdy Rekruter nie zaakceptował.");
        }

        /// <summary>
        /// Testuje, czy właściwość IsAccepted zwraca false, gdy Support nie zaakceptował projektu.
        /// </summary>
        [TestMethod]
        public void ProjectCard_IsAccepted_WhenSupportHasNotAccepted_ShouldReturnFalse()
        {
            // Arrange
            var projectCard = new ProjectCard
            {
                ProjectAcceptance = new ProjectAcceptance
                {
                    AcceptedByClient = true,
                    AcceptedByRecruiter = true,
                    AcceptedBySupport = false
                }
            };

            // Act
            var isAccepted = projectCard.IsAccepted;

            // Assert
            Assert.IsFalse(isAccepted, "IsAccepted powinno zwrócić false, gdy Support nie zaakceptował.");
        }

        /// <summary>
        /// Testuje, czy właściwość IsAccepted zwraca true, gdy wszystkie strony zaakceptowały projekt.
        /// </summary>
        [TestMethod]
        public void ProjectCard_IsAccepted_WhenAllPartiesHaveAccepted_ShouldReturnTrue()
        {
            // Arrange
            var projectCard = new ProjectCard
            {
                ProjectAcceptance = new ProjectAcceptance
                {
                    AcceptedByClient = true,
                    AcceptedByRecruiter = true,
                    AcceptedBySupport = true
                }
            };

            // Act
            var isAccepted = projectCard.IsAccepted;

            // Assert
            Assert.IsTrue(isAccepted, "IsAccepted powinno zwrócić true, gdy wszystkie strony zaakceptowały.");
        }

        #endregion

        #region Project Tests

        /// <summary>
        /// Testuje, czy konstruktor klasy Project poprawnie inicjalizuje kolekcję CandidateSelections.
        /// </summary>
        [TestMethod]
        public void Project_Constructor_ShouldInitializeCandidateSelections()
        {
            // Arrange & Act
            var project = new Project();

            // Assert
            Assert.IsNotNull(project.CandidateSelections, "Kolekcja CandidateSelections nie powinna być null po utworzeniu obiektu Project.");
        }

        #endregion

        #region Meeting Tests

        /// <summary>
        /// Testuje, czy konstruktor klasy Meeting poprawnie inicjalizuje kolekcję Participants.
        /// </summary>
        [TestMethod]
        public void Meeting_Constructor_ShouldInitializeParticipants()
        {
            // Arrange & Act
            var meeting = new Meeting();

            // Assert
            Assert.IsNotNull(meeting.Participants, "Kolekcja Participants nie powinna być null po utworzeniu obiektu Meeting.");
        }

        #endregion
    }
}
