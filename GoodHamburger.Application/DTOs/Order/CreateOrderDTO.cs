using System.ComponentModel.DataAnnotations;

namespace GoodHamburger.Application.DTOs.Order
{
    public class CreateOrderDTO
    {
        [Required(ErrorMessage = "Order must contain at least one item.")]
        [MinLength(1, ErrorMessage = "Order must contain at least one item.")]
        public List<OrderItemDTO> Items { get; set; } = new();
    }
}
