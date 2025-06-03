using FUNewsManagement.DataAccess.Models;
using FUNewsManagement.DataAccess.Repositories;

namespace FUNewsManagement.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _categoryRepository.GetActiveCategoriesAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(short id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            await _categoryRepository.UpdateAsync(category);
            return category;
        }

        public async Task DeleteCategoryAsync(short id)
        {
            if (!await CanDeleteCategoryAsync(id))
            {
                throw new InvalidOperationException("Cannot delete category that has news articles or subcategories.");
            }

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                await _categoryRepository.DeleteAsync(category);
            }
        }

        public async Task<IEnumerable<Category>> SearchCategoriesAsync(string searchTerm)
        {
            return await _categoryRepository.SearchCategoriesAsync(searchTerm);
        }

        public async Task<bool> CanDeleteCategoryAsync(short categoryId)
        {
            var hasNewsArticles = await _categoryRepository.HasNewsArticlesAsync(categoryId);
            var hasSubCategories = await _categoryRepository.HasSubCategoriesAsync(categoryId);
            
            return !hasNewsArticles && !hasSubCategories;
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithSubCategoriesAsync()
        {
            return await _categoryRepository.GetCategoriesWithSubCategoriesAsync();
        }
    }
}
