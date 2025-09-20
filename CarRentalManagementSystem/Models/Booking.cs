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
        public int UserID { get; set; }   // UserID → CustomerID

        [Required]
        [ForeignKey("Car")]
        public int CarID { get; set; }

        [Required]
        public DateTime PickupDate { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalCost { get; set; }
        public bool IsPaid { get; set; }

        public User User { get; set; }
        public Car Car { get; set; }
        public Payment? Payment { get; set; }



    }
}
