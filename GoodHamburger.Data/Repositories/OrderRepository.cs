using GoodHamburger.Data.Context;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Data.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderRepository"/> class with the specified database context.
    /// </summary>
    /// <param name="db">The application's database context.</param>
    public OrderRepository(AppDbContext db) : base(db) { }

    /// <summary>
    /// Retrieves all orders, including their associated items and products, asynchronously.
    /// </summary>
    /// <returns>A collection of <see cref="Order"/> objects with their related items and products.</returns>
    public override async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await DbSet
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Product)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves an order by its unique identifier, including its associated items and products, asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <returns>An <see cref="Order"/> object if found; otherwise, <c>null</c>.</returns>
    public override async Task<Order?> GetByIdAsync(Guid id)
    {
        return await Db.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    /// <summary>
    /// Marks an <see cref="OrderItem"/> as deleted in the context, without immediately removing it from the database.
    /// </summary>
    /// <param name="item">The <see cref="OrderItem"/> to be marked for deletion.</param>
    public void MarkOrderItemForDeletion(OrderItem item)
    {
        Db.Entry(item).State = EntityState.Deleted;
    }

    /// <summary>
    /// Marks a new <see cref="OrderItem"/> as added in the context, preparing it for insertion into the database.
    /// </summary>
    /// <param name="item">The <see cref="OrderItem"/> to be marked as added.</param>
    public void MarkNewOrderItemAdded(OrderItem item)
    {
        Db.Entry(item).State = EntityState.Added;
    }
}
