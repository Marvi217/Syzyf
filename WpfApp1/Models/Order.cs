using System;
using System.ComponentModel.DataAnnotations;
using WpfApp1.Models;

namespace WpfApp1.Model
{
    public class Order
    {
        public long Id { get; set; }

        [Required]
        public long ClientId { get; set; }
        public Client Client { get; set; }

        public long? SalesId { get; set; }
        public Employee Sales { get; set; }

        [Required]
        public string OrderContent { get; set; }

        public bool IsSignedByClient { get; set; }
        public DateTime? SignedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public OrderStatus Status { get; set; }

        public long? ProjectCardId { get; set; }
        public ProjectCard ProjectCard { get; set; }
    }

    public enum OrderStatus
    {
        Sent = 0,           // Wysłane do klienta
        Signed = 1,         // Podpisane przez klienta
        ProjectCardSent = 2,// Karta projektu wysłana
        Completed = 3       // Zakończone
    }
}