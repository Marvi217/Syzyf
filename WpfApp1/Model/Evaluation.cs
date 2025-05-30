using WpfApp1.Models;

namespace WpfApp1.Model
{
    public class Evaluation
    {
        public long Id { get; set; }
        public string Comment { get; set; }
        public DateTime? EvaluationDate { get; set; }
        public int? Score { get; set; }

        public long CandidateId { get; set; }
        public Candidate Candidate { get; set; }

        public long ClientId { get; set; }
        public Client Client { get; set; }
    }
}