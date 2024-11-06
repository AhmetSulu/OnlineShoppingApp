using OnlineShoppingApp.Business.Operations.Order.Dtos;
using OnlineShoppingApp.Business.Types;

namespace OnlineShoppingApp.Business.Operations.Order
{
    public interface IOrderService
    {
        Task<ServiceMessage> CreateOrder(CreateOrderDto createOrderDto);
        Task<OrderDto> GetOrder(int id);
        Task<IEnumerable<OrderDto>> GetAllOrders();
        Task<ServiceMessage> UpdateOrder(UpdateOrderDto updateOrderDto);
        Task<ServiceMessage> DeleteOrder(int id);
    }
}
