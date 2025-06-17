using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Model
{
    public class ProjectAcceptance
    {
        public long Id { get; set; }

        public long ProjectCardId { get; set; }
        public ProjectCard ProjectCard { get; set; }

        public long? SupportId { get; set; }

        public bool AcceptedBySupport { get; set; }
        public DateTime? SupportAcceptedAt { get; set; }

        public bool AcceptedByRecruiter { get; set; }
        public DateTime? RecruiterAcceptedAt { get; set; }

        public bool AcceptedByClient { get; set; }
        public DateTime? ClientAcceptedAt { get; set; }
    }

}
