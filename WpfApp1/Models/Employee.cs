﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WpfApp1.Model
{
    public class Employee
    {
        [Key]
        public long Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? WorkSince { get; set; }
        public long PositionId { get; set; }
        public Position Position { get; set; }
        public User User { get; set; }
    }
}
