using Microsoft.AspNetCore.Mvc;
using MVC_Project.Contract;
using MVC_Project.IRepository;
using MVC_Project.Models;
using MVC_Project.Repository;

namespace MVC_Project.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(Pagination pagination)
        {
            if (pagination.PageNumber <= 0) pagination.PageNumber = 1;
            if (pagination.PageSize <= 0) pagination.PageSize = 10;
            if (pagination.SortColumn == null) pagination.SortColumn = "CategoryId";
            if (!pagination.SortDesc) pagination.SortDesc = false;
            var category = await _unitOfWork.CategoryRepository.GetCategories(pagination);
            return View(category);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.CategoryRepository.AddCategory(entity);
                    //await _unitOfWork.SaveAsync();
                    TempData["SuccessMessage"] = "Product created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error creating product: {ex.Message}";
                    return View(entity);
                }
            }
            return View(entity);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategoryById(id);
            if (category == null)
            {
                TempData["ErrorMessage"] = "Category not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category entity)
        {
            if (id != entity.CategoryId)
            {
                TempData["ErrorMessage"] = "Category ID mismatch!";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.CategoryRepository.update(entity);
                    TempData["SuccessMessage"] = "Category updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error updating category: {ex.Message}";
                    return View(entity);
                }
            }
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var category = await _unitOfWork.CategoryRepository.GetCategoryById(id);
                if (category != null)
                {
                    await _unitOfWork.CategoryRepository.Delete(id);
                    TempData["SuccessMessage"] = "Category deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Category not found!";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting category: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
