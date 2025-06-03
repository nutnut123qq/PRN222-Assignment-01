using FUNewsManagement.DataAccess.Models;
using FUNewsManagement.DataAccess.Repositories;

namespace FUNewsManagement.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _newsRepository;
        private readonly INewsTagRepository _newsTagRepository;

        public NewsArticleService(INewsArticleRepository newsRepository, INewsTagRepository newsTagRepository)
        {
            _newsRepository = newsRepository;
            _newsTagRepository = newsTagRepository;
        }

        public async Task<IEnumerable<NewsArticle>> GetAllNewsAsync()
        {
            return await _newsRepository.GetNewsWithDetailsAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetActiveNewsAsync()
        {
            return await _newsRepository.GetActiveNewsAsync();
        }

        public async Task<NewsArticle?> GetNewsByIdAsync(string id)
        {
            return await _newsRepository.GetByIdAsync(id);
        }

        public async Task<NewsArticle?> GetNewsWithDetailsAsync(string id)
        {
            return await _newsRepository.GetNewsWithDetailsAsync(id);
        }

        public async Task<NewsArticle> CreateNewsAsync(NewsArticle news, IEnumerable<int> tagIds)
        {
            // Generate unique ID if not provided
            if (string.IsNullOrEmpty(news.NewsArticleId))
            {
                news.NewsArticleId = GenerateNewsId();
            }

            news.CreatedDate = DateTime.Now;
            await _newsRepository.AddAsync(news);

            // Add tags if provided
            if (tagIds.Any())
            {
                await _newsTagRepository.AddNewsTagsAsync(news.NewsArticleId, tagIds);
            }

            return news;
        }

        public async Task<NewsArticle> UpdateNewsAsync(NewsArticle news, IEnumerable<int> tagIds)
        {
            news.ModifiedDate = DateTime.Now;
            await _newsRepository.UpdateAsync(news);

            // Update tags
            await _newsTagRepository.UpdateNewsTagsAsync(news.NewsArticleId, tagIds);

            return news;
        }

        public async Task DeleteNewsAsync(string id)
        {
            var news = await _newsRepository.GetByIdAsync(id);
            if (news != null)
            {
                // Remove associated tags first
                await _newsTagRepository.RemoveNewsTagsAsync(id);
                await _newsRepository.DeleteAsync(news);
            }
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByAuthorAsync(short authorId)
        {
            return await _newsRepository.GetNewsByAuthorAsync(authorId);
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(short categoryId)
        {
            return await _newsRepository.GetNewsByCategoryAsync(categoryId);
        }

        public async Task<IEnumerable<NewsArticle>> SearchNewsAsync(string searchTerm)
        {
            return await _newsRepository.SearchNewsAsync(searchTerm);
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _newsRepository.GetNewsByDateRangeAsync(startDate, endDate);
        }

        public async Task<Dictionary<DateTime, int>> GetNewsStatisticsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _newsRepository.GetNewsStatisticsByDateRangeAsync(startDate, endDate);
        }

        public string GenerateNewsId()
        {
            return $"NEWS{DateTime.Now:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";
        }
    }
}
