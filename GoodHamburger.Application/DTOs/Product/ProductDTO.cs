using System.ComponentModel.DataAnnotations;

namespace GoodHamburger.Application.DTOs.Product
{
    public class ProductDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

       [Required]
        public string Type { get; set; } = string.Empty;
    }
}
