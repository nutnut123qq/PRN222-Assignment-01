using Microsoft.EntityFrameworkCore;
using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess.Repositories
{
    public class NewsTagRepository : Repository<NewsTag>, INewsTagRepository
    {
        public NewsTagRepository(FUNewsManagementDbContext context) : base(context)
        {
        }

        public async Task AddNewsTagsAsync(string newsArticleId, IEnumerable<int> tagIds)
        {
            var newsTags = tagIds.Select(tagId => new NewsTag
            {
                NewsArticleId = newsArticleId,
                TagId = tagId
            });

            await _dbSet.AddRangeAsync(newsTags);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveNewsTagsAsync(string newsArticleId)
        {
            var existingTags = await _dbSet
                .Where(nt => nt.NewsArticleId == newsArticleId)
                .ToListAsync();

            _dbSet.RemoveRange(existingTags);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateNewsTagsAsync(string newsArticleId, IEnumerable<int> tagIds)
        {
            // Remove existing tags
            await RemoveNewsTagsAsync(newsArticleId);
            
            // Add new tags
            if (tagIds.Any())
            {
                await AddNewsTagsAsync(newsArticleId, tagIds);
            }
        }
    }
}
