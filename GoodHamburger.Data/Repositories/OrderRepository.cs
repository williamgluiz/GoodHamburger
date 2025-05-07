using GoodHamburger.Data.Context;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Data.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext db) : base(db) { }

        public override async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await DbSet
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        public override async Task<Order?> GetByIdAsync(Guid id)
        {
            return await Db.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public void MarkOrderItemForDeletion(OrderItem item)
        {
            Db.Entry(item).State = EntityState.Deleted;
        }

        public void MarkNewOrderItemAdded(OrderItem item)
        {
            Db.Entry(item).State = EntityState.Added;
        }
    }
}
