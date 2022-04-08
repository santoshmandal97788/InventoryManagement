using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UI.ViewModel
{
    public class ListItemCategoryViewModel
    {
        public int ListItemCategoryId { get; set; }

        [Required(ErrorMessage = "ListItem Category Name is required")]
        [StringLength(30, MinimumLength = 3)]
        [Remote(action: "IsCategoryExists", controller: "ListItemCategory", AdditionalFields = "ListItemCategoryId")]
        public string ListItemCategoryName { get; set; }
    }
}
