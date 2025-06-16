using System;
using System.Collections.Generic;
using WpfApp1.Models;
using WpfApp1.Model;

namespace WpfApp1.Models
{
    public class Project : IProjectBase
    {
        public long Id { get; set; }
        public long ProjectCardId { get; set; }
        public long ClientId { get; set; }
        public int NumberOfPeople { get; set; }
        public string JobLevels { get; set; }
        public bool IsSalaryVisible { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public string MainDuties { get; set; }
        public string AdditionalDuties { get; set; }
        public string DevelopmentOpportunities { get; set; }
        public DateTime? PlannedHiringDate { get; set; }
        public string Education { get; set; }
        public string PreferredStudyFields { get; set; }
        public string AdditionalCertifications { get; set; }
        public string RequiredExperience { get; set; }
        public string PreferredExperience { get; set; }
        public string RequiredSkills { get; set; }
        public string PreferredSkills { get; set; }
        public string RequiredLanguages { get; set; }
        public string PreferredLanguages { get; set; }
        public string EmploymentsForms { get; set; }
        public string GrossSalary { get; set; }
        public bool BonusSystem { get; set; }
        public string AdditionalBenefits { get; set; }
        public string WorkTools { get; set; }
        public string WorkPlace { get; set; }
        public string WorkModes { get; set; }
        public string WorkingHours { get; set; }
        public string OtherRemarks { get; set; }
        public ProjectStatus Status { get; set; }
        public long RecruiterId { get; set; }

        public ProjectCard ProjectCard { get; set; }
        public Client Client { get; set; }
        public Employee Recruiter { get; set; }
        public ICollection<CandidateSelection> CandidateSelections { get; set; }


        public Project()
        {
            CandidateSelections = new HashSet<CandidateSelection>();
        }
    }
    public enum ProjectStatus
    {
        Planned,
        InProgress,
        Completed
    }
}
