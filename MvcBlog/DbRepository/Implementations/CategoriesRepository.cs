﻿using Microsoft.EntityFrameworkCore;
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

            if (сategories is not null)
            {
                return сategories;
            }

            return new List<Category>(); 
        }

        public async Task<Category> GetById(int id)
        {
            Category сategory = null;

            using (var context = RepositoryContextFactory.CreateDbContext(ConnectionString))
            {
                if (context.Сategories.Any())
                {
                    сategory = await context.Сategories.FirstOrDefaultAsync(c => c.Id == id);
                }
            }

            if (сategory is not null)
            {
                return сategory;
            }

            throw new NullReferenceException("Cannot find category");
        }
    }
}
