namespace GoodHamburger.Domain.Models
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public ProductType Type { get; set; }
    }
}
