using OnlineShoppingApp.Business.Operations.Order.Dtos;
using OnlineShoppingApp.Business.Types;
using OnlineShoppingApp.Data.Entities;
using OnlineShoppingApp.Data.Repositories;
using OnlineShoppingApp.Data.UnitOfWork;

namespace OnlineShoppingApp.Business.Operations.Order
{
    public class OrderManager : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<OrderEntity> _orderRepository;
        private readonly IRepository<OrderProductEntity> _orderProductRepository;
        private readonly IRepository<ProductEntity> _productRepository;

        // Constructor to inject necessary dependencies
        public OrderManager(IUnitOfWork unitOfWork, IRepository<OrderEntity> orderRepository, IRepository<OrderProductEntity> orderProductRepository, IRepository<ProductEntity> productRepository)
        {
            _unitOfWork = unitOfWork;
            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
            _productRepository = productRepository;
        }

        // ----------------------------------------------------------------------------------------------

        // Create a new order based on the provided DTO
        public async Task<ServiceMessage> CreateOrder(CreateOrderDto createOrderDto)
        {
            // Initialize a new order entity
            var order = new OrderEntity
            {
                OrderDate = DateTime.Now,
                CustomerId = createOrderDto.CustomerId,
                TotalAmount = 0,
                OrderProducts = new List<OrderProductEntity>()
            };

            // Iterate through products in the order
            foreach (var orderProductDto in createOrderDto.Products)
            {
                // Fetch product by ID
                var product = await Task.Run(() => _productRepository.GetById(orderProductDto.ProductId));

                // Check if product exists
                if (product == null)
                    return new ServiceMessage
                    {
                        IsSuccess = false,
                        Message = $"Product with ID {orderProductDto.ProductId} not found"
                    };

                // Check if there is enough stock
                if (product.StockQuantity < orderProductDto.Quantity)
                    return new ServiceMessage
                    {
                        IsSuccess = false,
                        Message = $"Insufficient stock for product ID {orderProductDto.ProductId}"
                    };

                // Update product stock
                product.StockQuantity -= orderProductDto.Quantity;
                _productRepository.Update(product);

                // Create order product entity
                var orderProductEntity = new OrderProductEntity
                {
                    ProductId = orderProductDto.ProductId,
                    Quantity = orderProductDto.Quantity,
                    Price = product.Price
                };

                // Calculate total amount and add to order products
                order.TotalAmount += orderProductEntity.TotalPrice;
                order.OrderProducts.Add(orderProductEntity);
            }

            // Begin transaction for order creation
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _orderRepository.AddAsync(order); 
                await _unitOfWork.CommitTransaction();
                return new ServiceMessage
                {
                    IsSuccess = true,
                    Message = "Order created successfully"
                };
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return new ServiceMessage
                {
                    IsSuccess = false,
                    Message = "Failed to create order"
                };
            }
        }

        // ----------------------------------------------------------------------------------------------

        // Get a specific order by ID
        public async Task<OrderDto> GetOrder(int id)
        {
            // Validate the provided ID
            if (id <= 0)
            {
                throw new ArgumentException("Invalid order ID.");
            }

            // Fetch the order by ID
            var order = await Task.Run(() => _orderRepository.GetById(id));

            // Return null if order is not found
            if (order == null)
            {
                return null;
            }
            if (order.OrderProducts == null)
            {
                order.OrderProducts = new List<OrderProductEntity>();
            }
            var orderDto = new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Products = new List<OrderProductDto>()
            };
            foreach (var orderProduct in order.OrderProducts)
            {
                orderDto.Products.Add(new OrderProductDto
                {
                    ProductId = orderProduct.ProductId,
                    Quantity = orderProduct.Quantity
                });
            }
            return orderDto;
        }

        // ----------------------------------------------------------------------------------------------

        // Get all orders
        public async Task<IEnumerable<OrderDto>> GetAllOrders()
        {
            var orders = await Task.Run(() => _orderRepository.GetAll());
            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                var orderDto = new OrderDto
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Products = new List<OrderProductDto>()
                };
                if (order.OrderProducts != null)
                {
                    foreach (var orderProduct in order.OrderProducts)
                    {
                        orderDto.Products.Add(new OrderProductDto
                        {
                            ProductId = orderProduct.ProductId,
                            Quantity = orderProduct.Quantity
                        });
                    }
                }
                orderDtos.Add(orderDto);
            }
            return orderDtos;
        }

        // ----------------------------------------------------------------------------------------------

        // Update an existing order based on the provided DTO
        public async Task<ServiceMessage> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            // Find the order by its ID
            var order = await _orderRepository.GetByIdAsync(updateOrderDto.Id);

            if (order == null)
            {
                return new ServiceMessage
                {
                    IsSuccess = false,
                    Message = "Order not found"
                };
            }

            // Begin transaction for order update
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // Update order details
                order.OrderDate = updateOrderDto.OrderDate;
                order.TotalAmount = 0;
                order.OrderProducts.Clear();

                // Loop through the products in the DTO
                foreach (var productDto in updateOrderDto.Products)
                {
                    // Get the product entity by its ID
                    var productEntity = await _productRepository.GetByIdAsync(productDto.ProductId);

                    if (productEntity == null)
                    {
                        await _unitOfWork.RollbackTransaction();
                        return new ServiceMessage
                        {
                            IsSuccess = false,
                            Message = $"Product with ID {productDto.ProductId} not found"
                        };
                    }

                    // Check if there is enough stock
                    if (productEntity.StockQuantity < productDto.Quantity)
                    {
                        await _unitOfWork.RollbackTransaction();
                        return new ServiceMessage
                        {
                            IsSuccess = false,
                            Message = $"Insufficient stock for product ID {productDto.ProductId}"
                        };
                    }

                    // Update product stock
                    productEntity.StockQuantity -= productDto.Quantity;
                    _productRepository.Update(productEntity);

                    // Create order product entity
                    var orderProductEntity = new OrderProductEntity
                    {
                        ProductId = productDto.ProductId,
                        Quantity = productDto.Quantity,
                        Price = productEntity.Price,
                        ModifiedDate = DateTime.Now
                    };

                    // Calculate total amount and add to order products
                    order.TotalAmount += orderProductEntity.Quantity * orderProductEntity.Price;
                    order.OrderProducts.Add(orderProductEntity);
                }

                // Update the order in the repository
                _orderRepository.Update(order);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();

                return new ServiceMessage
                {
                    IsSuccess = true,
                    Message = "Order updated successfully"
                };
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return new ServiceMessage
                {
                    IsSuccess = false,
                    Message = "Failed to update order"
                };
            }
        }

        // ----------------------------------------------------------------------------------------------

        // Delete an order by ID
        public async Task<ServiceMessage> DeleteOrder(int id)
        {
            // Fetch the order to be deleted
            var order = await Task.Run(() => _orderRepository.GetById(id));

            // Return error if order not found
            if (order == null)
                return new ServiceMessage
                {
                    IsSuccess = false,
                    Message = "Order not found"
                };

            // Begin transaction for order deletion
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                _orderRepository.Delete(order);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
                return new ServiceMessage
                {
                    IsSuccess = true,
                    Message = "Order deleted successfully"
                };
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return new ServiceMessage
                {
                    IsSuccess = false,
                    Message = "Failed to delete order"
                };
            }
        }

        // ----------------------------------------------------------------------------------------------
    }
}
