using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using UI.Data;
using UI.Models;
using UI.Security;

namespace UI.Services
{
    public class MockListItemRepository : IListItemRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IDataProtector protector;

        public MockListItemRepository(AppDbContext appDbContext,  IDataProtectionProvider dataProtectionProvider, DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _appDbContext = appDbContext;
            protector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.IdRouteValue);
        }
        #region -----Get List of ListItem-------
        public IEnumerable<ListItem> GetAllListItem()
        {
            var listItemLst = from listItems in _appDbContext.ListItems select listItems;

            // var listItemLst = _appDbContext.ListItems.ToList();
            return listItemLst;
        }
        #endregion

        #region ------Get Single ListItem------
        public ListItem GetListItem(int Id)
        {
            ListItem listItemLst = (from lst in _appDbContext.ListItems where lst.ListItemId==Id join category in _appDbContext.ListItemCategories on lst.ListItemCategoryId equals category.ListItemCategoryId
                                              select new ListItem
                                              {
                                                  ListItemId = lst.ListItemId,
                                                  ListItemCategory = category,
                                                  ListItemCategoryId = lst.ListItemCategoryId,
                                                  ListItemName = lst.ListItemName
                                              }).FirstOrDefault();

            return listItemLst;
             //return _appDbContext.ListItems.Where(l => l.ListItemId == Id).FirstOrDefault();

        }
        #endregion

        #region -----Check List Item Is in Use-----
        public bool ListItemAssignToPerson(int listItemId)
        {
            bool listItemIsInUse = _appDbContext.Persons.Any(p => p.GenderListItemId == listItemId);
            if (listItemIsInUse)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region -----ListItem Delete------
        public ListItem ListItemDel(int id)
        {
            ListItem listItemToDel = GetListItem(id);
            if (listItemToDel != null)
            {
                _appDbContext.ListItems.Remove(listItemToDel);
                _appDbContext.SaveChanges();
            }
            return listItemToDel;
        }
        #endregion

        #region ----- List Item Insert------
        public ListItem ListItemIns(ListItem listItem)
        {
            _appDbContext.ListItems.Add(listItem);
            _appDbContext.SaveChanges();
            return listItem;
        }
        #endregion

        public bool ListItemNameExists(string listItemName)
        {
            bool isListItemNameExists = _appDbContext.ListItems.Any(l => l.ListItemName == listItemName);
            if (isListItemNameExists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region ----- ListItem Update---------
        public ListItem ListItemUpd(ListItem listItem)
        {
            ListItem listItemToUpdate = GetListItem(listItem.ListItemId);
            if (listItemToUpdate != null)
            {
                listItemToUpdate.ListItemCategoryId = listItem.ListItemCategoryId;
                listItemToUpdate.ListItemName = listItem.ListItemName;
                _appDbContext.ListItems.Update(listItemToUpdate);
                _appDbContext.SaveChanges();
            }
            return listItemToUpdate;
        }
        #endregion


        public JsonResult GetListItemsForDataTable(string draw, string start, string length, string sortColumn, string sortColumnDirection, string searchValue, int pageSize, int skip)
        {
            int recordsTotal = 0;
            List<ListItem> listItems = new List<ListItem>();
            listItems = (from lstItem in _appDbContext.ListItems join category in _appDbContext.ListItemCategories on lstItem.ListItemCategoryId equals category.ListItemCategoryId
                         select new ListItem
                         {
                             EncryptedId = protector.Protect(lstItem.ListItemId.ToString()),
                             ListItemCategory = category,
                             ListItemCategoryId= lstItem.ListItemCategoryId,
                             ListItemName = lstItem.ListItemName
                         }).Skip(skip).Take(pageSize).ToList();
            //roleList = _appDbContext.Roles.Skip(skip).Take(pageSize).ToList();
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                listItems = listItems.OrderByDescending(s => sortColumn + " " + sortColumnDirection).ToList();
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                listItems = listItems.Where(l => l.ListItemName.ToLower().Contains(searchValue.ToLower()) || l.ListItemCategory.ListItemCategoryName.ToLower().Contains(searchValue.ToLower())).ToList();

            }
            //recordsTotal = _appDbContext.Roles.Count();
            recordsTotal = (from lstItems in _appDbContext.ListItems select lstItems).Count();
            var data = listItems;
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return new JsonResult(jsonData);
        }

        public IEnumerable<ListItem> GetAllListItemById(int id)
        {
            var listItemLst = from listItems in _appDbContext.ListItems where listItems.ListItemCategoryId == id select listItems;

            // var listItemLst = _appDbContext.ListItems.ToList();
            return listItemLst;
        }
    }
}
