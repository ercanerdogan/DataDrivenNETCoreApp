using BethanysPieShopAdmin.Models;
using BethanysPieShopAdmin.Models.Repositories;
using BethanysPieShopAdmin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BethanysPieShopAdmin.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;
        private readonly ICategoryRepository _categoryRepository;

        public PieController(IPieRepository pieRepository,
            ICategoryRepository categoryRepository)
        {
            _pieRepository = pieRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var pieList = await _pieRepository.GetAllPiesAsync();

            return View(pieList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pie = await _pieRepository.GetPieByIdAsync(id.Value);
            if (pie == null)
            {
                return NotFound();
            }

            return View(pie);
        }

        public async Task<IActionResult> Add()
        {

            var pieAddViewModel = await preparePieViewModel();
            return View(pieAddViewModel);
        }

        private async Task<PieViewModel> preparePieViewModel()
        {
            var listCategories = await _categoryRepository.GetAllCategoriesAsync();

            var pieAddViewModel = new PieViewModel()
            {
                Categories = listCategories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                })
            };

            return pieAddViewModel;
        }

        private async Task<IEnumerable<SelectListItem>> prepareListCategories()
        {
            var listCategories = await _categoryRepository.GetAllCategoriesAsync();

            var selectListItems = listCategories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            });

            return selectListItems;
        }

        [HttpPost]
        public async Task<IActionResult> Add(PieViewModel pieAddViewModel)
        {
            if (ModelState.IsValid)
            {
                var pie = new Pie
                {
                    CategoryId = pieAddViewModel.Pie.CategoryId,
                    Name = pieAddViewModel.Pie.Name,
                    ShortDescription = pieAddViewModel.Pie.ShortDescription,
                    LongDescription = pieAddViewModel.Pie.LongDescription,
                    Price = pieAddViewModel.Pie.Price,
                    AllergyInformation = pieAddViewModel.Pie.AllergyInformation,
                    ImageThumbnailUrl = pieAddViewModel.Pie.ImageThumbnailUrl,
                    ImageUrl = pieAddViewModel.Pie.ImageUrl,
                    InStock = pieAddViewModel.Pie.InStock,
                    IsPieOfTheWeek = pieAddViewModel.Pie.IsPieOfTheWeek
                };

                await _pieRepository.AddPieAsync(pie);

                return RedirectToAction(nameof(Index));
            }

            pieAddViewModel = await preparePieViewModel();

            return View(pieAddViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listCategories = await prepareListCategories();

            var pie = await _pieRepository.GetPieByIdAsync(id.Value);

            var pieViewModel = new PieViewModel
            {
                Categories = listCategories,
                Pie = pie
            };


            return View(pieViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PieViewModel pieViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _pieRepository.UpdatePieAsync(pieViewModel.Pie);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Updating pie failed, please try again! Error : {ex.Message}");
            }

            var listCategories = await prepareListCategories();
            pieViewModel.Categories = listCategories;
            return View(pieViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var pieToDelete = await _pieRepository.GetPieByIdAsync(id);
            if (pieToDelete == null)
            {
                return NotFound();
            }

            return View(pieToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? pieId)
        {
            if (pieId == null)
            {
                ViewData["ErrorMessage"] = "Deleting the pie failed, invalid ID!";
                return View();
            }

            try
            {
                await _pieRepository.DeletePieAsync(pieId.Value);

                TempData["PieDeleted"] = "Pie has been deleted successfully!";
                
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Deleting the pie failed, please try again! Error: {ex.Message}";
            }

            var pieToDelete = await _pieRepository.GetPieByIdAsync(pieId.Value);
            return View(pieToDelete);
        }
    }
}
