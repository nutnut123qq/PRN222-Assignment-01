using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.DataAccess.Repositories
{
    public interface INewsTagRepository : IRepository<NewsTag>
    {
        Task AddNewsTagsAsync(string newsArticleId, IEnumerable<int> tagIds);
        Task RemoveNewsTagsAsync(string newsArticleId);
        Task UpdateNewsTagsAsync(string newsArticleId, IEnumerable<int> tagIds);
    }
}
