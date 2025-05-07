using GoodHamburger.Domain.Models;

namespace GoodHamburger.Domain.Interfaces;

public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
    /// <summary>
    /// Retrieves all entities of type <typeparamref name="TEntity"/> asynchronously.
    /// </summary>
    /// <returns>A collection of all <typeparamref name="TEntity"/> objects.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Retrieves an entity of type <typeparamref name="TEntity"/> by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>A <typeparamref name="TEntity"/> object if found; otherwise, <c>null</c>.</returns>
    Task<TEntity?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a new entity of type <typeparamref name="TEntity"/> to the database asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be added.</param>
    Task AddAsync(TEntity entity);

    /// <summary>
    /// Updates an existing entity of type <typeparamref name="TEntity"/> in the database asynchronously.
    /// </summary>
    /// <param name="entity">The entity to be updated.</param>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Deletes an entity of type <typeparamref name="TEntity"/> by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to be deleted.</param>
    Task DeleteAsync(Guid id);
}
