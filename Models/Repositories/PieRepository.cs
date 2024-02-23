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

        public async Task<int> UpdatePieAsync(Pie pie)
        {
            if (_context.Pies.Any(p => p.Name == pie.Name && p.PieId != pie.PieId))
            {
                throw new InvalidOperationException("Pie already exists");
            }

            var pieToUpdate = await _context.Pies.FirstOrDefaultAsync(p => p.PieId == pie.PieId);
            if (pieToUpdate != null)
            {
                _context.Entry(pieToUpdate).Property("RowVersion").OriginalValue = pie.RowVersion;
                pieToUpdate.Name = pie.Name;
                pieToUpdate.Price = pie.Price;
                pieToUpdate.ShortDescription = pie.ShortDescription;
                pieToUpdate.LongDescription = pie.LongDescription;
                pieToUpdate.CategoryId = pie.CategoryId;
                pieToUpdate.ImageUrl = pie.ImageUrl;
                pieToUpdate.InStock = pie.InStock;
                pieToUpdate.IsPieOfTheWeek = pie.IsPieOfTheWeek;
                pieToUpdate.ImageThumbnailUrl = pie.ImageThumbnailUrl;

                _context.Pies.Update(pieToUpdate);
                return await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Pie to update not found");
            }
        }

        public async Task<int> DeletePieAsync(int pieId)
        {
            var pieToDelete = await _context.Pies.FirstOrDefaultAsync(p => p.PieId == pieId);
            if (pieToDelete != null)
            {
                _context.Pies.Remove(pieToDelete);
                return await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("Pie to delete not found");
            }
        }

        public async Task<int> GetAllPiesCountAsync()
        {
            return await _context.Pies.CountAsync();
        }

        public async Task<IEnumerable<Pie>> GetPiesPagedAsync(int? pageNumber, int pageSize)
        {
            IQueryable<Pie> pies = _context.Pies
                .AsNoTracking();

            pageNumber ??= 1;

            return await pies
                .Skip((pageNumber.Value - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pie>> GetPiesSortedAndPagedAsync(string sortBy, int? pageNumber, int pageSize)
        {
            IQueryable<Pie> pies = _context.Pies.AsNoTracking();

            pies = sortBy switch
            {
                "name_desc" => pies.OrderByDescending(p => p.Name),
                "name" => pies.OrderBy(p => p.Name),
                "id_desc" => pies.OrderByDescending(p => p.PieId),
                "id" => pies.OrderBy(p => p.PieId),
                "price_desc" => pies.OrderByDescending(p => p.Price),
                "price" => pies.OrderBy(p => p.Price),
                _ => pies
            };

            pageNumber ??= 1;

            return await pies
                .Skip((pageNumber.Value - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Pie>> SearchPiesAsync(string searchQuery, int? categoryId)
        {
            IQueryable<Pie> pies = _context.Pies.AsNoTracking();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                pies = pies.Where(p => p.Name.Contains(searchQuery) ||
                    p.ShortDescription.Contains(searchQuery) ||
                    p.LongDescription.Contains(searchQuery));
            }

            if (categoryId.HasValue)
            {
                pies = pies.Where(p => p.CategoryId == categoryId);
            }

            return await pies.ToListAsync();
        }
    }

}
