using GoodHamburger.Application.DTOs.Order;

namespace GoodHamburger.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDTO> CreateOrderAsync(CreateOrderDTO dto);
        Task<OrderResponseDTO?> GetByIdAsync(Guid id);
        Task<IEnumerable<OrderResponseDTO>> GetAllAsync();
        Task<OrderResponseDTO> UpdateOrderAsync(Guid id, UpdateOrderDTO dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> HasDuplicatedProductTypesAsync(IEnumerable<OrderItemDTO> items);
    }
}
