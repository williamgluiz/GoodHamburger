namespace GoodHamburger.Application.DTOs.Order;

public class OrderItemResponseDTO
{
    public Guid ProductId { get; set; }
    public string Name { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string Type { get; set; } = default!;
}
