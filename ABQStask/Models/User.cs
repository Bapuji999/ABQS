using System.ComponentModel.DataAnnotations;

namespace ABQStask.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
        public bool isAdmin { get; set; }
        public bool isDeleted { get; set; }
    }
}
