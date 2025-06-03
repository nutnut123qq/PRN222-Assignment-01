using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess.Repositories
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<IEnumerable<Tag>> SearchTagsAsync(string searchTerm);
        Task<Tag?> GetByNameAsync(string tagName);
        Task<IEnumerable<Tag>> GetTagsByNewsArticleAsync(string newsArticleId);
    }
}
