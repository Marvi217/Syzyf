using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Model
{
    public class ProjectCard : IProjectBase
    {
        public long Id { get; set; }
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
        public bool IsAcceptedDb { get; set; }

        public ProjectCardStatus Status { get; set; }

        public ProjectAcceptance ProjectAcceptance { get; set; }

        public bool IsAccepted
        {
            get
            {
                return ProjectAcceptance != null &&
                       ProjectAcceptance.AcceptedByClient &&
                       ProjectAcceptance.AcceptedByRecruiter &&
                       ProjectAcceptance.AcceptedBySupport;
            }
        }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        [ForeignKey("RecruiterId")]
        public long? RecruiterId { get; set; }
        public Employee Recruiter { get; set; }
        [InverseProperty("ProjectCard")]
        public List<Notification> Notifications { get; set; }


    }

    public enum ProjectCardStatus
    {
        Processed,
        Pending,
        Accepted,
        Canceled,
        Rejected
    }

}
