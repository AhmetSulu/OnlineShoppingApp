using OnlineShoppingApp.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.WebApi.Models.Product
{
    public class AddProductRequest
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string ProductName { get; set; }

        // ----------------------------------------------------------------------------------------------

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        // ----------------------------------------------------------------------------------------------

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        // ----------------------------------------------------------------------------------------------

        [Required(ErrorMessage = "Stock quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int StockQuantity { get; set; }

        // ----------------------------------------------------------------------------------------------

        [Required(ErrorMessage = "Category ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Category ID must be greater than zero.")]
        public int CategoryId { get; set; }

        // ----------------------------------------------------------------------------------------------

        [Required(ErrorMessage = "Category is required.")]
        public ProductCategory ProductCategory { get; set; }

        // ----------------------------------------------------------------------------------------------
    }
}
