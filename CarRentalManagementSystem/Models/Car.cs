using System.ComponentModel.DataAnnotations;

namespace CarRentalManagementSystem.Models
{
    public class Car
    {
        [Key]
        public int CarID { get; set; }

        [Required, MaxLength(100)]
        public string CarName { get; set; } = string.Empty;   // 👈 Default value

        [Required, MaxLength(50)]
        public string CarModel { get; set; } = string.Empty;  // 👈 Default value

        [MaxLength(200)]
        public string? ImageUrl { get; set; }                 // 👈 Nullable

        public bool IsAvailable { get; set; } = true;

        [DataType(DataType.Currency)]
        public decimal DailyRate { get; set; } = 50m;
    }
}
