using Blazor.Server.Services.AuthService;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blazor.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;
        private readonly ICartService _cartService;

        public OrderService(DataContext dataContext, IAuthService authService, ICartService cartService)
        {
            _context = dataContext;
            _authService = authService;
            _cartService = cartService;
        }

        public async Task<ServiceResponse<OrderDetailsFullDTO>> GetOrderDetails(int orderId)
        {
                var response = new ServiceResponse<OrderDetailsFullDTO>();
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ProductType)
                .Where(o => o.UserId == _authService.GetUserId() && o.Id == orderId)
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                response.Sucess = false;
                response.Message = "Order not found";

                return response;
            }


            var orderDTO = new OrderDetailsFullDTO
            {
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                Products = new List<OrderDetailsProductDTO>()
            };


            order.OrderItems.ForEach(o =>
                orderDTO.Products.Add(
                        new OrderDetailsProductDTO()
                        {
                            ProductId = o.ProductId,
                            ImageUrl = o.Product.ImageUrl,
                            ProductType = o.ProductType.Name,
                            Quantity = o.Quantity,
                            Title = o.Product.Name,
                            TotalPrice = o.TotalPrice,
                        }
                    )
                );

            response.Data = orderDTO;
            return response;



        }

        public async Task<ServiceResponse<List<OrderDetailsDTO>>> GetOrders()
        {
            var response = new ServiceResponse<List<OrderDetailsDTO>>();
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(u => u.UserId == _authService.GetUserId())
                .OrderBy(o => o.OrderDate)
                .ToListAsync();

            var orderResponse = new List<OrderDetailsDTO>();
            orders.ForEach(o=> orderResponse.Add(new OrderDetailsDTO
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Total   = o.TotalPrice,
                Product = o.OrderItems.Count>1?
                $"{o.OrderItems.First().Product.Name} and" +
                $"{o.OrderItems.Count - 1} more...": o.OrderItems.First().Product.Name,
                ProductImageUrl = o.OrderItems.First().Product.ImageUrl

            }));

            response.Data = orderResponse;
            return response;


        }

        public async Task<ServiceResponse<bool>> PlaceOrder()
        {
            var cart = (await _cartService.GetCartProductsFromDB()).Data;
            if(cart == null) return new ServiceResponse<bool> { Data = false };


            decimal totalPrice = 0;

            cart.ForEach(x => totalPrice += x.Price * x.Quantity);

            var orderItems = new List<OrderItem>();
            foreach (var item in cart)
            {
                orderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ProductTypeId = item.ProductTypeId,
                    TotalPrice = item.Price * item.Quantity
                });
            }


            var order = new Order {
                TotalPrice = totalPrice,
                OrderItems = orderItems,
                OrderDate = DateTime.Now,
                UserId = _authService.GetUserId(),
            };


            _context.Orders.Add(order);
            // obrisati sve iz korpe
            _context.CartItems.RemoveRange(_context.CartItems.Where(ci=>ci.UserId == _authService.GetUserId()));

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };

        }
    }
}
