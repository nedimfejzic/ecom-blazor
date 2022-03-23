namespace Blazor.Server.Services.OrderService
{
    public interface IOrderService
    {
        Task<ServiceResponse<bool>> PlaceOrder();
        Task<ServiceResponse<List<OrderDetailsDTO>>> GetOrders();
        Task<ServiceResponse<OrderDetailsFullDTO>> GetOrderDetails(int orderId);

    }
}
