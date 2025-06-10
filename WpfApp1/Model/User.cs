using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WpfApp1.Models;

namespace WpfApp1.Model
{
    public class User
    {
        [Key]
        public long Id { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }

        public long? EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        public long? ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        public long? CandidateId { get; set; }
        [ForeignKey("CandidateId")]
        public Candidate Candidate { get; set; }
    }
}
