using GoodHamburger.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Data.Context;

public static class DatabaseSeeder
{
    /// <summary>
    /// Seeds the database with initial product data if no products exist.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Products.AnyAsync())
            return;

        var products = new List<Product>
        {
            new Sandwich { Name = "X Bacon", Price = 7.0m },
            new Sandwich { Name = "X Burger", Price = 5.0m },
            new Sandwich { Name = "X Egg", Price = 4.5m },
            new Extra { Name = "Fries", Price = 2.0m },
            new Extra { Name = "Soft Drink", Price = 2.5m }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
    }
}
