using Microsoft.EntityFrameworkCore;
using MvcBlog.DbRepository.Interfaces;
using MvcBlog.Models;

namespace MvcBlog.DbRepository.Implementations
{
    public class CategoriesRepository : BaseRepository, ICategoriesRepository
    {
        public CategoriesRepository(string connectionString, IRepositoryContextFactory repositoryContextFactory) : base(connectionString, repositoryContextFactory)
        {
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            List<Category> сategories = null;

            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                if (context.Сategories.Any())
                {
                    сategories = await context.Сategories.ToListAsync();
                }
            }

            if (сategories is null)
            {
                throw new NullReferenceException("No available categories found.");
            }

            return сategories;
        }

        public async Task<Category> GetById(int id)
        {
            Category? сategory = null;

            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                if (context.Сategories.Any())
                {
                    сategory = await context.Сategories.FirstOrDefaultAsync(c => c.Id == id);
                }
            }

            if (сategory is null)
            {
                throw new NullReferenceException("Cannot find category");
            }

            return сategory;
        }
    }
}
