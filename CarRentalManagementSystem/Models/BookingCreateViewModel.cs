using System.ComponentModel.DataAnnotations;

namespace CarRentalManagementSystem.Models
{
    public class BookingCreateViewModel
    {
        public int CarID { get; set; }

        // Car Details
        public string CarName { get; set; } = "";
        public string CarModel { get; set; } = "";
        public decimal DailyRate { get; set; }
        public string ImageUrl { get; set; } = "";
        public int SeatCount { get; set; }
        public string CarColor { get; set; } = "";
        public string Location { get; set; } = "";
        public string Description { get; set; } = "";

        // Booking Details
        [Required(ErrorMessage = "Pickup Date is required")]
        [DataType(DataType.Date)]
        public DateTime PickupDate { get; set; }

        [Required(ErrorMessage = "Return Date is required")]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }

        public decimal TotalCost { get; set; }

        // Auto-filled from session
        public string CustomerName { get; set; } = "";
    }
}
