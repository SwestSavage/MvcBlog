using MvcBlog.Models;

namespace MvcBlog.DbRepository.Interfaces
{
    public interface IPostsRepository
    {
        public Task<IEnumerable<Post>> GetAllAsync();
        public Task<Post> GetByIdAsync(int id);
        public Task<IEnumerable<Post>> GetAllByCategoryAsync(Сategory сategory);
        public Task<IEnumerable<Post>> GetAllByTagAsync(Tag tag);

        public Task AddAsync(Post post);
        public Task UpdateAsync(Post post);
        public Task DeleteByIdAsync(int id);

        public Task AddTagAsync(Post post, Tag tag);
        public Task RemoveTagAsync(Post post, Tag tag);
    }
}
