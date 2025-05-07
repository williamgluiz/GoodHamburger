using AutoMapper;
using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Models;
using Microsoft.Extensions.Logging;

namespace GoodHamburger.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;

    /// <summary>
    /// Initializes a new instance of the product service.
    /// </summary>
    /// <param name="productRepository">The repository responsible for accessing product data.</param>
    /// <param name="mapper">The mapper used to map objects between layers.</param>
    /// <param name="logger">The logger for recording information and errors related to the product service.</param>
    public ProductService(IProductRepository productRepository, IMapper mapper, ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all products asynchronously, ordered by product type.
    /// </summary>
    /// <returns>A collection of <see cref="ProductDTO"/> objects.</returns>
    /// <exception cref="Exception">Thrown when an error occurs while retrieving the products.</exception>
    public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
    {
        _logger.LogInformation($"GetAllProductsAsync called at: {DateTime.UtcNow}.");

        try
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(products.OrderBy(item => item.Type));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting all products at: {DateTime.UtcNow}.");
            throw;
        }
    }

    /// <summary>
    /// Retrieves all products of type <see cref="ProductType.Extra"/> asynchronously.
    /// </summary>
    /// <returns>A collection of <see cref="ProductDTO"/> objects representing extra items.</returns>
    /// <exception cref="Exception">Thrown when an error occurs while retrieving the extras.</exception>

    public async Task<IEnumerable<ProductDTO>> GetExtrasAsync()
    {
        _logger.LogInformation($"GetExtrasAsync called at: {DateTime.UtcNow}.");

        try
        {
            var sandwiches = await _productRepository.GetByTypeAsync(ProductType.Extra);
            return _mapper.Map<IEnumerable<ProductDTO>>(sandwiches);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting all extras at: {DateTime.UtcNow}.");
            throw;
        }
    }

    /// <summary>
    /// Retrieves a product by its unique identifier asynchronously.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <returns>A <see cref="ProductDTO"/> object if found; otherwise, <c>null</c>.</returns>
    /// <exception cref="Exception">Thrown when an error occurs while retrieving the product.</exception>
    public async Task<ProductDTO?> GetProductByIdAsync(Guid productId)
    {
        _logger.LogInformation($"GetProductByIdAsync called at: {DateTime.UtcNow}.");

        try
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return null;
            return _mapper.Map<ProductDTO>(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting products by Id at: {DateTime.UtcNow}.");
            throw;
        }
    }

    /// <summary>
    /// Retrieves all products of type <see cref="ProductType.Sandwich"/> asynchronously.
    /// </summary>
    /// <returns>A collection of <see cref="ProductDTO"/> objects representing sandwiches.</returns>
    /// <exception cref="Exception">Thrown when an error occurs while retrieving the sandwiches.</exception>
    public async Task<IEnumerable<ProductDTO>> GetSandwichesAsync()
    {
        _logger.LogInformation($"GetSandwichesAsync called at: {DateTime.UtcNow}.");

        try
        {
            var sandwiches = await _productRepository.GetByTypeAsync(ProductType.Sandwich);
            return _mapper.Map<IEnumerable<ProductDTO>>(sandwiches);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while getting sandwiches at: {DateTime.UtcNow}.");
            throw;
        }
    }
}
