using System.ComponentModel.DataAnnotations;

public class CreateOrderRequest
{
    public List<OrderProductRequest> Products { get; set; }
}
