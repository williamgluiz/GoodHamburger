using AutoMapper;
using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Models;

namespace GoodHamburger.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the product service.
    /// </summary>
    /// <param name="productRepository">The repository responsible for accessing product data.</param>
    /// <param name="mapper">The mapper used to map objects between layers.</param>
    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all products asynchronously, ordered by product type.
    /// </summary>
    /// <returns>A collection of <see cref="ProductDTO"/> objects.</returns>
    /// <exception cref="Exception">Thrown when an error occurs while retrieving the products.</exception>
    public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProductDTO>>(products.OrderBy(item => item.Type));
    }

    /// <summary>
    /// Retrieves all products of type <see cref="ProductType.Extra"/> asynchronously.
    /// </summary>
    /// <returns>A collection of <see cref="ProductDTO"/> objects representing extra items.</returns>
    /// <exception cref="Exception">Thrown when an error occurs while retrieving the extras.</exception>

    public async Task<IEnumerable<ProductDTO>> GetExtrasAsync()
    {
        var sandwiches = await _productRepository.GetByTypeAsync(ProductType.Extra);
        return _mapper.Map<IEnumerable<ProductDTO>>(sandwiches);
    }

    /// <summary>
    /// Retrieves a product by its unique identifier asynchronously.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A <see cref="ProductDTO"/> object if found; otherwise, <c>null</c>.</returns>
    /// <exception cref="Exception">Thrown when an error occurs while retrieving the product.</exception>
    public async Task<ProductDTO?> GetProductByIdAsync(Guid productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        return _mapper.Map<ProductDTO>(product);
    }

    /// <summary>
    /// Retrieves all products of type <see cref="ProductType.Sandwich"/> asynchronously.
    /// </summary>
    /// <returns>A collection of <see cref="ProductDTO"/> objects representing sandwiches.</returns>
    /// <exception cref="Exception">Thrown when an error occurs while retrieving the sandwiches.</exception>
    public async Task<IEnumerable<ProductDTO>> GetSandwichesAsync()
    {
        var sandwiches = await _productRepository.GetByTypeAsync(ProductType.Sandwich);
        return _mapper.Map<IEnumerable<ProductDTO>>(sandwiches);
    }
}
