using System.ComponentModel.DataAnnotations;

namespace UI.ViewModel
{
    public class ResetPasswordViewModel
    {
        //[Required(ErrorMessage = "Email is required")]
        //public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [StringLength(15, MinimumLength = 5)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage =
            "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
