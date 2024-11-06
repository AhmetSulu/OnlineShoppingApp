using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Business.Operations.Products.Dtos;
using OnlineShoppingApp.Business.Types;
using OnlineShoppingApp.Data.Entities;
using OnlineShoppingApp.Data.Repositories;
using OnlineShoppingApp.Data.UnitOfWork;

namespace OnlineShoppingApp.Business.Operations.Products
{
    public class ProductManager : IProductService
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IRepository<ProductEntity> _productRepository;

        public ProductManager(IUnitOfWork unitOfWork, IRepository<ProductEntity> productRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        // ----------------------------------------------------------------------------------------------

        // Adds a new product to the database.
        public async Task<ServiceMessage> AddProduct(AddProductDto product)
        {
            // Check if a product with the same name already exists (case-insensitive).
            var hasProduct = _productRepository.GetAll(x => x.ProductName.ToLower() == product.ProductName.ToLower()).Any();

            if (hasProduct)
            {
                return new ServiceMessage
                {
                    IsSuccess = false,
                    Message = "This product is already in use." // Return an error if the product exists.
                };
            }

            await _unitOfWork.BeginTransactionAsync(); // Start a new transaction.
            var productEntity = new ProductEntity
            {
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                ProductCategory = product.ProductCategory
            };

            _productRepository.Add(productEntity); // Add the new product entity to the repository.
            try
            {
                await _unitOfWork.SaveChangesAsync(); // Save changes to the database.
                await _unitOfWork.CommitTransaction(); // Commit the transaction if successful.
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while adding the product."); // Handle any exceptions.
            }

            return new ServiceMessage
            {
                IsSuccess = true,
                Message = "Product added successfully." // Return success message.
            };
        }

        // ----------------------------------------------------------------------------------------------

        // Deletes a product by its ID.
        public async Task<ServiceMessage> DeleteProduct(int id)
        {
            var product = _productRepository.GetById(id); // Retrieve the product by ID.

            if (product is null)
            {
                return new ServiceMessage
                {
                    IsSuccess = false,
                    Message = "Product not found." // Return error if the product doesn't exist.
                };
            }

            _productRepository.Delete(id); // Delete the product from the repository.
            try
            {
                await _unitOfWork.SaveChangesAsync(); // Save changes to the database.
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while deleting the product."); // Handle exceptions.
            }

            return new ServiceMessage
            {
                IsSuccess = true,
                Message = "Product deleted successfully." // Return success message.
            };
        }

        // ----------------------------------------------------------------------------------------------

        // Retrieves a list of all products.
        public async Task<List<ProductDto>> GetAllProducts()
        {
            var product = await _productRepository.GetAll() // Fetch all products.
                                                  .Select(x => new ProductDto()
                                                  {
                                                      Id = x.Id,
                                                      ProductName = x.ProductName,
                                                      Description = x.Description,
                                                      Price = x.Price,
                                                      StockQuantity = x.StockQuantity,
                                                      CategoryId = x.CategoryId,
                                                      ProductCategory = x.ProductCategory
                                                  }).ToListAsync();

            return product; // Return the list of products.
        }

        // ----------------------------------------------------------------------------------------------

        // Retrieves a specific product by its ID.
        public async Task<ProductDto> GetProduct(int id)
        {
            var product = await _productRepository.GetAll(x => x.Id == id) // Fetch product by ID.
                                                  .Select(x => new ProductDto()
                                                  {
                                                      Id = x.Id,
                                                      ProductName = x.ProductName,
                                                      Description = x.Description,
                                                      Price = x.Price,
                                                      StockQuantity = x.StockQuantity,
                                                      CategoryId = x.CategoryId,
                                                      ProductCategory = x.ProductCategory
                                                  }).FirstOrDefaultAsync();

            return product; // Return the product details.
        }

        // ----------------------------------------------------------------------------------------------

        // Updates an existing product's details.
        public async Task<ServiceMessage> UpdateProduct(UpdateProductDto product)
        {
            var productEntity = _productRepository.GetById(product.Id); // Retrieve the product to update.

            if (productEntity is null)
            {
                return new ServiceMessage
                {
                    IsSuccess = false,
                    Message = "Product not found." // Return error if the product doesn't exist.
                };
            }

            await _unitOfWork.BeginTransactionAsync(); // Start a new transaction.

            // Update product properties with new values.
            productEntity.ProductName = product.ProductName;
            productEntity.Description = product.Description;
            productEntity.Price = product.Price;
            productEntity.StockQuantity = product.StockQuantity;
            productEntity.CategoryId = product.CategoryId;
            productEntity.ProductCategory = product.ProductCategory;

            _productRepository.Update(productEntity); // Update the product in the repository.
            try
            {
                await _unitOfWork.SaveChangesAsync(); // Save changes to the database.
                await _unitOfWork.CommitTransaction(); // Commit the transaction if successful.
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction(); // Rollback the transaction on error.
                throw new Exception("An error occurred while updating the product."); // Handle exceptions.
            }

            return new ServiceMessage
            {
                IsSuccess = true,
                Message = "Product updated successfully." // Return success message.
            };
        }

        // ----------------------------------------------------------------------------------------------

        // Updates the stock quantity of a specific product.
        public async Task<ServiceMessage> UpdateStock(int id, int quantity)
        {
            var product = _productRepository.GetById(id); // Retrieve the product by ID.
            if (product is null)
            {
                return new ServiceMessage
                {
                    IsSuccess = false,
                    Message = "Product not found." // Return error if the product doesn't exist.
                };
            }

            product.StockQuantity = quantity; // Update stock quantity.

            _productRepository.Update(product); // Update the product in the repository.
            try
            {
                await _unitOfWork.SaveChangesAsync(); // Save changes to the database.
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while updating the stock."); // Handle exceptions.
            }

            return new ServiceMessage
            {
                IsSuccess = true,
                Message = "Stock updated successfully." // Return success message.
            };
        }
    }
}
