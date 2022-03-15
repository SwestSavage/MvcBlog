using MvcBlog.DbRepository.Interfaces;

namespace MvcBlog.DbRepository
{
    public class BaseRepository
    {
        public string ConnectionString { get; }
        public IRepositoryContextFactory RepositoryContextFactory { get; }

        public BaseRepository(string connectionString, IRepositoryContextFactory repositoryContextFactory)
        {
            ConnectionString = connectionString;
            RepositoryContextFactory = repositoryContextFactory;
        }
    }
}
