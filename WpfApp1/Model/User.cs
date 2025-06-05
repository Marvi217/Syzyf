using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp1.Model
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        [ForeignKey("Employee")]
        public long? EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}