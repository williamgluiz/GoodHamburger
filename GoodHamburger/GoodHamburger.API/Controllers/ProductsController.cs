using GoodHamburger.API.ViewModels.Product;
using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllProducts()
        {
            _logger.LogInformation($"Get /products called at: {DateTime.UtcNow}.");

            try
            {
                var products = await _productRepository.GetAllAsync();

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
        [ProducesResponseType(typeof(IEnumerable<ProductViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetSandwiches()
        {
            _logger.LogInformation($"GET /products/sandwiches called at: {DateTime.UtcNow}.");

            try
            {
                var sandwiches = await _productRepository.GetByTypeAsync(ProductType.Sandwich);

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
        [ProducesResponseType(typeof(IEnumerable<ProductViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetExtras()
        {
            _logger.LogInformation($"GET /products/extras called at: {DateTime.UtcNow}.");

            try
            {
                var extras = await _productRepository.GetByTypeAsync(ProductType.Extra);

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
