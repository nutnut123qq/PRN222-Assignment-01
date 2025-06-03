using FUNewsManagement.DataAccess.Models;

namespace FUNewsManagement.Services
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag?> GetTagByIdAsync(int id);
        Task<Tag?> GetTagByNameAsync(string name);
        Task<Tag> CreateTagAsync(Tag tag);
        Task<Tag> UpdateTagAsync(Tag tag);
        Task DeleteTagAsync(int id);
        Task<IEnumerable<Tag>> SearchTagsAsync(string searchTerm);
        Task<IEnumerable<Tag>> GetTagsByNewsArticleAsync(string newsArticleId);
        Task<Tag> GetOrCreateTagAsync(string tagName);
    }
}
