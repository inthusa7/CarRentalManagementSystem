using System.ComponentModel.DataAnnotations;

namespace CarRentalManagementSystem.Models
{
    public class BookingCreateViewModel
    {
        public int CarID { get; set; }

        // Display only info about the car, not editable
        public string CarName { get; set; } = "";
        public string CarModel { get; set; } = "";
        public decimal DailyRate { get; set; }
        public string ImageUrl { get; set; } = "";

        [Required(ErrorMessage = "Pickup Date is required")]
        [DataType(DataType.Date)]
        public DateTime PickupDate { get; set; }

        [Required(ErrorMessage = "Return Date is required")]
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }

        [Required(ErrorMessage = "Customer Name is required")]
        public string CustomerName { get; set; } = "";
    }
}
