
using System.ComponentModel.DataAnnotations;

namespace UI.ViewModel
{
    public class ResetPasswordConfirmEmailViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
