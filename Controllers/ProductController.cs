using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Project.Contract;
using MVC_Project.IRepository;
using MVC_Project.Models;

namespace MVC_Project.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(Pagination pagination)
        {
            if (pagination.PageNumber <= 0) pagination.PageNumber = 1;
            if (pagination.PageSize <= 0) pagination.PageSize = 5; 
            if (pagination.SortColumn == null) pagination.SortColumn = "ProductId";  
            if (!pagination.SortDesc) pagination.SortDesc = false;
            var productResult = await _unitOfWork.ProductRepository.GetProducts(pagination);
            var totalRecords = productResult.TotalProductsCount;

            var totalPages = (int)Math.Ceiling((double)totalRecords / pagination.PageSize);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { productResult.Products, totalPages });
            }

            return View(productResult.Products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var pagination = new Pagination
            {
                SkipPagination = true,
            };
            var categories = await _unitOfWork.CategoryRepository.GetCategories(pagination); 
            ViewBag.CategoryId = new SelectList(categories.Categories, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.ProductRepository.AddProduct(entity);
                    await _unitOfWork.SaveAsync(); 
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

        // GET: Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductById(id);
            product.ModifiedAt = DateTime.Now;
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction(nameof(Index));
            }

            var pagination = new Pagination { SkipPagination = true };
            var categories = await _unitOfWork.CategoryRepository.GetCategories(pagination);
            ViewBag.CategoryId = new SelectList(categories.Categories, "CategoryId", "Name");

            return View(product);
        }

        // POST: Edit
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var productToUpdate = await _unitOfWork.ProductRepository.GetProductById(entity.ProductId);
                    if (productToUpdate == null)
                    {
                        TempData["ErrorMessage"] = "Product not found.";
                        return RedirectToAction(nameof(Index));
                    }
                    productToUpdate.Name = entity.Name;
                    productToUpdate.Description = entity.Description;
                    productToUpdate.Price = entity.Price;
                    productToUpdate.CategoryId = entity.CategoryId;
                    productToUpdate.DiscountPrice = entity.DiscountPrice;
                    productToUpdate.StockQuantity = entity.StockQuantity;
                    productToUpdate.ImageUrl = entity.ImageUrl;

                    await _unitOfWork.ProductRepository.update(productToUpdate);
                    await _unitOfWork.SaveAsync();

                    TempData["SuccessMessage"] = "Product updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error updating product: {ex.Message}";
                    return View(entity);
                }
            }
            return View(entity);
        }

        // GET: Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductById(id);
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            return Json(new { success = true });
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetProductById(id);
                if (product == null)
                {
                    TempData["ErrorMessage"] = "Product not found.";
                    return RedirectToAction(nameof(Index));
                }

                await _unitOfWork.ProductRepository.Delete(id);
                await _unitOfWork.SaveAsync();

                TempData["SuccessMessage"] = "Product deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting product: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }


    }
}
