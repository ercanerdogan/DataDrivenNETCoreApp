using BethanysPieShopAdmin.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BethanysPieShopAdmin.Controllers
{
    public class PieController : Controller
    {
        private readonly IPieRepository _pieRepository;

        public PieController(IPieRepository pieRepository)
        {
            _pieRepository = pieRepository;
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
    }
}
