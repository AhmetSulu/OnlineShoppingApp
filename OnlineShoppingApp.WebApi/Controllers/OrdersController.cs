using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingApp.Business.Operations.Order;
using OnlineShoppingApp.Business.Operations.Order.Dtos;
using OnlineShoppingApp.WebApi.Jwt;

namespace OnlineShoppingApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        // Constructor to inject the order service
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // ----------------------------------------------------------------------------------------------

        // Endpoint for creating a new order, accessible only to Admin users
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {
            // Extract user ID from JWT token
            var userIdClaim = User.FindFirst(JwtClaimNames.Id); // Assuming the claim type is "id"
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }
            var userId = int.Parse(userIdClaim.Value);

            // Create DTO for the new order
            var createOrderDto = new CreateOrderDto
            {
                CustomerId = userId, // Use the extracted user ID            
                Products = request.Products.Select(p => new OrderProductDto
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity
                }).ToList()
            };

            // Attempt to create the order
            var result = await _orderService.CreateOrder(createOrderDto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            else
                return Ok(new { Message = "Order created successfully!" });
        }

        // ----------------------------------------------------------------------------------------------

        // Endpoint for retrieving a specific order by ID, accessible only to Admin users
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}/GetOrder")]
        public async Task<IActionResult> GetOrder(int id)
        {
            // Validate the order ID
            if (id <= 0)
            {
                return BadRequest("Invalid order ID.");
            }
            if (_orderService == null)
                return StatusCode(500, "Order service is not available.");

            // Attempt to retrieve the order
            var order = await _orderService.GetOrder(id);

            // Check if the order exists
            if (order == null)
                return NotFound("Order not found.");

            return Ok(order);
        }

        // ----------------------------------------------------------------------------------------------

        // Endpoint for retrieving all orders, accessible only to Admin users
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllOrders")]
        public async Task<IActionResult> GetAllOrders()
        {
            // Retrieve all orders
            var orders = await _orderService.GetAllOrders();
            return Ok(orders);
        }

        // ----------------------------------------------------------------------------------------------

        // Endpoint for deleting an order by ID, accessible only to Admin users
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}/DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            // Attempt to delete the order
            var result = await _orderService.DeleteOrder(id);
            if (!result.IsSuccess)
                return NotFound(result.Message);
            else
                return Ok(new { Message = "Order deleted successfully!" });
        }

        // ----------------------------------------------------------------------------------------------

        // Endpoint for updating an existing order by ID, accessible only to Admin users
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(int id, UpdateOrderRequest request)
        {
            // Create DTO for updating the order
            var updateOrderDto = new UpdateOrderDto
            {
                Id = id,
                OrderDate = DateTime.Now,
                TotalAmount = request.TotalAmount,
                Products = request.Products.Select(p => new OrderProductDto
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity
                }).ToList()
            };

            // Attempt to update the order
            var result = await _orderService.UpdateOrder(updateOrderDto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);
            else
                return Ok(new { Message = "Order updated successfully!" });
        }

        // ----------------------------------------------------------------------------------------------

    }
}
