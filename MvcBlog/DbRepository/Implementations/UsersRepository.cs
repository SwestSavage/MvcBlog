using Microsoft.EntityFrameworkCore;
using MvcBlog.DbRepository.Interfaces;
using MvcBlog.Models;

namespace MvcBlog.DbRepository.Implementations
{
    public class UsersRepository : BaseRepository, IUsersRepository
    {
        public UsersRepository(string connectionString, IRepositoryContextFactory repositoryContextFactory) : base(connectionString, repositoryContextFactory)
        {
        }

        public async Task AddAsync(User user)
        {
            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                await context.Users.AddAsync(user);

                await context.SaveChangesAsync();
            }
        }

        public async Task<User> GetByIdAsync(int id)
        {
            User? user = null;

            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                if (context.Users.Any())
                {
                    user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
                }
            }

            if (user is null)
            {
                throw new NullReferenceException("Cannot find user by id");
            }

            return user;
        }

        public async Task<User> GetByLoginAsync(string login)
        {
            User? user = null;

            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                if (context.Users.Any())
                {
                    user = await context.Users.FirstOrDefaultAsync(u => u.Login == login);
                }
            }

            if (user is null)
            {
                throw new NullReferenceException("Cannot find user by login");
            }

            return user;
        }
    }
}
