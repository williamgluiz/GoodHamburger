using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    /// <summary>
    /// Initializes a new instance of the ProductsController.
    /// </summary>
    /// <param name="productService">The service responsible for handling product-related operations.</param>
    /// <param name="logger">The logger for recording information and errors related to the ProductsController.</param>
    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all products from the system (Sandwiches and Extras).
    /// </summary>
    /// <returns>A list of products. If no products are found, returns a 204 No Content status.</returns>
    /// <response code="200">Returns a list of products.</response>
    /// <response code="204">No products found.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAllProducts()
    {
        _logger.LogInformation($"GET /products called at: {DateTime.UtcNow}.");

        var products = await _productService.GetAllProductsAsync();

        if (!products.Any())
        {
            _logger.LogWarning("No products found.");

            return NoContent();
        }

        _logger.LogInformation($"Returning {products.Count()} products.");

        return Ok(products);
    }

    /// <summary>
    /// Retrieves all sandwiches from the system (Sandwiches only).
    /// </summary>
    /// <returns>A list of sandwiches. If no sandwiches are found, returns a 204 No Content status.</returns>
    /// <response code="200">Returns a list of sandwiches.</response>
    /// <response code="204">No sandwiches found.</response>
    [HttpGet("sandwiches")]
    [ProducesResponseType(typeof(IEnumerable<ProductDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetSandwiches()
    {
        _logger.LogInformation($"GET /products/sandwiches called at: {DateTime.UtcNow}.");

        var sandwiches = await _productService.GetSandwichesAsync();

        if (!sandwiches.Any())
        {
            _logger.LogWarning($"No sandwiches found at: {DateTime.UtcNow}.");

            return NoContent();
        }

        _logger.LogInformation($"Returning {sandwiches.Count()} sandwiches.");

        return Ok(sandwiches);
    }

    /// <summary>
    /// Retrieves all extras from the system (Extras only).
    /// </summary>
    /// <returns>A list of extras. If no extras are found, returns a 204 No Content status.</returns>
    /// <response code="200">Returns a list of extras.</response>
    /// <response code="204">No extras found.</response>
    [HttpGet("extras")]
    [ProducesResponseType(typeof(IEnumerable<ProductDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExtras()
    {
        _logger.LogInformation($"GET /products/extras called at: {DateTime.UtcNow}.");

        var extras = await _productService.GetExtrasAsync();

        if (!extras.Any())
        {
            _logger.LogWarning($"No extras found at: {DateTime.UtcNow}");

            return NoContent();
        }

        _logger.LogInformation($"Returning {extras.Count()} extras.");

        return Ok(extras);
    }
}
