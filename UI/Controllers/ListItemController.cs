using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Models;
using UI.Services;
using UI.ViewModel;

namespace UI.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class ListItemController : Controller
    {
        private readonly IListItemRepository _mockListItemRepository;
        private readonly IListItemCategoryRepository _mockListItemCategoryRepository;
       

        public ListItemController(IListItemRepository mockListItemRepository, IListItemCategoryRepository mockListItemCategoryRepository)
        {
            _mockListItemRepository = mockListItemRepository;
            _mockListItemCategoryRepository = mockListItemCategoryRepository;
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsListItemNameExists(string listItemName, int listItemId)
        {
            bool isExists = false;
            if (listItemId == 0)
            {
                isExists = _mockListItemRepository.ListItemNameExists(listItemName);
            }
            else
            {
                isExists = _mockListItemRepository.ListItemNameExists(listItemName);
                var listItemById = _mockListItemRepository.GetListItem(listItemId);
                if (isExists && listItemById.ListItemName != listItemName)
                {
                    isExists = true;
                }
                else
                {
                    isExists = false;
                }

            }
            if (isExists)
            {
                return Json($"ListItem Name: {listItemName}  already exists in Database.");
            }
            else
            {
                return Json(true);
            }

        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult GetAllListItems()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                //var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumn = Request.Form["order[0][column]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                var jsonData = _mockListItemRepository.GetListItemsForDataTable(draw, start, length, sortColumn, sortColumnDirection, searchValue, pageSize, skip);
                return Ok(jsonData.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public IActionResult AddListItem()
        {
            var categoryList = _mockListItemCategoryRepository.GetAllListItemCategory().Select(p => new SelectListItem
            {
                Value = p.ListItemCategoryId.ToString(),
                Text = p.ListItemCategoryName
            }).ToList();
            ViewBag.Category = categoryList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddListItem(ListItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isexists = _mockListItemRepository.ListItemNameExists(model.ListItemName);
                if (isexists)
                {
                    ModelState.AddModelError("ListItemName", "ListItemName Already Exists");
                    return View(model);
                }

                ListItem listItem= new ListItem
                {
                    ListItemCategoryId = model.ListItemCategoryId,
                    ListItemName = model.ListItemName
                };
                _mockListItemRepository.ListItemIns(listItem);
                return RedirectToAction("Index", "ListItem");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ListItem listItem = _mockListItemRepository.GetListItem(id);
            if (listItem == null)
            {
                return View("NotFound", id);
            }
            var categoryList = _mockListItemCategoryRepository.GetAllListItemCategory().Select(p => new SelectListItem
            {
                Value = p.ListItemCategoryId.ToString(),
                Text = p.ListItemCategoryName
            }).ToList();
            ViewBag.Category = categoryList;
            ListItemViewModel listItemViewModel = new ListItemViewModel
            {
                ListItemId = listItem.ListItemId,
                ListItemCategoryId = listItem.ListItemCategoryId,
                ListItemCategoryName = listItem.ListItemCategory.ListItemCategoryName,
                ListItemName = listItem.ListItemName
            };
            return View(listItemViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ListItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isexists = _mockListItemRepository.ListItemNameExists(model.ListItemName);
                ListItem listItem = _mockListItemRepository.GetListItem(model.ListItemId);
                if (isexists && listItem.ListItemName != model.ListItemName)
                {
                    ModelState.AddModelError("ListItemName", "ListItem Name Already Exist");
                    return View(model);
                }
                listItem.ListItemCategoryId = model.ListItemCategoryId;
                listItem.ListItemName = model.ListItemName;
                ListItem updatedListItem = _mockListItemRepository.ListItemUpd(listItem);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var listItem = _mockListItemRepository.GetListItem(id);
            if (listItem == null)
            {
                Response.StatusCode = 404;
                return View("NotFound", id);
            }
            ListItemViewModel listItemViewModel = new ListItemViewModel() { ListItemId = listItem.ListItemId, ListItemCategoryId = listItem.ListItemCategoryId, ListItemCategoryName= listItem.ListItemCategory.ListItemCategoryName, ListItemName= listItem.ListItemName };
            return View(listItemViewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var isListItemInUse = _mockListItemRepository.ListItemAssignToPerson(id);
            if (isListItemInUse)
            {
                return Json(new { success = false, message = " Can not Delete! List Item Is in Use." });
            }
            ListItem listItemToDelete = _mockListItemRepository.GetListItem(id);
            if (listItemToDelete != null)
            {
                _mockListItemRepository.ListItemDel(id);
                return Json(new { success = true, message = "Deleted Successfully" });
            }
            return Json(new { success = false, message = "Something Went Wrong" });
        }
    }
}
