using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UI.Models
{
    [Index(nameof(ListItemName), IsUnique = true)]
    public class ListItem
    {
        [Key]
        public int ListItemId { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }
        public int ListItemCategoryId { get; set; }
        public string ListItemName { get; set; }
        public ListItemCategory ListItemCategory { get; set; }

    }
}
