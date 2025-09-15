using System.ComponentModel.DataAnnotations;

namespace CarRentalManagementSystem.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }  // Primary key

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(50)]
        public string Password { get; set; } = string.Empty; // plain text as per assignment

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact number is required")]
        [Phone(ErrorMessage = "Invalid Contact Number")]
        public string ContactNo { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid Alternative Contact Number")]
        public string AltContactNo { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Driver License Number is required")]
        public string DriverLicenseNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        [StringLength(20)]
        public string Role { get; set; } = "Customer";
    }
}
