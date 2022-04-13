using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UI.Models
{
    [Index(nameof(ListItemCategoryName), IsUnique = true)]
    public class ListItemCategory
    {
        [Key]
        public int ListItemCategoryId { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }
        public string ListItemCategoryName { get; set; }
        

    }
}
