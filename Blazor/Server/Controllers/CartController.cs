using Blazor.Server.Services.CartService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("products")]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponseDTO>>>> GetCartProduct([FromBody] List<CartItem> cartItems)
        {
            var results = await _cartService.GetCartProducts(cartItems);
            return Ok(results);
        }




        [HttpPost("add")]
        public async Task<ActionResult<ServiceResponse<bool>>> AddToCart(CartItem cartItem)
        {
            var result = await _cartService.AddToCard(cartItem);
            return Ok(result);
        }


        [HttpPut("update-quantity")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateQuantity(CartItem cartItem)
        {
            var result = await _cartService.UpdateQuantity(cartItem);
            return Ok(result);
        }


        [HttpDelete("{productId}/{productTypeId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteItem(int productId, int productTypeId)
        {
            var result = await _cartService.RemoveItemFromCart(productId, productTypeId);
            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponseDTO>>>> StoreCartItems([FromBody] List<CartItem> cartItems)
        {

            var results = await _cartService.StoreCartItems(cartItems);
            return Ok(results);
        }

        [HttpGet("count")]
        public async Task<ActionResult<ServiceResponse<int>>> GetCartCount()
        {
            return await _cartService.GetCartCount();
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<CartProductResponseDTO>>>>GetProductsFromDb()
        {
            var result = await _cartService.GetCartProductsFromDB();
            return Ok(result);
        }
    }
}
