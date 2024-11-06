using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Business.Operations.Products;
using OnlineShoppingApp.Business.Operations.Products.Dtos;
using OnlineShoppingApp.Data.Enums;
using OnlineShoppingApp.WebApi.Models.Product;

namespace OnlineShoppingApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        // Constructor to inject the product service
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // ----------------------------------------------------------------------------------------------

        // Endpoint for adding a new product, accessible only to Admin users
        [Authorize(Roles = "Admin")]
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(AddProductRequest request)
        {
            // Create DTO for the new product
            var addProductDto = new AddProductDto
            {
                ProductName = request.ProductName,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId,
                StockQuantity = request.StockQuantity,
                ProductCategory = request.ProductCategory
            };

            // Attempt to add the product
            var result = await _productService.AddProduct(addProductDto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            else
                return Ok(new { Message = "Product added successfully!" });
        }

        // ----------------------------------------------------------------------------------------------

        // Endpoint for retrieving a specific product by ID
        [HttpGet("{id}/GetProduct")]
        public async Task<IActionResult> GetProduct(int id)
        {
            // Attempt to retrieve the product
            var product = await _productService.GetProduct(id);

            // Check if the product exists
            if (product is null)
                return NotFound("Product not found.");
            else
                return Ok(product);
        }

        // ----------------------------------------------------------------------------------------------

        // Endpoint for retrieving all products
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            // Retrieve all products
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        // ----------------------------------------------------------------------------------------------

        // Endpoint for deleting a product by ID, accessible only to Admin users
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}/DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // Attempt to delete the product
            var result = await _productService.DeleteProduct(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            else
                return Ok(new { Message = "Product deleted successfully!" });
        }

        // ----------------------------------------------------------------------------------------------

        // Endpoint for updating an existing product by ID, accessible only to Admin users
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductRequest request)
        {
            // Create DTO for updating the product
            var updateProductDto = new UpdateProductDto
            {
                Id = id,
                ProductName = request.ProductName,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId,
                StockQuantity = request.StockQuantity,
                ProductCategory = request.ProductCategory
            };

            // Attempt to update the product
            var result = await _productService.UpdateProduct(updateProductDto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            else
                return Ok(new { Message = "Product updated successfully!" });
        }

        // ----------------------------------------------------------------------------------------------

        // Endpoint for updating the stock quantity of a product by ID, accessible only to Admin users
        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}/UpdateStock")]
        public async Task<IActionResult> UpdateStock(int id, int stockQuantity)
        {
            // Attempt to update the stock quantity
            var result = await _productService.UpdateStock(id, stockQuantity);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            else
                return Ok(new { Message = "Stock updated successfully!" });
        }

        // ----------------------------------------------------------------------------------------------
    }
}
