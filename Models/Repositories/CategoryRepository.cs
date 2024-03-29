using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BethanysPieShopAdmin.Models.Repositories;

internal class CategoryRepository : ICategoryRepository
{
    private readonly BethanyPieShopDbContext _context;
    private readonly IMemoryCache _memoryCache;
    private const string AllCategoriesCacheKey = "allCategories";

    public CategoryRepository(BethanyPieShopDbContext context,
        IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    public IEnumerable<Category> GetAllCategories()
    {
        return _context.Categories.OrderBy(c => c.CategoryId);
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        List<Category> categories;

        if (!_memoryCache.TryGetValue(AllCategoriesCacheKey, out categories))
        {
            categories = await _context.Categories.OrderBy(c => c.CategoryId).ToListAsync();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromMinutes(60)
            };

            _memoryCache.Set(AllCategoriesCacheKey, categories, cacheEntryOptions);
        }

        return categories;
    }

    public async Task<Category> GetCategoryByIdAsync(int categoryId)
    {
        return await _context.Categories
            .Include(p => p.Pies)
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
    }

    public async Task<int> AddCategoryAsync(Category category)
    {
        if (await _context.Categories.AnyAsync(c => c.Name == category.Name))
        {
            throw new InvalidOperationException("Category already exists");
        }

        _context.Categories.Add(category);

        InvalidateCache(); // cache invalidation

        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateCategoryAsync(Category category)
    {
        if (_context.Categories.Any(c => c.Name == category.Name && c.CategoryId != category.CategoryId))
        {
            throw new InvalidOperationException("Category already exists");
        }

        var categoryToUpdate = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);
        if (categoryToUpdate != null)
        {
            categoryToUpdate.Name = category.Name;
            categoryToUpdate.Description = category.Description;

            _context.Categories.Update(categoryToUpdate);
            return await _context.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException("The category to update can't be found");
        }
    }

    public async Task<int> DeleteCategoryAsync(int id)
    {
        var existingPies = _context.Pies.Any(p => p.CategoryId == id);
        if (existingPies)
        {
            throw new InvalidOperationException("Category can't be deleted as it is being used by pies");
        }

        var categoryToDelete = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
        if (categoryToDelete != null)
        {
            _context.Categories.Remove(categoryToDelete);
            return await _context.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException("The category to delete can't be found");
        }
    }

    public async Task<int> UpdateCategoryNamesAsync(List<Category> categories)
    {
        foreach (var category in categories)
        {
            var categoryToUpdate = await GetCategoryByIdAsync(category.CategoryId);

            if (categoryToUpdate != null)
            {
                categoryToUpdate.Name = category.Name;
                _context.Categories.Update(categoryToUpdate);
            }
        }

        var result = await _context.SaveChangesAsync();
        InvalidateCache();

        return result;

    }

    private void InvalidateCache()
    {
        _memoryCache.Remove(AllCategoriesCacheKey);
    }
}