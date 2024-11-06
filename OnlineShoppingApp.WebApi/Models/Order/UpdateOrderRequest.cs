public class UpdateOrderRequest
{
    public decimal TotalAmount { get; set; }
    public List<OrderProductRequest> Products { get; set; }
}
