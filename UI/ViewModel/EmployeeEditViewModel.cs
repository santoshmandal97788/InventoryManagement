using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UI.ViewModel
{
    public class EmployeeEditViewModel
    {
        public int EmployeeId { get; set; }
        public int PersonId { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        [Remote(action: "IsEmailExists", controller: "Employee", AdditionalFields = "EmployeeId")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Select Gender")]
        public int GenderId { get; set; }
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Please Select Role")]
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
    }
}
