using GoodHamburger.Domain.Models;

namespace GoodHamburger.Domain.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    /// <summary>
    /// Marks an <see cref="OrderItem"/> as deleted in the context, without immediately removing it from the database.
    /// </summary>
    /// <param name="item">The <see cref="OrderItem"/> to be marked for deletion.</param>
    void MarkOrderItemForDeletion(OrderItem item);

    /// <summary>
    /// Marks a new <see cref="OrderItem"/> as added in the context, preparing it for insertion into the database.
    /// </summary>
    /// <param name="item">The <see cref="OrderItem"/> to be marked as added.</param>
    void MarkNewOrderItemAdded(OrderItem item);
}
