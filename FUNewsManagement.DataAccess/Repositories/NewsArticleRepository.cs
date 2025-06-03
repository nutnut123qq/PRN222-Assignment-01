using Microsoft.EntityFrameworkCore;
using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess.Repositories
{
    public class NewsArticleRepository : Repository<NewsArticle>, INewsArticleRepository
    {
        public NewsArticleRepository(FUNewsManagementDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<NewsArticle>> GetActiveNewsAsync()
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .Where(n => n.NewsStatus)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByAuthorAsync(short authorId)
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .Where(n => n.CreatedById == authorId)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(short categoryId)
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .Where(n => n.CategoryId == categoryId && n.NewsStatus)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsWithDetailsAsync()
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.UpdatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<NewsArticle?> GetNewsWithDetailsAsync(string newsId)
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.UpdatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .FirstOrDefaultAsync(n => n.NewsArticleId == newsId);
        }

        public async Task<IEnumerable<NewsArticle>> SearchNewsAsync(string searchTerm)
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .Where(n => n.NewsTitle.Contains(searchTerm) || 
                           n.Headline.Contains(searchTerm) ||
                           n.NewsContent.Contains(searchTerm))
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync();
        }

        public async Task<Dictionary<DateTime, int>> GetNewsStatisticsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var newsData = await _dbSet
                .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                .GroupBy(n => n.CreatedDate.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Date)
                .ToListAsync();

            return newsData.ToDictionary(x => x.Date, x => x.Count);
        }
    }
}
