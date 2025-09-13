using System.ComponentModel.DataAnnotations;

namespace CarRentalManagementSystem.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Password { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Role { get; set; } // "Admin" or "Customer"

        [Required, MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Navigation
        public ICollection<Booking>? Bookings { get; set; }
    }
}
