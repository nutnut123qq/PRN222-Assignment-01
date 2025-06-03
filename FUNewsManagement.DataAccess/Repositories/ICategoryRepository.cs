using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<IEnumerable<Category>> GetCategoriesWithSubCategoriesAsync();
        Task<bool> HasNewsArticlesAsync(short categoryId);
        Task<bool> HasSubCategoriesAsync(short categoryId);
        Task<IEnumerable<Category>> SearchCategoriesAsync(string searchTerm);
    }
}
