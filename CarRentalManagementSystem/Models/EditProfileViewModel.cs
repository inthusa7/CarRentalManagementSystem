using System.ComponentModel.DataAnnotations;

namespace CarRentalManagementSystem.Models
{
    public class EditProfileViewModel
    {
        [Required]
        public int UserID { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; } // Readonly, cannot be changed

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact No is required.")]
        [Display(Name = "Contact No")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string ContactNo { get; set; }

        [Display(Name = "Alternate Contact No")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string AltContactNo { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Driver License No")]
        public string DriverLicenseNo { get; set; }
    }
}
