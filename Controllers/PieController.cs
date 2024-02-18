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
            
            var pieAddViewModel = await prepareAddViewModel();
            return View(pieAddViewModel);
        }

        private async Task<PieAddViewModel> prepareAddViewModel()
        {
            var listCategories = await _categoryRepository.GetAllCategoriesAsync();

            var pieAddViewModel = new PieAddViewModel()
            {
                Categories = listCategories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                })
            };

            return pieAddViewModel;
        }

        [HttpPost]
        public async Task<IActionResult> Add(PieAddViewModel pieAddViewModel)
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

            pieAddViewModel = await prepareAddViewModel();

            return View(pieAddViewModel);
        }
    }
}
