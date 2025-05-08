using GoodHamburger.Application.DTOs.Order;

namespace GoodHamburger.Application.Interfaces;

public interface IOrderService
{
    /// <summary>
    /// Creates a new order asynchronously based on the provided order data.
    /// </summary>
    /// <param name="dto">The data transfer object containing the order details to be created.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="OrderResponseDTO"/> object representing the created order.
    /// </returns>
    /// <remarks>
    /// This method creates a new order using the provided data and returns the corresponding <see cref="OrderResponseDTO"/> with the order details.
    /// </remarks>
    Task<OrderResponseDTO> CreateOrderAsync(CreateOrderDTO dto);

    /// <summary>
    /// Retrieves an order by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the order to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="OrderResponseDTO"/> object representing the order, or <c>null</c> if the order is not found.
    /// </returns>
    /// <remarks>
    /// This method retrieves a single order by its ID and maps it to an <see cref="OrderResponseDTO"/> object.
    /// </remarks>
    Task<OrderResponseDTO?> GetByIdAsync(Guid id);

    /// <summary>
    /// Retrieves all orders asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a collection of <see cref="OrderResponseDTO"/> objects representing all orders.
    /// </returns>
    /// <remarks>
    /// This method fetches all orders from the repository and returns them as a collection of <see cref="OrderResponseDTO"/> objects.
    /// </remarks>
    Task<IEnumerable<OrderResponseDTO>> GetAllAsync();

    /// <summary>
    /// Updates an existing order asynchronously based on the provided ID and updated order data.
    /// </summary>
    /// <param name="id">The unique identifier of the order to update.</param>
    /// <param name="dto">The data transfer object containing the updated order details.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="OrderResponseDTO"/> object representing the updated order.
    /// </returns>
    /// <remarks>
    /// This method updates an existing order with the new details and returns the corresponding <see cref="OrderResponseDTO"/> with the updated order.
    /// </remarks>
    Task<OrderResponseDTO> UpdateOrderAsync(Guid id, UpdateOrderDTO dto);

    /// <summary>
    /// Deletes an order asynchronously based on its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order to delete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// </returns>
    /// <remarks>
    /// This method deletes an order from the repository based on its ID.
    /// </remarks>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Checks if there are any duplicated product types in the provided list of order items asynchronously.
    /// </summary>
    /// <param name="items">A collection of <see cref="OrderItemDTO"/> objects representing the items in the order.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean indicating whether there are duplicated product types in the order items.
    /// </returns>
    /// <remarks>
    /// This method checks for duplicated product types in the order items and returns <c>true</c> if at least one product type appears more than once.
    /// </remarks>
    Task<bool> HasDuplicatedProductAsync(IEnumerable<OrderItemDTO> items);
}
