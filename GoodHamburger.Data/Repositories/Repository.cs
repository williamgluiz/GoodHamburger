using GoodHamburger.Data.Context;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Data.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly AppDbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        protected Repository(AppDbContext db)
        {
            Db = db;
            DbSet = Db.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
            => await DbSet.ToListAsync();

        public virtual async Task<TEntity?> GetByIdAsync(Guid id)
            => await DbSet.FindAsync(id);

        public virtual async Task AddAsync(TEntity entity)
        {
            DbSet.Add(entity);
            await SaveChanges();
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChanges();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await DbSet.FindAsync(id);

            if (entity == null)
                throw new InvalidOperationException("Entity not found");

            DbSet.Remove(entity);
            await SaveChanges();
        }

        public virtual async Task<int> SaveChanges()
            => await Db.SaveChangesAsync();

        public void Dispose()
            => Db?.Dispose();
    }
}
