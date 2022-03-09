namespace Blazor.Client.Services.CartService
{
    public interface ICartService
    {
        event Action OnChange;
        Task AddToCart(CartItem item);
        Task <List<CartItem>> GetAllCartItems();
    }
}
