using Microsoft.EntityFrameworkCore;
using MvcBlog.DbRepository.Interfaces;
using MvcBlog.Models;

namespace MvcBlog.DbRepository.Implementations
{
    public class TagsRepository : BaseRepository, ITagsRepository
    {
        public TagsRepository(string connectionString, IRepositoryContextFactory repositoryContextFactory) : base(connectionString, repositoryContextFactory)
        {
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            List<Tag> tags = null;

            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                if (context.Tags.Any())
                {
                    tags = await context.Tags.Include(t => t.Posts).ToListAsync();
                }
            }

            if (tags is null)
            {
                throw new NullReferenceException("No available tags found.");
            }

            return tags;
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            Tag? tag = null;

            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                if (context.Tags.Any())
                {
                    tag = await context.Tags.Include(t => t.Posts).FirstOrDefaultAsync(t => t.Id == id);
                }
            }

            if (tag is null)
            {
                throw new NullReferenceException("Cannot find tag");               
            }

            return tag;
        }
    }
}
