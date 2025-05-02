using GoodHamburger.Application.DTOs.Product;

namespace GoodHamburger.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<IEnumerable<ProductDTO>> GetSandwichesAsync();
        Task<IEnumerable<ProductDTO>> GetExtrasAsync();
        Task<ProductDTO?> GetProductByIdAsync(Guid productId);
    }
}
