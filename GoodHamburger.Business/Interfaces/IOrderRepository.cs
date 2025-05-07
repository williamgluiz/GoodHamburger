using GoodHamburger.Domain.Models;

namespace GoodHamburger.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        void MarkOrderItemForDeletion(OrderItem item);
        void MarkNewOrderItemAdded(OrderItem item);
    }
}
