namespace MvcBlog.DbRepository.Interfaces
{
    public interface IRepositoryContextFactory
    {
        public RepositoryContext CreateDbContext(string connectionString);
    }
}
