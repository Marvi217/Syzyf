namespace WpfApp1.Model
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public long? EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}