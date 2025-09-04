using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalManagementSystem.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }



        [Required]
        [ForeignKey("Car")]
        public int CarID { get; set; }
        public Car Car { get; set; }



        [Required]
        [DataType(DataType.Date)]
        public DateTime PickupDate { get; set; }



        [Required]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalCost { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ReturnDate < PickupDate)
            {
                yield return new ValidationResult(
                    "Return date cannot be earlier than Pickup date",
                    new[] { nameof(ReturnDate) });
            }
        }
    }
}
