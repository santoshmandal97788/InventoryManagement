using Microsoft.AspNetCore.Mvc;
using UI.Models;

namespace UI.Services
{
    public interface IListItemCategoryRepository
    {
        ListItemCategory GetListItemCategory(int Id);
        IEnumerable<ListItemCategory> GetAllListItemCategory();
        ListItemCategory ListItemCategoryIns(ListItemCategory listItemCategory);
        ListItemCategory ListItemCategoryUpd(ListItemCategory listItemCategory);
        ListItemCategory ListItemCategoryDel(int id);
        bool ListItemCategoryExists(string categoryName);
        bool ListItemCategoryIsInUse(int listItemCatId);
        JsonResult GetListItemCatForDataTable(string draw, string start, string length, string sortColumn, string sortColumnDirection, string searchValue, int pageSize, int skip);

    }
}
