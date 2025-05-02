namespace GoodHamburger.Domain.Models
{
    public class OrderItem : Entity
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = default!;
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public int Quantity { get; set; } = 1;
    }
}
