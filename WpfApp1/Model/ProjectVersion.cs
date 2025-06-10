using WpfApp1.Models;

namespace WpfApp1.Model
{
    public class ProjectVersion
    {
        public long Id { get; set; }
        public DateTime? ChangedTime { get; set; }
        public string Details { get; set; }
        public long? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public long ProjectId { get; set; }
        public Project Project { get; set; }
    }
}