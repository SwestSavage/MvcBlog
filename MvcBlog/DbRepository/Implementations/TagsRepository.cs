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

            if (tags is not null)
            {
                return tags;
            }

            return new List<Tag>();
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            Tag tag = null;

            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                if (context.Tags.Any())
                {
                    tag = await context.Tags.Include(t => t.Posts).FirstOrDefaultAsync(t => t.Id == id);
                }
            }

            if (tag is not null)
            {
                return tag;
            }

            throw new NullReferenceException("Cannot find tag");
        }
    }
}
