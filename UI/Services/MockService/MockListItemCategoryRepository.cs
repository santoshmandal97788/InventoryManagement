using Microsoft.AspNetCore.Mvc;
using UI.Data;
using UI.Models;

namespace UI.Services
{
    public class MockListItemCategoryRepository : IListItemCategoryRepository
    {
        private readonly AppDbContext _appDbContext;

        public MockListItemCategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        #region ------Get ALl listItem Category---------
        public IEnumerable<ListItemCategory> GetAllListItemCategory()
        {
           
            var listItemCategoryLst = from cat in _appDbContext.ListItemCategories select cat; ;
            //var listItemCategoryLst = _appDbContext.ListItemCategories.ToList();
            return listItemCategoryLst;
        }
        #endregion

        #region ------ Get Single ListItem Category------
        public ListItemCategory GetListItemCategory(int Id)
        {

            return _appDbContext.ListItemCategories.Where(l => l.ListItemCategoryId == Id).FirstOrDefault();

        }
        #endregion

        #region ------ Delete ListItem Category--------
        public ListItemCategory ListItemCategoryDel(int id)
        {
            ListItemCategory lstItemCategoryToDel = GetListItemCategory(id);
            if (lstItemCategoryToDel != null)
            {
                _appDbContext.ListItemCategories.Remove(lstItemCategoryToDel);
                _appDbContext.SaveChanges();
            }
            return lstItemCategoryToDel;
        }
        #endregion

        #region ------Checks ListItem CategoryName Exists or Not-----------
        public bool ListItemCategoryExists(string categoryName)
        {
            bool isListItemCategoryExists = _appDbContext.ListItemCategories.Any(l => l.ListItemCategoryName == categoryName);
            if (isListItemCategoryExists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region ------- ListItem Category insert--------
        public ListItemCategory ListItemCategoryIns(ListItemCategory listItemCategory)
        {
            _appDbContext.ListItemCategories.Add(listItemCategory);
            _appDbContext.SaveChanges();
            return listItemCategory;
        }
        #endregion

        #region -------ListItem Category Update--------
        public ListItemCategory ListItemCategoryUpd(ListItemCategory listItemCategory)
        {
            ListItemCategory  listItemCategoryToUpdate = GetListItemCategory(listItemCategory.ListItemCategoryId);
            if (listItemCategoryToUpdate != null)
            {
                listItemCategoryToUpdate.ListItemCategoryName = listItemCategory.ListItemCategoryName;
                _appDbContext.ListItemCategories.Update(listItemCategoryToUpdate);
                _appDbContext.SaveChanges();
            }
            return listItemCategoryToUpdate;
        }
        #endregion


        public JsonResult GetListItemCatForDataTable(string draw, string start, string length, string sortColumn, string sortColumnDirection, string searchValue, int pageSize, int skip)
        {
            int recordsTotal = 0;
            List<ListItemCategory> listItemCategory= new List<ListItemCategory>();
            listItemCategory = (from cat in _appDbContext.ListItemCategories select cat).Skip(skip).Take(pageSize).ToList();
            //roleList = _appDbContext.Roles.Skip(skip).Take(pageSize).ToList();
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                listItemCategory = listItemCategory.OrderByDescending(s => sortColumn + " " + sortColumnDirection).ToList();
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                listItemCategory = listItemCategory.Where(c => c.ListItemCategoryId.ToString().Contains(searchValue.ToLower()) || c.ListItemCategoryName.ToLower().Contains(searchValue.ToLower())).ToList();

            }
            recordsTotal = _appDbContext.ListItemCategories.Count();
            var data = listItemCategory;
            var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return new JsonResult(jsonData);
        }
        public bool ListItemCategoryIsInUse(int listItemCatId)
        {
           bool listItemCategoryIsInUse = _appDbContext.ListItems.Any(l => l.ListItemCategoryId == listItemCatId);
            if (listItemCategoryIsInUse)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
