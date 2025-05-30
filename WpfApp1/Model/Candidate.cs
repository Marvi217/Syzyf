using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class Candidate
    {
        public long Id { get; set; }
        public string? Address { get; set; }
        public byte[]? Cv { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Positions { get; set; }

        public ICollection<CandidateSelection> CandidateSelections { get; set; }
        public ICollection<Evaluation> Evaluations { get; set; }
        public ICollection<RecruitmentMeeting> RecruitmentMeetings { get; set; }
    }

}
