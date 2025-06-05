using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class OrderFormModel
    {
        // Sekcja klienta
        public string? Nip { get; set; }
        public string? Address { get; set; }
        public string? Company { get; set; }

        // 1. Informacje o ofercie
        public int NumberOfPeople { get; set; }
        public List<string> SearchReasons { get; set; } = new();
        public bool ShowSalary { get; set; }

        // 2. Osoba kontaktowa
        public string? FullName { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactNumber { get; set; }
        public string? ContactPersonEmail { get; set; }
        public string? ContactPersonNumber { get; set; }

        // 3. Informacja o stanowisku
        public string? JobTitle { get; set; }
        public List<string> JobLevels { get; set; } = new();
        public string? Department { get; set; }
        public string? MainDuties { get; set; }
        public string? ExtraDuties { get; set; }
        public string? DevelopmentOpportunities { get; set; }
        public DateTime? PlannedStartDate { get; set; }

        // 4. Wymagania
        public List<string> EducationLevels { get; set; } = new();
        public string? PreferredFields { get; set; }
        public string? AdditionalQualifications { get; set; }
        public string? RequiredExperience { get; set; }
        public string? NiceToHaveExperience { get; set; }
        public string? RequiredSkills { get; set; }
        public string? NiceToHaveSkills { get; set; }
        public string? RequiredLanguages { get; set; }
        public string? NiceToHaveLanguages { get; set; }

        // 5. Warunki pracy
        public List<string> EmploymentForms { get; set; } = new();
        public string? SalaryRange { get; set; }
        public bool HasBonusSystem { get; set; }
        public string? AdditionalBenefits { get; set; }
        public string? WorkTools { get; set; }
        public string? WorkLocation { get; set; }
        public string? WorkingHours { get; set; }
        public List<string> WorkModes { get; set; } = new(); // np. zdalna, hybrydowa, stacjonarna
        public string? AdditionalNotes { get; set; }
    }

}
