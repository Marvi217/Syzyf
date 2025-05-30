namespace WpfApp1.Model
{
    public class ProjectEmployee
    {
        public long Id { get; set; }
        public long? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public long? ProjectId { get; set; }
        public Project Project { get; set; }
    }
}