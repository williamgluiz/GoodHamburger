using GoodHamburger.Domain.Models;

namespace GoodHamburger.Domain.Services
{
    public class DiscountService
    {
        public (decimal discountPercent, decimal finalAmount) ApplyDiscount(List<OrderItem> items)
        {
            if(items == null || !items.Any())
                return (0, 0);

            var total = items.Sum(item => item.Product.Price * item.Quantity);

            bool hasSandwich = items.Any(item => item.Product.Type == ProductType.Sandwich);
            bool hasDrink = items.Any(item => item.Product.Name.Equals("Soft Drink", StringComparison.OrdinalIgnoreCase));
            bool hasFries = items.Any(item => item.Product.Name.Equals("Fries", StringComparison.OrdinalIgnoreCase));

            decimal discount = 0;

            if (hasSandwich && hasDrink && hasFries)
                discount = 0.20m; // 20% discount
            else if (hasSandwich && hasDrink)
                discount = 0.15m; // 15% discount
            else if (hasSandwich && hasFries)
                discount = 0.10m; // 10% discount

            var finalAmount = total * (1 - discount);

            return (discount * 100, finalAmount);
        }
    }
}
