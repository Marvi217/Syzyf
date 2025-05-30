using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Data;

namespace WpfApp1.Services
{
    public class AuthService
    {
        private readonly SyzyfContext _context;

        public AuthService(SyzyfContext context)
        {
            _context = context;
        }

        public bool Login(string login, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            return user != null;
        }
    }

}
