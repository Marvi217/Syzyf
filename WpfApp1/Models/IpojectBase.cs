using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public interface IProjectBase
    {
        long Id { get; }
        int NumberOfPeople { get; }
        bool IsSalaryVisible { get; }
        string JobTitle { get; }
        string JobLevels { get; }
        string Department { get; }
        string MainDuties { get; }
        string AdditionalDuties { get; }
        DateTime? PlannedHiringDate { get; }
        string Education { get; }
        string PreferredStudyFields { get; }
        string AdditionalCertifications { get; }
        string RequiredExperience { get; }
        string PreferredExperience { get; }
        string RequiredSkills { get; }
        string PreferredSkills { get; }
        string RequiredLanguages { get; }
        string PreferredLanguages { get; }
        string EmploymentsForms { get; }
        string GrossSalary { get; }
        bool BonusSystem { get; }
        string AdditionalBenefits { get; }
        string WorkTools { get; }
        string WorkPlace { get; }
        string WorkModes { get; }
        string WorkingHours { get; }
        string OtherRemarks { get; }
        public long ClientId { get; set; }
        public long? RecruiterId { get; set; }
    }
}

