using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UI.ViewModel
{
    public class RoleViewModel
    {
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Role Name is required")]
        [StringLength(15, MinimumLength = 2)]
        [Remote(action: "IsRoleExists", controller: "Administration", AdditionalFields = "RoleId")]
        public string RoleName { get; set; }
    }
}
