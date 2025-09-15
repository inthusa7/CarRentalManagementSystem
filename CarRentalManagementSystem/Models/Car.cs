using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http; // For IFormFile

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

        // For file upload only, not mapped to DB
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public bool IsAvailable { get; set; } = true;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DailyRate { get; set; } = 50m;

        // Navigation property
        public ICollection<Booking>? Bookings { get; set; }
    }
}
