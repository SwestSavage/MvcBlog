using Microsoft.EntityFrameworkCore;
using MvcBlog.DbRepository.Interfaces;
using MvcBlog.Models;

namespace MvcBlog.DbRepository.Implementations
{
    public class PostsRepository : BaseRepository, IPostsRepository
    {
        public PostsRepository(string connectionString, IRepositoryContextFactory repositoryContextFactory) : base(connectionString, repositoryContextFactory)
        {
        }

        public async Task AddAsync(Post post)
        {
            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                await context.Posts.AddAsync(post);

                await context.SaveChangesAsync();
            }
        }

        public async Task AddTagAsync(Post post, Tag tag)
        {
            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                var p = await context.Posts
                    .Include(p => p.Tags)
                    .FirstOrDefaultAsync(p => p.Id == post.Id);

                var t = await context.Tags
                    .Include(t => t.Posts)
                    .FirstOrDefaultAsync(t => t.Name == tag.Name);

                if (p is not null && t is not null)
                {
                    p.Tags.Add(t);
                    t.Posts.Add(p);
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteByIdAsync(int id)
        {
            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                var p = await context.Posts
                    .Include(p => p.Tags)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (p is not null)
                {
                    context.Posts.Remove(p);
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            List<Post> posts = null;

            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                if (context.Posts.Any())
                {
                    posts = await context.Posts
                        .Include(p => p.Author)
                        .Include(p => p.Tags)
                        .Include(p => p.Сategory)
                        .ToListAsync();
                }
            }

            if (posts is not null)
            {
                return posts;
            }

            return new List<Post>();
        }

        public async Task<IEnumerable<Post>> GetAllByCategoryAsync(Category сategory)
        {
            List<Post> posts = null;

            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                if (context.Posts.Any())
                {
                    var c = await context.Сategories
                        .FirstOrDefaultAsync(c => c.Name == сategory.Name);

                    if (c is not null)
                    {
                        posts = await context.Posts
                            .Include(p => p.Author)
                            .Include(p => p.Tags)
                            .Include(p => p.Сategory)
                            .Where(p => p.Сategory == c)
                            .ToListAsync();
                    }
                }
            }

            if (posts is not null)
            {
                return posts;
            }

            return new List<Post>();
        }

        public async Task<IEnumerable<Post>> GetAllByTagAsync(Tag tag)
        {
            List<Post> posts = null;

            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                if (context.Posts.Any())
                {
                    var t = await context.Tags
                        .Include(t => t.Posts)
                        .FirstOrDefaultAsync(t => t.Name == tag.Name);

                    if (t is not null)
                    {
                        posts = await context.Posts
                            .Include(p => p.Author)
                            .Include(p => p.Tags)
                            .Include(p => p.Сategory)
                            .Where(p => p.Tags.Contains(t))
                            .ToListAsync();
                    }
                }
            }

            if (posts is not null)
            {
                return posts;
            }

            return new List<Post>();
        }

        public async Task<Post> GetByIdAsync(int id)
        {
            Post post = null;

            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                if (context.Posts.Any())
                {
                    post = await context.Posts
                        .Include(p => p.Author)
                        .Include(p => p.Tags)
                        .Include(p => p.Сategory)
                        .FirstOrDefaultAsync(p => p.Id == id);
                }
            }

            if (post is not null)
            {
                return post;
            }

            throw new NullReferenceException("Cannot find post by id");
        }

        public async Task RemoveTagAsync(Post post, Tag tag)
        {
            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                var p = await context.Posts
                    .Include(p => p.Tags)
                    .FirstOrDefaultAsync(p => p.Id == post.Id);

                if (p is not null)
                {
                    p.Tags.Remove(tag);
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Post post)
        {
            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                var p = await context.Posts
                        .Include(p => p.Author)
                        .Include(p => p.Tags)
                        .Include(p => p.Сategory)
                        .FirstOrDefaultAsync(p => p.Id == post.Id);

                if (p is not null)
                {
                    if (!string.IsNullOrEmpty(post.Name)) p.Name = post.Name;

                    if (!string.IsNullOrEmpty(post.ShortDescription)) p.ShortDescription = post.ShortDescription;

                    if (!string.IsNullOrEmpty(post.Description)) p.Description = post.Description;

                    if (!string.IsNullOrEmpty(post.ImagePath)) p.ImagePath = post.ImagePath;

                    if (post.Сategory is not null) p.Сategory = post.Сategory;
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
