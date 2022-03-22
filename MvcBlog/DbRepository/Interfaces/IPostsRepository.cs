using MvcBlog.Models;

namespace MvcBlog.DbRepository.Interfaces
{
    public interface IPostsRepository
    {
        public Task<IEnumerable<Post>> GetAllAsync();
        public Task<Post> GetByIdAsync(int id);
        public Task<IEnumerable<Post>> GetAllByCategoryIdAsync(int сategoryId);
        public Task<IEnumerable<Post>> GetAllByTagIdAsync(int tagId);
        public Task<IEnumerable<Post>> GetAllByCategoryAndTagIds(int categoryId, int tagId);
        public Task<IEnumerable<Post>> GetAllByDateAsync(DateTime date);
        public Task<IEnumerable<Post>> GetAllByAuthorIdAsync(int authorId);

        public Task AddAsync(Post post);
        public Task UpdateAsync(Post post);
        public Task DeleteByIdAsync(int id);

        public Task AddTagAsync(Post post, Tag tag);
        public Task RemoveTagAsync(Post post, Tag tag);
    }
}
