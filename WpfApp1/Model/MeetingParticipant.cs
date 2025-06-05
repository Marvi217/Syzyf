using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Model
{
    public class MeetingParticipant
    {
        [Key]
        public long Id { get; set; }

        public long MeetingId { get; set; }
        public Meeting Meeting { get; set; }

        public long? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public long? CandidateId { get; set; }
        public Candidate Candidate { get; set; }

        public long? ClientId { get; set; }
        public Client Client { get; set; }
    }
}
