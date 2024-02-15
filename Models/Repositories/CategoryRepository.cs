using Microsoft.EntityFrameworkCore;

namespace BethanysPieShopAdmin.Models.Repositories;

internal class CategoryRepository : ICategoryRepository
{
    private readonly BethanyPieShopDbContext _context;

    public CategoryRepository(BethanyPieShopDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Category> GetAllCategories()
    {
        return _context.Categories.OrderBy(c=>c.CategoryId);
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.OrderBy(c=>c.CategoryId).ToListAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(int categoryId)
    {
        return await _context.Categories
            .Include(p => p.Pies)
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
    }
}