using Microsoft.AspNetCore.Mvc;
using UI.Models;

namespace UI.Services
{
    public interface IListItemRepository
    {
        ListItem GetListItem(int Id);
        IEnumerable<ListItem> GetAllListItem();
        ListItem ListItemIns(ListItem listItem);
        ListItem ListItemUpd(ListItem listItem);
        ListItem ListItemDel(int id);
        bool ListItemNameExists(string listItemName);
        bool ListItemAssignToPerson(int listItemId);
        JsonResult GetListItemsForDataTable(string draw, string start, string length, string sortColumn, string sortColumnDirection, string searchValue, int pageSize, int skip);
        IEnumerable<ListItem> GetAllListItemById(int id);
    }
}
