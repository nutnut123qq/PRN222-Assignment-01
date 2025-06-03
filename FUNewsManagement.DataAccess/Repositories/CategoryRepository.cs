using Microsoft.EntityFrameworkCore;
using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(FUNewsManagementDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _dbSet.Where(c => c.IsActive).ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithSubCategoriesAsync()
        {
            return await _dbSet
                .Include(c => c.SubCategories)
                .Include(c => c.ParentCategory)
                .ToListAsync();
        }

        public async Task<bool> HasNewsArticlesAsync(short categoryId)
        {
            return await _context.NewsArticles.AnyAsync(n => n.CategoryId == categoryId);
        }

        public async Task<bool> HasSubCategoriesAsync(short categoryId)
        {
            return await _dbSet.AnyAsync(c => c.ParentCategoryId == categoryId);
        }

        public async Task<IEnumerable<Category>> SearchCategoriesAsync(string searchTerm)
        {
            return await _dbSet
                .Where(c => c.CategoryName.Contains(searchTerm) || 
                           (c.CategoryDescription != null && c.CategoryDescription.Contains(searchTerm)))
                .ToListAsync();
        }
    }
}
