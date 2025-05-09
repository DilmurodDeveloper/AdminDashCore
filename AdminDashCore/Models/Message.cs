using System;
using System.ComponentModel.DataAnnotations;

namespace AdminDashCore.Models
{
    public class Message
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public Client? Client { get; set; }

        [Required]
        public string? Content { get; set; }

        public DateTime SentAt { get; set; } = DateTime.Now;

        public bool IsRead { get; set; }
    }
}
