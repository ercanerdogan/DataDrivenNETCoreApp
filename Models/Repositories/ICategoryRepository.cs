namespace BethanysPieShopAdmin.Models.Repositories;

public interface ICategoryRepository
{
    IEnumerable<Category> GetAllCategories();
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category> GetCategoryByIdAsync(int categoryId);
    Task<int> AddCategoryAsync(Category category);
    Task<int> UpdateCategoryAsync(Category category);
    Task<int> DeleteCategoryAsync(int id);
}