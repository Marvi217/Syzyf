using Microsoft.EntityFrameworkCore;
using System.Linq;
using WpfApp1.Data;
using WpfApp1.Model;

namespace WpfApp1.Services
{
    public class AuthService
    {
        private readonly SyzyfContext _context;

        public AuthService(SyzyfContext context)
        {
            _context = context;
        }

        public User? Authenticate(string login, string password)
        {
            return _context.Users
                .Include(u => u.Employee)
                .Include(u => u.Client)
                .Include(u => u.Candidate)
                .AsNoTracking()
                .FirstOrDefault(u => u.Login == login && u.Password == password);
        }
    }
}
