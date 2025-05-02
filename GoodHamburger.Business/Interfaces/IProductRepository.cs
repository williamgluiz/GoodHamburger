using GoodHamburger.Domain.Models;

namespace GoodHamburger.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetByTypeAsync(ProductType type);
    }
}
