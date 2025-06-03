using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.Services
{
    public interface INewsArticleService
    {
        Task<IEnumerable<NewsArticle>> GetAllNewsAsync();
        Task<IEnumerable<NewsArticle>> GetActiveNewsAsync();
        Task<NewsArticle?> GetNewsByIdAsync(string id);
        Task<NewsArticle?> GetNewsWithDetailsAsync(string id);
        Task<NewsArticle> CreateNewsAsync(NewsArticle news, IEnumerable<int> tagIds);
        Task<NewsArticle> UpdateNewsAsync(NewsArticle news, IEnumerable<int> tagIds);
        Task DeleteNewsAsync(string id);
        Task<IEnumerable<NewsArticle>> GetNewsByAuthorAsync(short authorId);
        Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(short categoryId);
        Task<IEnumerable<NewsArticle>> SearchNewsAsync(string searchTerm);
        Task<IEnumerable<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Dictionary<DateTime, int>> GetNewsStatisticsByDateRangeAsync(DateTime startDate, DateTime endDate);
        string GenerateNewsId();
    }
}
