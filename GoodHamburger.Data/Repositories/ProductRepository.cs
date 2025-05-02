using GoodHamburger.Data.Context;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext db) : base(db)
        {
        }

        public async Task<IEnumerable<Product>> GetByTypeAsync(ProductType type)
        {
            var productsFiltered = await Db.Products
                .Where(p => p.Type == type)
                .ToListAsync();

            return productsFiltered;
        }
    }
}
