using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    [Index(nameof(RoleName), IsUnique = true)]
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }

    }
}
