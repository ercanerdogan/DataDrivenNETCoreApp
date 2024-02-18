using Microsoft.EntityFrameworkCore;

namespace BethanysPieShopAdmin.Models.Repositories
{
    public class PieRepository : IPieRepository
    {
        private readonly BethanyPieShopDbContext _context;

        public PieRepository(BethanyPieShopDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pie>> GetAllPiesAsync()
        {
            var pieList = await _context.Pies
                .OrderBy(p => p.PieId)
                .AsNoTracking()
                .ToListAsync();

            return pieList;
        }

        public async Task<Pie?> GetPieByIdAsync(int PieId)
        {
            var pie = await _context.Pies
                .Include(p => p.Ingredients)
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PieId == PieId);


            return pie;
        }

        public Task<int> AddPieAsync(Pie pie)
        {
            _context.Pies.Add(pie);
            return _context.SaveChangesAsync();
        }
    }

}
