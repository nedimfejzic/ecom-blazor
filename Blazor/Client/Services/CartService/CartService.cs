using Blazored.LocalStorage;

namespace Blazor.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private ILocalStorageService _localStorage;

        public event Action OnChange;

        public CartService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task AddToCart(CartItem item)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            cart.Add(item); 
            await _localStorage.SetItemAsync("cart",cart);
            OnChange.Invoke();

        }

        public async Task<List<CartItem>> GetAllCartItems()
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (cart == null)
            {
                cart = new List<CartItem>();
            }


            return cart;
        }
    }
}
