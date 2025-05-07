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

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/> class with the specified repositories and mapper.
        /// </summary>
        /// <param name="productRepository">The repository for managing products.</param>
        /// <param name="orderRepository">The repository for managing orders.</param>
        /// <param name="mapper">The mapper used for object-to-object mapping.</param>
        public OrderService(IProductRepository productRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all orders asynchronously and maps them to <see cref="OrderResponseDTO"/> objects.
        /// </summary>
        /// <returns>A collection of <see cref="OrderResponseDTO"/> objects representing all orders.</returns>
        public async Task<IEnumerable<OrderResponseDTO>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderResponseDTO>>(orders);
        }

        /// <summary>
        /// Retrieves an order by its unique identifier asynchronously and maps it to an <see cref="OrderResponseDTO"/> object.
        /// </summary>
        /// <param name="id">The unique identifier of the order to retrieve.</param>
        /// <returns>
        /// An <see cref="OrderResponseDTO"/> object representing the order if found; otherwise, <c>null</c>.
        /// </returns>
        public async Task<OrderResponseDTO?> GetByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                return null;

            return _mapper.Map<OrderResponseDTO>(order);
        }

        /// <summary>
        /// Creates a new order asynchronously using the provided <see cref="CreateOrderDTO"/> and maps it to an <see cref="OrderResponseDTO"/> object.
        /// </summary>
        /// <param name="dto">The data transfer object containing information about the order to be created.</param>
        /// <returns>An <see cref="OrderResponseDTO"/> object representing the created order.</returns>
        /// <exception cref="Exception">Thrown when a product referenced in the order is not found in the repository.</exception>
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

        /// <summary>
        /// Updates an existing order asynchronously by replacing its items with the new ones provided in the <see cref="UpdateOrderDTO"/>.
        /// </summary>
        /// <param name="id">The unique identifier of the order to update.</param>
        /// <param name="dto">The data transfer object containing the new items for the order.</param>
        /// <returns>An <see cref="OrderResponseDTO"/> object representing the updated order.</returns>
        /// <exception cref="Exception">
        /// Thrown if the order with the given <paramref name="id"/> is not found, or if any product referenced in the update is not found in the repository.
        /// </exception>
        public async Task<OrderResponseDTO> UpdateOrderAsync(Guid id, UpdateOrderDTO dto)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                throw new Exception($"Order {id} not found");

            foreach (var existingItem in order.Items.ToList())
            {
                _orderRepository.MarkOrderItemForDeletion(existingItem);
                order.Items.Remove(existingItem);
            }

            foreach (var itemDto in dto.Items)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null)
                    throw new Exception($"Product {itemDto.ProductId} not found");

                var newItem = new OrderItem
                {
                    Product = product,
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    Order = order,        
                    OrderId = order.Id   
                };

                _orderRepository.MarkNewOrderItemAdded(newItem);
                order.Items.Add(newItem);
            }

            CalculateOrder(order);

            await _orderRepository.UpdateAsync(order);

            return _mapper.Map<OrderResponseDTO>(order);
        }

        /// <summary>
        /// Deletes an order by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the order to delete.</param>
        /// <exception cref="Exception">Thrown when the order with the specified <paramref name="id"/> is not found in the repository.</exception>
        public async Task DeleteAsync(Guid id)
        {
            await _orderRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Checks if there are any duplicated product types in the provided list of order items asynchronously.
        /// </summary>
        /// <param name="items">A collection of <see cref="OrderItemDTO"/> objects representing the items in the order.</param>
        /// <returns>
        /// <c>true</c> if there are duplicated product types; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method checks for duplicated product types in the order items and returns <c>true</c> if at least one product type appears more than once.
        /// It also returns <c>true</c> if any product referenced in the items is not found in the repository.
        /// </remarks>
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

        /// <summary>
        /// Calculates the total amount, discount percentage, and final amount for the specified order based on its items.
        /// </summary>
        /// <param name="order">The <see cref="Order"/> object to calculate the amounts for.</param>
        /// <remarks>
        /// The method checks for the presence of sandwiches, fries, and soft drinks in the order's items and applies a discount based on the following conditions:
        /// - 20% discount if the order contains a sandwich, fries, and a soft drink.
        /// - 15% discount if the order contains a sandwich and a soft drink.
        /// - 10% discount if the order contains a sandwich and fries.
        /// No discount is applied if the order doesn't meet any of the above conditions.
        /// The final amount is rounded to two decimal places.
        /// </remarks>
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