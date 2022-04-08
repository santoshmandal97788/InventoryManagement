using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    [Index(nameof(Email), nameof(PersonId), IsUnique = true)]
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public int  PersonId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int  RoleId { get; set; }
        public Person Person { get; set; }
        public Role Role { get; set; }
       // public ListItem ListItem { get; set; }
    }
}
