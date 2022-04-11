using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Models;
using UI.Services;
using UI.ViewModel;

namespace UI.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class ListItemCategoryController : Controller
    {
        private readonly IListItemCategoryRepository _mockListItemCategoryRepository;

        public ListItemCategoryController(IListItemCategoryRepository mockListItemCategoryRepository)
        {
            _mockListItemCategoryRepository = mockListItemCategoryRepository;
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult IsCategoryExists(string categoryName, int listItemCateegoryId)
        {
            bool isExists = false;
            if (listItemCateegoryId == 0)
            {
                isExists = _mockListItemCategoryRepository.ListItemCategoryExists(categoryName);
            }
            else
            {
                isExists = _mockListItemCategoryRepository.ListItemCategoryExists(categoryName);
                var categoryById = _mockListItemCategoryRepository.GetListItemCategory(listItemCateegoryId);
                if (isExists && categoryById.ListItemCategoryName != categoryName)
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
                return Json($"Category Name: {categoryName}  already exists in Database.");
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
        public IActionResult GetAllCategory()
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
                var jsonData = _mockListItemCategoryRepository.GetListItemCatForDataTable(draw, start, length, sortColumn, sortColumnDirection, searchValue, pageSize, skip);
                return Ok(jsonData.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(ListItemCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isexists = _mockListItemCategoryRepository.ListItemCategoryExists(model.ListItemCategoryName);
                if (isexists)
                {
                    ModelState.AddModelError("ListItemCategoryName", "ListItemCategoryName Already Exists");
                    return View(model);
                }

                ListItemCategory listItemCategory = new ListItemCategory
                {
                    ListItemCategoryName = model.ListItemCategoryName
                };
                _mockListItemCategoryRepository.ListItemCategoryIns(listItemCategory);
                TempData["message"] = "List Item Category Added Successfully.";
                return RedirectToAction("Index", "ListItemCategory");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ListItemCategory listItemCategory = _mockListItemCategoryRepository.GetListItemCategory(id);
            if (listItemCategory == null)
            {
                string msg = $"ListItemCategory with id: {id}, you are looking cannot be found";
                TempData["errMessage"] = msg;
                return RedirectToAction("NotFound", "Error");
            }
            ListItemCategoryViewModel listItemCategoryViewModel = new ListItemCategoryViewModel
            {
                ListItemCategoryId  = listItemCategory.ListItemCategoryId,
                ListItemCategoryName = listItemCategory.ListItemCategoryName,
            };
            return View(listItemCategoryViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ListItemCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isexists = _mockListItemCategoryRepository.ListItemCategoryExists(model.ListItemCategoryName);
                ListItemCategory listItemCategory = _mockListItemCategoryRepository.GetListItemCategory(model.ListItemCategoryId);
                if (isexists && listItemCategory.ListItemCategoryName != model.ListItemCategoryName)
                {
                    ModelState.AddModelError("ListItemCategoryName", "ListItem CategoryName Already Exist");
                    return View(model);
                }
                listItemCategory.ListItemCategoryName = model.ListItemCategoryName;
                ListItemCategory updatedCategory = _mockListItemCategoryRepository.ListItemCategoryUpd(listItemCategory);
                TempData["message"] = "ListItem  Category Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var category = _mockListItemCategoryRepository.GetListItemCategory(id);
            if (category == null)
            {
                Response.StatusCode = 404;
                string msg = $"ListItemCategory with id: {id}, cannot be found";
                TempData["errMessage"] = msg;
                return RedirectToAction("NotFound", "Error");
            }
            ListItemCategoryViewModel listItemCategoryViewModel = new ListItemCategoryViewModel() { ListItemCategoryId = category.ListItemCategoryId, ListItemCategoryName = category.ListItemCategoryName };
            return View(listItemCategoryViewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken] //On uncomment Error  400 on AJax Call Request
        public IActionResult Delete(int id)
        {
            var isListItemCategoryInUse = _mockListItemCategoryRepository.ListItemCategoryIsInUse(id);
            if (isListItemCategoryInUse)
            {
                return Json(new { success = false, message = " Can not Delete! Category Is in Use." });
            }
            ListItemCategory categoryToDelete = _mockListItemCategoryRepository.GetListItemCategory(id);
            if (categoryToDelete != null)
            {
                _mockListItemCategoryRepository.ListItemCategoryDel(id);
                return Json(new { success = true, message = "Deleted Successfully" });
            }
            return Json(new { success = false, message = "Something Went Wrong" });
        }
    }
}



