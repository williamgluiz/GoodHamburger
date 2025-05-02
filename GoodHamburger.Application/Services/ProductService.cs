using AutoMapper;
using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Models;
using Microsoft.Extensions.Logging;
using System;


namespace GoodHamburger.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, IMapper mapper, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            _logger.LogInformation($"GetAllProductsAsync called at: {DateTime.UtcNow}.");

            try
            {
                var products = await _productRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<ProductDTO>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting all products at: {DateTime.UtcNow}.");
                throw;
            }
        }

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
}
