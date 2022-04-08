using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UI.Models
{
    public class Person
    {
        [Key]
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public int GenderListItemId { get; set; }
      
        [ForeignKey("GenderListItemId")]
        public ListItem ListItem { get; set; }
        

    }
}
