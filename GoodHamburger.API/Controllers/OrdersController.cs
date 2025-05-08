using GoodHamburger.Application.DTOs.Order;
using GoodHamburger.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrdersController"/> class.
    /// </summary>
    /// <param name="orderService">Service for managing orders.</param>
    /// <param name="logger">Logger instance for logging operations.</param>
    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all existing orders.
    /// </summary>
    /// <returns>
    /// Returns 200 OK with a list of all orders.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllOrders()
    {
        _logger.LogInformation($"GET /api/Orders called at: {DateTime.UtcNow}.");

        var orders = await _orderService.GetAllAsync();

        _logger.LogInformation($"Returning {orders.Count()} orders.");

        return Ok(orders);
    }

    /// <summary>
    /// Retrieves an order by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <returns>
    /// Returns 200 OK with the order details if found;  
    /// 404 Not Found if no order exists with the specified ID.
    /// </returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult<OrderResponseDTO>> GetById(Guid id)
    {
        _logger.LogInformation($"GET /api/Orders/{id} called at: {DateTime.UtcNow}.");

        var order = await _orderService.GetByIdAsync(id);

        if (order == null)
            return NotFound($"Order with ID {id} not found.");

        _logger.LogInformation($"Returning {order.Id}.");

        return Ok(order);
    }

    /// <summary>
    /// Creates a new order with the specified items.
    /// </summary>
    /// <param name="dto">The data required to create the order, including a list of product items.</param>
    /// <returns>
    /// Returns 201 Created with the created order if successful;  
    /// 400 Bad Request if the order contains duplicate product types (e.g., more than one sandwich, fries, or soda);  
    /// 500 Internal Server Error if an unexpected error occurs.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderResponseDTO>> Create([FromBody] CreateOrderDTO dto)
    {
        _logger.LogInformation("POST /api/Orders/ called at: {time}.", DateTime.UtcNow);

        try
        {
            var hasDuplicates = await _orderService.HasDuplicatedProductAsync(dto.Items);

            if (hasDuplicates)
                return BadRequest("The order cannot contain more than one sandwich, fries, or soda.");

            var result = await _orderService.CreateOrderAsync(dto);

            _logger.LogInformation($"Order created with ID: {result.Id}.");

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, $"Failed to create order: {ex.Message}.");
            return StatusCode(500, "Internal server error.");
        }
    }

    /// <summary>
    /// Updates an existing order with the specified ID using the provided data.
    /// </summary>
    /// <param name="id">The unique identifier of the order to update.</param>
    /// <param name="dto">The data used to update the order, including a list of items.</param>
    /// <returns>
    /// Returns 200 OK with the updated order if successful;  
    /// 400 Bad Request if the order contains duplicate product types;  
    /// 404 Not Found if the order does not exist;  
    /// 500 Internal Server Error if an unexpected error occurs.
    /// </returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] UpdateOrderDTO dto)
    {
        _logger.LogInformation("PUT /api/Orders/{id} called at: {{time}}.", DateTime.UtcNow);

        try
        {
            var hasDuplicates = await _orderService.HasDuplicatedProductAsync(dto.Items);

            if (hasDuplicates)
                return BadRequest("The order cannot contain more than one sandwich, fries, or soda.");

            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound($"Order with ID '{id}' not found.");

            var result = await _orderService.UpdateOrderAsync(id, dto);

            _logger.LogInformation($"Order with ID: {id} updated successfully.");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating order with ID {id}.");
            return StatusCode(500, "Internal server error.");
        }
    }

    /// <summary>
    /// Deletes an existing order by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order to be deleted.</param>
    /// <returns>
    /// Returns 204 No Content if the deletion is successful;  
    /// 404 Not Found if the order does not exist;  
    /// 500 Internal Server Error in case of an unexpected failure.
    /// </returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<OrderResponseDTO>> DeleteOrder(Guid id)
    {
        _logger.LogInformation($"DELETE /api/Orders/{id} called at: {DateTime.UtcNow}.");

        try
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order is null)
            {
                _logger.LogWarning($"Order with ID {id} not found.");
                return NotFound($"Order with ID {id} does not exist.");
            }

            await _orderService.DeleteAsync(id);

            _logger.LogInformation($"Order with ID: {id} deleted successfully.");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to delete order with ID {id}. Error: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}