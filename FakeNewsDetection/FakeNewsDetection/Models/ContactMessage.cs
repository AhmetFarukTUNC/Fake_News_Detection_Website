using System.ComponentModel.DataAnnotations;

namespace FakeNewsMVC.Models
{
    public class ContactMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }
    }
}
