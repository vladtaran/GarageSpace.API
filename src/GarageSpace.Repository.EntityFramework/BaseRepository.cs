using Microsoft.EntityFrameworkCore;
using GarageSpace.Models.Repository.EF;

namespace GarageSpace.Repository.EntityFramework
{
    public abstract class BaseRepository<TEntity> where TEntity : BaseEntity
    {
        public DbSet<TEntity>? dbSet;
        public readonly DbContext dbContext;
        public DbSet<TEntity> DbSet => this.dbSet ??= this.dbContext.Set<TEntity>();

        protected BaseRepository(
            DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected virtual IQueryable<TEntity> InitQuery() => this.DbSet;

        protected virtual async IAsyncEnumerable<TEntity> GetAll()
        {
            var entities = await this.DbSet.ToListAsync();
            foreach (var entity in entities)
            {
                yield return entity;
            }
        }
    }
}
