using GoodHamburger.Application.DTOs.Product;

namespace GoodHamburger.Application.Interfaces;

public interface IProductService
{
    /// <summary>
    /// Retrieves all products asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a collection of <see cref="ProductDTO"/> objects representing all products.
    /// </returns>
    /// <remarks>
    /// This method fetches all products from the repository without applying any filtering.
    /// </remarks>
    Task<IEnumerable<ProductDTO>> GetAllProductsAsync();

    /// <summary>
    /// Retrieves all products of type "Sandwich" asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a collection of <see cref="ProductDTO"/> objects representing sandwiches.
    /// </returns>
    /// <remarks>
    /// This method filters products by the "Sandwich" type and returns only those products in the result.
    /// </remarks>
    Task<IEnumerable<ProductDTO>> GetSandwichesAsync();

    /// <summary>
    /// Retrieves all products categorized as "Extras" asynchronously.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a collection of <see cref="ProductDTO"/> objects representing extra items (e.g., Fries, Soft Drinks, etc.).
    /// </returns>
    /// <remarks>
    /// This method filters products by the "Extras" category, which includes items like fries and soft drinks.
    /// </remarks>
    Task<IEnumerable<ProductDTO>> GetExtrasAsync();

    /// <summary>
    /// Retrieves a specific product by its ID asynchronously.
    /// </summary>
    /// <param name="productId">The unique identifier of the product to retrieve.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="ProductDTO"/> object representing the product, or <c>null</c> if the product is not found.
    /// </returns>
    /// <remarks>
    /// This method retrieves a single product by its ID and maps it to a <see cref="ProductDTO"/> object.
    /// </remarks>
    Task<ProductDTO?> GetProductByIdAsync(Guid productId);
}
