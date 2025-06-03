using FUNewsManagement.DataAccess.Models;
using FUNewsManagement.DataAccess.Repositories;

namespace FUNewsManagement.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _tagRepository.GetAllAsync();
        }

        public async Task<Tag?> GetTagByIdAsync(int id)
        {
            return await _tagRepository.GetByIdAsync(id);
        }

        public async Task<Tag?> GetTagByNameAsync(string name)
        {
            return await _tagRepository.GetByNameAsync(name);
        }

        public async Task<Tag> CreateTagAsync(Tag tag)
        {
            await _tagRepository.AddAsync(tag);
            return tag;
        }

        public async Task<Tag> UpdateTagAsync(Tag tag)
        {
            await _tagRepository.UpdateAsync(tag);
            return tag;
        }

        public async Task DeleteTagAsync(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            if (tag != null)
            {
                await _tagRepository.DeleteAsync(tag);
            }
        }

        public async Task<IEnumerable<Tag>> SearchTagsAsync(string searchTerm)
        {
            return await _tagRepository.SearchTagsAsync(searchTerm);
        }

        public async Task<IEnumerable<Tag>> GetTagsByNewsArticleAsync(string newsArticleId)
        {
            return await _tagRepository.GetTagsByNewsArticleAsync(newsArticleId);
        }

        public async Task<Tag> GetOrCreateTagAsync(string tagName)
        {
            var existingTag = await _tagRepository.GetByNameAsync(tagName);
            if (existingTag != null)
            {
                return existingTag;
            }

            var newTag = new Tag { TagName = tagName };
            await _tagRepository.AddAsync(newTag);
            return newTag;
        }
    }
}
