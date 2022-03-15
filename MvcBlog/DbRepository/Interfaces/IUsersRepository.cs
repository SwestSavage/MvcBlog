using MvcBlog.Models;

namespace MvcBlog.DbRepository.Interfaces
{
    public interface IUsersRepository
    {
        public Task AddAsync(User user);
        public Task<User> GetByIdAsync(int id);
        public Task<User> GetByLoginAsync(string login);
    }
}
