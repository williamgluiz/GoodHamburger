using GoodHamburger.Data.Context;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Data.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    protected readonly AppDbContext Db;
    protected readonly DbSet<TEntity> DbSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="Repository{TEntity}"/> class with the specified database context.
    /// </summary>
    /// <param name="db">The application's database context.</param>
    protected Repository(AppDbContext db)
    {
        Db = db;
        DbSet = Db.Set<TEntity>();
    }

    /// <summary>
    /// Retrieves all entities of type <typeparamref name="TEntity"/> asynchronously.
    /// </summary>
    /// <returns>A collection of all <typeparamref name="TEntity"/> objects.</returns>
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        => await DbSet.ToListAsync();

    /// <summary>
    /// Retrieves an entity of type <typeparamref name="TEntity"/> by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>A <typeparamref name="TEntity"/> object if found; otherwise, <c>null</c>.</returns>
    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
        => await DbSet.FindAsync(id);

    /// <summary>
    /// Adds a new entity of type <typeparamref name="TEntity"/> to the database asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    /// <exception cref="Exception">Thrown when an error occurs while saving changes to the database.</exception>
    public virtual async Task AddAsync(TEntity entity)
    {
        DbSet.Add(entity);
        await SaveChanges();
    }

    /// <summary>
    /// Updates an existing entity of type <typeparamref name="TEntity"/> in the database asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    /// <exception cref="Exception">Thrown when an error occurs while saving changes to the database.</exception>
    public virtual async Task UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
        await SaveChanges();
    }

    /// <summary>
    /// Deletes an entity of type <typeparamref name="TEntity"/> by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to be deleted.</param>
    /// <exception cref="InvalidOperationException">Thrown if the entity with the specified identifier is not found.</exception>
    /// <exception cref="Exception">Thrown when an error occurs while saving changes to the database.</exception>
    public virtual async Task DeleteAsync(Guid id)
    {
        var entity = await DbSet.FindAsync(id);

        if (entity == null)
            throw new InvalidOperationException("Entity not found");

        DbSet.Remove(entity);
        await SaveChanges();
    }

    /// <summary>
    /// Saves all changes made to the database asynchronously.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    /// <exception cref="Exception">Thrown when an error occurs while saving changes to the database.</exception>
    public virtual async Task<int> SaveChanges()
        => await Db.SaveChangesAsync();

    /// <summary>
    /// Disposes of the resources used by the repository, including the database context.
    /// </summary>
    /// <remarks>
    /// This method is called to release unmanaged resources and free up memory when the repository is no longer needed.
    /// </remarks>
    public void Dispose()
        => Db?.Dispose();
}
