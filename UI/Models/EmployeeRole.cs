using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class EmployeeRole
    {
        [Key]
        public int EmployeeRoleId { get; set; }
        public int EmployeeId { get; set; }
        public int RoleId { get; set; }
        public Employee Employee { get; set; }
        public Role Role { get; set; }
    }
}
