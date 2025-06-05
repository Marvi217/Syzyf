using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WpfApp1.Data;
using WpfApp1.Model;
using WpfApp1.Models; // zakładamy że są modele: User, Employee, Position

namespace WpfApp1.Services
{
    public class RegistrationService
    {
        private readonly SyzyfContext _context;

        public RegistrationService(SyzyfContext context)
        {
            _context = context;
        }

        public User RegisterEmployeeAndUser(Employee employeeData, string login, string rawPassword, long positionId)
        {
            var position = _context.Positions.FirstOrDefault(p => p.Id == positionId);
            if (position == null)
            {
                throw new Exception($"Pozycja o ID {positionId} nie istnieje.");
            }

            employeeData.Position = position;
            employeeData.WorkSince = DateTime.Now;

            _context.Employees.Add(employeeData);
            _context.SaveChanges();

            var user = new User
            {
                Login = login,
                Password = rawPassword, // rozważ szyfrowanie hasła np. przez BCrypt
                Employee = employeeData
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }
    }
}
