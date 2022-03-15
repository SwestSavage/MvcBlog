using Microsoft.EntityFrameworkCore;
using MvcBlog.Models;

namespace MvcBlog.DbRepository
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Сategories { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
