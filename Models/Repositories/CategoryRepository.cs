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
        return _context.Categories.OrderBy(c => c.CategoryId);
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.OrderBy(c => c.CategoryId).ToListAsync();
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
}