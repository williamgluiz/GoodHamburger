using GoodHamburger.Data.Context;
using GoodHamburger.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.API.Configurations
{
    public static class DatabaseSeeder
    {
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

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
    }
}
