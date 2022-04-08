using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    [Index(nameof(ListItemCategoryName), IsUnique = true)]
    public class ListItemCategory
    {
        [Key]
        public int ListItemCategoryId { get; set; }
        public string ListItemCategoryName { get; set; }
        

    }
}
