using OnlineShoppingApp.Business.Operations.Order.Dtos;

namespace OnlineShoppingApp.Business.Operations.Order.Dtos
{
    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
        public List<OrderProductDto> Products { get; set; }
    }
}

