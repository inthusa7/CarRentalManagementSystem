using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalManagementSystem.Models
{
    public class Car
    {
        [Key]
        public int CarID { get; set; }

        [Required, MaxLength(100)]
        public string CarName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string CarModel { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string ImageUrl { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DailyRate { get; set; } = 50m;

        // Navigation
        public ICollection<Booking>? Bookings { get; set; }
    }
}
