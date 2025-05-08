using AutoMapper;
using GoodHamburger.Application.DTOs.Order;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Models;
using GoodHamburger.Domain.Services;

namespace GoodHamburger.Application.Services;

public class OrderService : IOrderService
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly DiscountService _discountService;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrderService"/> class with the specified repositories and mapper.
    /// </summary>
    /// <param name="productRepository">The repository for managing products.</param>
    /// <param name="orderRepository">The repository for managing orders.</param>
    /// <param name="mapper">The mapper used for object-to-object mapping.</param>
    public OrderService(IProductRepository productRepository, IOrderRepository orderRepository, IMapper mapper, DiscountService discountService)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _mapper = mapper;
        _discountService = discountService;
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
        var order = new Order { Items = [] };

        foreach (var item in dto.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId) ?? throw new Exception($"Product {item.ProductId} not found");

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
        var order = await _orderRepository.GetByIdAsync(id) ?? throw new Exception($"Order {id} not found");

        foreach (var existingItem in order.Items.ToList())
        {
            _orderRepository.MarkOrderItemForDeletion(existingItem);
            order.Items.Remove(existingItem);
        }

        foreach (var itemDto in dto.Items)
        {
            var product = await _productRepository.GetByIdAsync(itemDto.ProductId) ?? throw new Exception($"Product {itemDto.ProductId} not found");

            var newItem = new OrderItem
            {
                Product = product,
                ProductId = product.Id,
                Quantity = itemDto.Quantity,
                Order = order,
                OrderId = order.Id   
            };

            _orderRepository.MarkNewOrderItemAdded(newItem);
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
    /// Checks if there are any duplicated product categories in the provided list of order items asynchronously.
    /// </summary>
    /// <param name="items">A collection of <see cref="OrderItemDTO"/> objects representing the items in the order.</param>
    /// <returns>
    /// <c>true</c> if there are duplicated product categories; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// This method checks for duplicated product categories in the order items and returns <c>true</c> if at least one category appears more than once.
    /// It also returns <c>true</c> if any product referenced in the items is not found in the repository.
    /// </remarks>
    public async Task<bool> HasDuplicatedProductAsync(IEnumerable<OrderItemDTO> items)
    {
        var categorySet = new HashSet<string>();

        foreach (var item in items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null) return true;

            var category = ProductCategoryResolver.GetCategory(product);

            if (!categorySet.Add(category))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Calculates and sets the total amount, discount percentage, and final amount for the specified order.
    /// </summary>
    /// <param name="order">The <see cref="Order"/> object to calculate values for.</param>
    /// <remarks>
    /// This method delegates the discount logic to <see cref="DiscountService.ApplyDiscount"/> and sets the computed values in the order:
    /// - <c>TotalAmount</c> is the sum of all item prices multiplied by their quantity.
    /// - <c>DiscountPercentage</c> and <c>FinalAmount</c> are obtained from the discount service based on the order's items.
    /// </remarks>
    private void CalculateOrder(Order order)
    {
        var (discountPercent, finalAmount) = _discountService.ApplyDiscount(order.Items);
        order.TotalAmount = order.Items.Sum(i => i.Product.Price * i.Quantity);
        order.DiscountPercentage = discountPercent;
        order.FinalAmount = finalAmount;
    }
}