using Domain.Contracts.DataAccess;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class, BaseEntity
    {
        private readonly SEBTaskDbContext dbContext;

        public Repository(SEBTaskDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<T> GetAsync(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null
        )
        {
            IQueryable<T> query = this.dbContext.Set<T>();

            if (include != null) query = include(query);
            if (predicate != null) query = query.Where(predicate);

            return await query.FirstOrDefaultAsync();
        }
    }
}
