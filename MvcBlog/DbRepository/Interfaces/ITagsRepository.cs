using MvcBlog.Models;

namespace MvcBlog.DbRepository.Interfaces
{
    public interface ITagsRepository
    {
        public Task<IEnumerable<Tag>> GetAllAsync();
        public Task<Tag> GetByIdAsync(int id);
    }
}
