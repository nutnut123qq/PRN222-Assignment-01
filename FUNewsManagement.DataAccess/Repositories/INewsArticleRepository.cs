using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess.Repositories
{
    public interface INewsArticleRepository : IRepository<NewsArticle>
    {
        Task<IEnumerable<NewsArticle>> GetActiveNewsAsync();
        Task<IEnumerable<NewsArticle>> GetNewsByAuthorAsync(short authorId);
        Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(short categoryId);
        Task<IEnumerable<NewsArticle>> GetNewsWithDetailsAsync();
        Task<NewsArticle?> GetNewsWithDetailsAsync(string newsId);
        Task<IEnumerable<NewsArticle>> SearchNewsAsync(string searchTerm);
        Task<IEnumerable<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Dictionary<DateTime, int>> GetNewsStatisticsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
