namespace WpfApp1.Model
{
    public class Position
    {
        public long Id { get; set; }
        public string PositionName { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}