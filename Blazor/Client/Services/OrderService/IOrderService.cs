namespace Blazor.Client.Services.OrderService
{
    public interface IOrderService
    {
        Task PlaceOrder();
        Task<List<OrderDetailsDTO>> GetOrders();
        Task<OrderDetailsFullDTO> GetOrder(int orderId);

    }
}
