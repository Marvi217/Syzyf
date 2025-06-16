using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Models
{
    public class Client
    {
        [Key]
        public long Id { get; set; }
        public string? Nip { get; set; }
        public string? Address { get; set; }
        public string? Company { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactNumber { get; set; }
        public string? ContactPersonEmail { get; set; }
        public string? ContactPersonNumber { get; set; }
        public User User { get; set; }

        public ICollection<Project> Projects { get; set; }
        public ICollection<ProjectCard> ProjectCards { get; set; }
        public ICollection<Evaluation> Evaluations { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
        public ICollection<RecruitmentMeeting> RecruitmentMeetings { get; set; }
    }


}

