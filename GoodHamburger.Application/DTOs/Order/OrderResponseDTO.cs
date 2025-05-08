namespace GoodHamburger.Application.DTOs.Order;

public class OrderResponseDTO
{
    public Guid Id { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal FinalAmount { get; set; }
    public List<OrderItemResponseDTO> Items { get; set; } = new();
}
