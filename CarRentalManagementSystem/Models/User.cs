using System.ComponentModel.DataAnnotations;

namespace CarRentalManagementSystem.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required, MaxLength(50)]
        public string Password { get; set; } // plain text as per assignment

        [Required, MaxLength(20)]
        public string Role { get; set; } // "Admin" or "Customer"
    }
}
