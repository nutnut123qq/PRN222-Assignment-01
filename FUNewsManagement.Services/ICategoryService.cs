using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(short id);
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(short id);
        Task<IEnumerable<Category>> SearchCategoriesAsync(string searchTerm);
        Task<bool> CanDeleteCategoryAsync(short categoryId);
        Task<IEnumerable<Category>> GetCategoriesWithSubCategoriesAsync();
    }
}
