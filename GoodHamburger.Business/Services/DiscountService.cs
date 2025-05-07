using GoodHamburger.Domain.Models;

namespace GoodHamburger.Domain.Services;

public class DiscountService
{
    /// <summary>
    /// Calculates the discount and final amount for a list of order items based on specific conditions.
    /// </summary>
    /// <param name="items">A list of <see cref="OrderItem"/> objects representing the items in the order.</param>
    /// <returns>
    /// A tuple containing:
    /// <list type="bullet">
    /// <item>
    /// <description>The discount percentage applied (as a decimal).</description>
    /// </item>
    /// <item>
    /// <description>The final amount after the discount has been applied.</description>
    /// </item>
    /// </list>
    /// </returns>
    /// <remarks>
    /// The discount is applied based on the following conditions:
    /// - 20% discount if the order contains a sandwich, drink, and fries.
    /// - 15% discount if the order contains a sandwich and drink.
    /// - 10% discount if the order contains a sandwich and fries.
    /// No discount is applied for other combinations of items.
    /// </remarks>
    //TODO: WTF???
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
