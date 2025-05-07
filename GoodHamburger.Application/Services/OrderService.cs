using AutoMapper;
using FluentValidation;
using GoodHamburger.Application.DTOs.Order;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Models;

namespace GoodHamburger.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IProductRepository productRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderResponseDTO> CreateOrderAsync(CreateOrderDTO dto)
        {
            var order = new Order { Items = new List<OrderItem>() };

            foreach (var item in dto.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Product {item.ProductId} not found");

                order.Items.Add(new OrderItem
                {
                    Product = product,
                    ProductId = product.Id,
                    Quantity = item.Quantity
                });
            }

            CalculateOrder(order);

            await _orderRepository.AddAsync(order);

            return _mapper.Map<OrderResponseDTO>(order);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                return false;

            await _orderRepository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderResponseDTO>>(orders);
        }

        public async Task<OrderResponseDTO?> GetByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                return null;

            return _mapper.Map<OrderResponseDTO>(order);
        }

        public async Task<OrderResponseDTO> UpdateOrderAsync(Guid id, UpdateOrderDTO dto)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                return null;

            order.Items.Clear();

            //_orderRepository.Attach(order);

            foreach (var item in dto.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    throw new Exception($"Product with id {item.ProductId} not found.");

                order.Items.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Product = product,
                    Quantity = item.Quantity
                });
            }

            CalculateOrder(order);

            await _orderRepository.UpdateAsync(order);

            return _mapper.Map<OrderResponseDTO>(order);
        }

        public async Task<bool> HasDuplicatedProductTypesAsync(IEnumerable<OrderItemDTO> items)
        {
            var productTypes = new List<string>();

            foreach (var item in items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);

                if (product == null) return true; 

                productTypes.Add(product.Type.ToString());
            }

            var duplicatedTypes = productTypes
                .GroupBy(t => t)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            return duplicatedTypes.Count > 0;
        }

        private void CalculateOrder(Order order)
        {
            order.TotalAmount = order.Items.Sum(i => i.Product.Price * i.Quantity);

            var hasSandwich = order.Items.Any(i => i.Product.Type == ProductType.Sandwich);
            var hasFries = order.Items.Any(i => i.Product.Name.ToLower().Contains("fries"));
            var hasDrink = order.Items.Any(i => i.Product.Name.ToLower().Contains("soft drink"));

            order.DiscountPercentage = (hasSandwich && hasFries && hasDrink) ? 20 :
                                       (hasSandwich && hasDrink) ? 15 :
                                       (hasSandwich && hasFries) ? 10 : 0;

            var discount = order.TotalAmount * (order.DiscountPercentage / 100);
            order.FinalAmount = Math.Round(order.TotalAmount - discount, 2);
        }
    }
}
