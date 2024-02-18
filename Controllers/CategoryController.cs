using BethanysPieShopAdmin.Models;
using BethanysPieShopAdmin.Models.Repositories;
using BethanysPieShopAdmin.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopAdmin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var categoryListViewModel = new CategoryListViewModel
            {
                Categories = (await _categoryRepository.GetAllCategoriesAsync()).ToList()
            };

            return View(categoryListViewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _categoryRepository.GetCategoryByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add([Bind("Name, Description, DateAdded")] Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _categoryRepository.AddCategoryAsync(category);
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Adding category failed, please try again! Error : {ex.Message}");
            }

            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetCategoryByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _categoryRepository.UpdateCategoryAsync(category);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Updating the category failed try again! : Error {ex.Message}");
            }

            return View(category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? CategoryId)
        {
            if (CategoryId == null)
            {
                ViewData["ErrorMessage"] = "Deleting the category failed, invalid Id";
                return View();
            }

            try
            {
                await _categoryRepository.DeleteCategoryAsync(CategoryId.Value);
                TempData["CategoryDeleted"] = "Category deleted successfully";

                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Deleting the category failed, please try again! Error : {ex.Message}";
            }

            var category = await _categoryRepository.GetCategoryByIdAsync(CategoryId.Value);
            return View(category);
        }

    }
}
