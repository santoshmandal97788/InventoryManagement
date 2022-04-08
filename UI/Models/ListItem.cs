using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    [Index(nameof(ListItemName), IsUnique = true)]
    public class ListItem
    {
        [Key]
        public int ListItemId { get; set; }
        public int ListItemCategoryId { get; set; }
        public string ListItemName { get; set; }
        public ListItemCategory ListItemCategory { get; set; }

    }
}
