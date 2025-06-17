using WpfApp1.Models;

namespace WpfApp1.Model
{
    public class CandidateSelection
    {
        public long Id { get; set; }
        public DateTime? SelectionDate { get; set; }
        public string Status { get; set; } // Możesz zrobić enum jeśli chcesz

        public long CandidateId { get; set; }
        public Candidate Candidate { get; set; }

        public long ProjectId { get; set; }
        public Project Project { get; set; }
    }
}