using Microsoft.EntityFrameworkCore;
using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(FUNewsManagementDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Tag>> SearchTagsAsync(string searchTerm)
        {
            return await _dbSet
                .Where(t => t.TagName.Contains(searchTerm) || 
                           (t.Note != null && t.Note.Contains(searchTerm)))
                .ToListAsync();
        }

        public async Task<Tag?> GetByNameAsync(string tagName)
        {
            return await _dbSet.FirstOrDefaultAsync(t => t.TagName == tagName);
        }

        public async Task<IEnumerable<Tag>> GetTagsByNewsArticleAsync(string newsArticleId)
        {
            return await _context.NewsTags
                .Where(nt => nt.NewsArticleId == newsArticleId)
                .Select(nt => nt.Tag)
                .ToListAsync();
        }
    }
}
