using System.ComponentModel.DataAnnotations;

namespace GoodHamburger.Application.DTOs.Order;

public class OrderItemDTO
{
    [Required(ErrorMessage = "ProductId is required.")]
    public Guid ProductId { get; set; }

    [Range(1, 1, ErrorMessage = "Quantity must be 1.")]
    public int Quantity { get; set; }
}
