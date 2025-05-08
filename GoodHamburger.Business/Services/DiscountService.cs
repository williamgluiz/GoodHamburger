using GoodHamburger.Domain.Models;

namespace GoodHamburger.Domain.Services;

public class DiscountService
{
    /// <summary>
    /// Calculates the discount percentage and final amount for a list of order items based on their categories.
    /// </summary>
    /// <param name="items">A list of <see cref="OrderItem"/> objects representing the items in the order.</param>
    /// <returns>
    /// A tuple containing:
    /// <list type="bullet">
    /// <item>
    /// <description>The discount percentage applied (as a value between 0 and 100).</description>
    /// </item>
    /// <item>
    /// <description>The final amount after applying the discount.</description>
    /// </item>
    /// </list>
    /// </returns>
    /// <remarks>
    /// The discount is determined using the following rules:
    /// - 20% discount if the order contains a sandwich, a soft drink, and fries.
    /// - 15% discount if the order contains a sandwich and a soft drink.
    /// - 10% discount if the order contains a sandwich and fries.
    /// No discount is applied for other combinations.
    /// 
    /// Product categories are resolved via <see cref="ProductCategoryResolver.GetCategory(Product)"/>.
    /// </remarks>
    public (decimal discountPercent, decimal finalAmount) ApplyDiscount(List<OrderItem> items)
    {
        if (items == null || !items.Any())
            return (0, 0);

        var total = items.Sum(item => item.Product.Price * item.Quantity);

        bool hasSandwich = false;
        bool hasDrink = false;
        bool hasFries = false;

        foreach (var item in items)
        {
            var category = ProductCategoryResolver.GetCategory(item.Product);

            if (category.Equals("Sandwich", StringComparison.OrdinalIgnoreCase))
                hasSandwich = true;
            else if (category.Equals("SoftDrink", StringComparison.OrdinalIgnoreCase))
                hasDrink = true;
            else if (category.Equals("Fries", StringComparison.OrdinalIgnoreCase))
                hasFries = true;
        }

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
