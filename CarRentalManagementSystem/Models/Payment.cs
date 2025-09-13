using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalManagementSystem.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [Required]
        [ForeignKey("Booking")]
        public int BookingID { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required, MaxLength(50)]
        public string PaymentMethod { get; set; } = "Cash"; // Cash, Card, etc.

        public Booking Booking { get; set; }
    }
}
