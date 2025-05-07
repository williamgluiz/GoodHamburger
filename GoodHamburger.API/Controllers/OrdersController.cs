using GoodHamburger.Application.DTOs.Order;
using GoodHamburger.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

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

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderResponseDTO>> GetById(Guid id)
    {
        _logger.LogInformation($"GET /api/Orders/{id} called at: {DateTime.UtcNow}.");

        var order = await _orderService.GetByIdAsync(id);

        if (order == null)
            return NotFound($"Order with ID {id} not found.");

        _logger.LogInformation($"Returning {order.Id}.");

        return Ok(order);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderResponseDTO>> Create([FromBody] CreateOrderDTO dto)
    {
        _logger.LogInformation("POST /api/Orders/ called at: {time}.", DateTime.UtcNow);

        try
        {
            var hasDuplicates = await _orderService.HasDuplicatedProductTypesAsync(dto.Items);

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

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] UpdateOrderDTO dto)
    {
        _logger.LogInformation("PUT /api/Orders/{id} called at: {{time}}.", DateTime.UtcNow);

        try
        {
            var hasDuplicates = await _orderService.HasDuplicatedProductTypesAsync(dto.Items);

            if (hasDuplicates)
                return BadRequest("The order cannot contain more than one sandwich, fries, or soda.");

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


    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        _logger.LogInformation("DELETE /api/Orders/{id} called at: {{time}}.", DateTime.UtcNow);

        try
        {
            var result = await _orderService.DeleteAsync(id);

            if (!result)
                return NotFound();

            _logger.LogInformation($"Order with ID: {id} deleted.");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting order with ID {id}.");
            return StatusCode(500, "Internal server error");
        }
    }
}