using WpfApp1.Models;

namespace WpfApp1.Model
{
    public class RecruitmentMeeting
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public DateTime? MeetingDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string MeetingType { get; set; }

        public long CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        public long ClientId { get; set; }
        public Client Client { get; set; }
    }
}