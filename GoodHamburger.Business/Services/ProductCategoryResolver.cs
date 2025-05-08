using GoodHamburger.Domain.Models;

namespace GoodHamburger.Domain.Services;

public static class ProductCategoryResolver
{
    /// <summary>
    /// Determines the category of a given product based on its type and name.
    /// </summary>
    /// <param name="product">The <see cref="Product"/> object whose category is to be determined.</param>
    /// <returns>
    /// A string representing the category of the product, such as "Sandwich", "Fries", "SoftDrink", or "Other".
    /// </returns>
    /// <remarks>
    /// The category is determined based on the following conditions:
    /// - "Sandwich" if the product type is <see cref="ProductType.Sandwich"/>.
    /// - "Fries" if the product name contains "Fries".
    /// - "SoftDrink" if the product name contains "Soft Drink".
    /// - "Other" if none of the above conditions are met.
    /// </remarks>
    public static string GetCategory(Product product)
    {
        if (product.Type == ProductType.Sandwich)
            return "Sandwich";

        if (product.Name.Contains("Fries", StringComparison.OrdinalIgnoreCase))
            return "Fries";

        if (product.Name.Contains("Soft Drink", StringComparison.OrdinalIgnoreCase))
            return "SoftDrink";

        return "Other";
    }
}
