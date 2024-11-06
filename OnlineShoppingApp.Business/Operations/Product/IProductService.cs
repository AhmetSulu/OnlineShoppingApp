using OnlineShoppingApp.Business.Operations.Products.Dtos;
using OnlineShoppingApp.Business.Types;

namespace OnlineShoppingApp.Business.Operations.Products
{
    public interface IProductService
    {
        Task<ServiceMessage> AddProduct(AddProductDto product);
        Task<ProductDto> GetProduct(int id);
        Task<List<ProductDto>> GetAllProducts();
        Task<ServiceMessage> DeleteProduct(int id);
        Task<ServiceMessage> UpdateProduct(UpdateProductDto product);
        Task<ServiceMessage> UpdateStock(int id, int quantity);
    }
}
