//using GoodHamburger.API.DTOs.Order;
//using GoodHamburger.Domain.Interfaces;
//using Microsoft.AspNetCore.Mvc;


//namespace GoodHamburger.API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class OrdersController : ControllerBase
//    {
//        private readonly IOrderRepository _orderRepository;
//        private readonly ILogger<OrdersController> _logger;

//        public OrdersController(IOrderRepository orderRepository, ILogger<OrdersController> logger)
//        {
//            _orderRepository = orderRepository;
//            _logger = logger;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAllOrders()
//        {
//            var orders = await _orderRepository.GetAllAsync();
//            return Ok(orders);
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<OrderResponseDTO>> GetById(Guid id)
//        {
//            var order = await _orderRepository.GetByIdAsync(id);

//            if (order == null)
//                return NotFound($"Order with ID {id} not found.");

//            return Ok(order);
//        }

//        [HttpPost]
//        public async Task<ActionResult<OrderResponseDTO>> Create([FromBody] OrderDTO dto)
//        {
//            var result = await _orderRepository.AddAsync(dto);
//            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] UpdateOrderDTO dto)
//        {
//            var result = await _orderRepository.UpdateAsync(dto);

//            if (result == null)
//                return NotFound();

//            return Ok(result);
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteOrder(Guid id)
//        {
//            var result = await _orderRepository.DeleteAsync(id);

//            if (!result)
//                return NotFound();

//            return NoContent();
//        }
//    }
//}
