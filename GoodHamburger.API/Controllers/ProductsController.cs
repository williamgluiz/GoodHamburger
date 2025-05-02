using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllProducts()
        {
            _logger.LogInformation($"Get /products called at: {DateTime.UtcNow}.");

            try
            {
                var products = await _productService.GetAllProductsAsync();

                if (!products.Any())
                {
                    _logger.LogWarning("No products found.");

                    return NoContent();
                }

                _logger.LogInformation($"Returning {products.Count()} products.");

                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting products at: {DateTime.UtcNow}.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("sandwiches")]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetSandwiches()
        {
            _logger.LogInformation($"GET /products/sandwiches called at: {DateTime.UtcNow}.");

            try
            {
                var sandwiches = await _productService.GetSandwichesAsync();

                if (!sandwiches.Any())
                {
                    _logger.LogWarning($"No sandwiches found at: {DateTime.UtcNow}.");

                    return NoContent();
                }

                _logger.LogInformation($"Returning {sandwiches.Count()} sandwiches.");

                return Ok(sandwiches);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting sandwiches at: {DateTime.UtcNow}.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("extras")]
        [ProducesResponseType(typeof(IEnumerable<ProductDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetExtras()
        {
            _logger.LogInformation($"GET /products/extras called at: {DateTime.UtcNow}.");

            try
            {
                var extras = await _productService.GetExtrasAsync();

                if (!extras.Any())
                {
                    _logger.LogWarning($"No extras found at: {DateTime.UtcNow}");
                    
                    return NoContent();
                }

                _logger.LogInformation($"Returning {extras.Count()} extras.");

                return Ok(extras);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting extras at: {DateTime.UtcNow}.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
