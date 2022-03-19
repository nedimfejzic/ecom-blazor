using Blazored.LocalStorage;
using System.Net.Http.Json;

namespace Blazor.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private ILocalStorageService _localStorage;
        HttpClient _httpClient;
        AuthenticationStateProvider _authStateProvider;

        public event Action OnChange;

        public CartService(ILocalStorageService localStorage, HttpClient httpClient, AuthenticationStateProvider AuthenticationStateProvider)
        {
            _localStorage = localStorage;
            _httpClient = httpClient;
            _authStateProvider = AuthenticationStateProvider;
        }

        private async Task<bool> IsUserAuthenticated()
        {
            return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }

        public async Task AddToCart(CartItem item)
        {
            if (await IsUserAuthenticated())
            {
                await _httpClient.PostAsJsonAsync("api/cart/add", item);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    cart = new List<CartItem>();
                }

                var sameItem = cart.Find(x => x.ProductId == item.ProductId && x.ProductTypeId == item.ProductTypeId);
                if (sameItem == null)
                {
                    cart.Add(item);
                }
                else
                {
                    sameItem.Quantity += item.Quantity;
                }

                await _localStorage.SetItemAsync("cart", cart);
            }

          
            await GetCartCount();


        }

        //public async Task<List<CartItem>> GetAllCartItems()
        //{
        //    await GetCartCount();
        //    var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
        //    if (cart == null)
        //    {
        //        cart = new List<CartItem>();
        //    }


        //    return cart;
        //}

        public async Task<List<CartProductResponseDTO>> GetCartProducts()
        {
            if (await IsUserAuthenticated())
            {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<CartProductResponseDTO>>>("api/cart");
                return result.Data;
            }
            else
            {
                var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cartItems == null) return new List<CartProductResponseDTO>();

                var response = await _httpClient.PostAsJsonAsync("api/cart/products", cartItems);
                var cartProcuts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponseDTO>>>();

                return cartProcuts.Data;
            }


        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            if(await IsUserAuthenticated())
            {
                await _httpClient.DeleteAsync($"api/cart/{productId}/{productTypeId}");
            }
            else
            {

                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    return;
                }

                var cartItem = cart.Find(c => c.ProductId == productId && c.ProductTypeId == productTypeId);
                if (cartItem != null)
                {
                    cart.Remove(cartItem);
                    await _localStorage.SetItemAsync("cart", cart);
                }
            }





        }

        public async Task UpdateProductQuantity(CartProductResponseDTO product)
        {
            if (await IsUserAuthenticated())
            {
                var request = new CartItem
                {
                    ProductId = product.ProductId,
                    ProductTypeId = product.ProductTypeId,
                    Quantity = product.Quantity
                };

                await _httpClient.PutAsJsonAsync("api/cart/update-quantity", request);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                if (cart == null)
                {
                    return;
                }

                var cartItem = cart.Find(c => c.ProductId == product.ProductId && c.ProductTypeId == product.ProductTypeId);
                if (cartItem != null)
                {
                    cartItem.Quantity = product.Quantity;
                    await _localStorage.SetItemAsync("cart", cart);
                }
            }

           
        }

        public async Task StoreCartItems(bool emptyLocalCart = false)
        {
            var localCart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            if (localCart == null)
            {
                return;
            }

            await _httpClient.PostAsJsonAsync("api/cart", localCart);

            if (emptyLocalCart)
            {
                await _localStorage.RemoveItemAsync("cart");
            }



        }

        public async Task GetCartCount()
        {
            if (await IsUserAuthenticated())
            {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
                var count = result.Data;

                await _localStorage.SetItemAsync<int>("cartItemsCount", count);
            }
            else
            {
                var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

                await _localStorage.SetItemAsync<int>("cartItemsCount", cart != null ? cart.Count : 0);
            }

            OnChange.Invoke();
        }
    }
}
