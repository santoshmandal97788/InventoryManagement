using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UI.Models
{
    [Index(nameof(RoleName), IsUnique = true)]
    public class Role
    {
        [Key]
        public int RoleId { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }
        public string RoleName { get; set; }

    }
}
