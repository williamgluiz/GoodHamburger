namespace GoodHamburger.Domain.Models
{
    public class Order : Entity
    {
        public List<OrderItem> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal FinalAmount { get; set; }
    }
}
