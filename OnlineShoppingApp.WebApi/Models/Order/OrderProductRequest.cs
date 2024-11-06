using OnlineShoppingApp.Business.Operations.Order.Dtos;
using System.ComponentModel.DataAnnotations;

public class OrderProductRequest
{
    [Required]
    public int ProductId { get; set; }
    [Required]
    public int Quantity { get; set; }
}
