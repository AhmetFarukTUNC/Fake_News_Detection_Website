using System.ComponentModel.DataAnnotations;

namespace FakeNewsMVC.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; } // Şifreyi hashleyerek saklamak güvenlidir
    }
}
