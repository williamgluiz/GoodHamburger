namespace GoodHamburger.API.ViewModels.Product
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}
