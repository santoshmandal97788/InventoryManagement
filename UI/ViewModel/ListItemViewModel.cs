using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UI.ViewModel
{
    public class ListItemViewModel
    {
        public int ListItemId { get; set; }

        [Required(ErrorMessage = "Please Select Category Name")]
        public int ListItemCategoryId { get; set; }

        [Display(Name ="Category Name")]
        public string? ListItemCategoryName { get; set; }

        [Required(ErrorMessage = "ListItem  Name is required")]
        [StringLength(30, MinimumLength = 3)]
        [Remote(action: "IsListItemNameExists", controller: "ListItem", AdditionalFields = "ListItemId")]
        [Display(Name = "ListItem Name")]
        public string ListItemName { get; set; }

      
    }
}
