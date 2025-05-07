using GoodHamburger.Domain.Models;

namespace GoodHamburger.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    /// <summary>
    /// Retrieves all products of a specific <see cref="ProductType"/> asynchronously.
    /// </summary>
    /// <param name="type">The type of product to retrieve.</param>
    /// <returns>A collection of <see cref="Product"/> objects matching the specified type.</returns>
    Task<IEnumerable<Product>> GetByTypeAsync(ProductType type);
}
