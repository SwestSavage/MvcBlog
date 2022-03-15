using MvcBlog.Models;

namespace MvcBlog.DbRepository.Interfaces
{
    public interface ICategoriesRepository
    {
        public Task<IEnumerable<Category>> GetAllAsync();
        public Task<Category> GetById(int id);
    }
}
