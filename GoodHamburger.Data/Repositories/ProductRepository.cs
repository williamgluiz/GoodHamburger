using GoodHamburger.Data.Context;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Data.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductRepository"/> class with the specified database context.
    /// </summary>
    /// <param name="db">The application's database context.</param>
    public ProductRepository(AppDbContext db) : base(db) { }

    /// <summary>
    /// Retrieves all products that match the specified <see cref="ProductType"/> asynchronously.
    /// </summary>
    /// <param name="type">The type of product to filter by.</param>
    /// <returns>A collection of <see cref="Product"/> objects that match the specified type.</returns>
    public async Task<IEnumerable<Product>> GetByTypeAsync(ProductType type)
    {
        var productsFiltered = await Db.Products
            .Where(p => p.Type == type)
            .ToListAsync();

        return productsFiltered;
    }
}
