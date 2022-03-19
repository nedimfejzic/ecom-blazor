using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blazor.Server.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _contextAccessor;


        public CartService(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _context = dataContext;
            _contextAccessor = httpContextAccessor;
        }
        public async Task<ServiceResponse<List<CartProductResponseDTO>>> GetCartProducts(List<CartItem> cartItems)
        {
            var result = new ServiceResponse<List<CartProductResponseDTO>>
            {
                Data = new List<CartProductResponseDTO>()

            };

            foreach (var item in cartItems)
            {
                var product = await _context.Products
                    .Where(c => c.Id == item.ProductId)
                    .FirstOrDefaultAsync();

                if (product == null)
                {
                    continue;
                }

                var productVariant = await _context.ProductVariants
                    .Where(c => c.ProductId == item.ProductId && c.ProductTypeId == item.ProductTypeId)
                    .Include(v => v.ProductType)
                    .FirstOrDefaultAsync();

                if (productVariant == null)
                {
                    continue;
                }

                var cartProduct = new CartProductResponseDTO
                {
                    ProductId = product.Id,
                    Title = product.Name,
                    ImageUrl = product.ImageUrl,
                    Price = productVariant.Price,
                    ProductTypeId = productVariant.ProductTypeId,
                    ProductType = productVariant.ProductType.Name,
                    Quantity = item.Quantity
                };

                result.Data.Add(cartProduct);
            }

            return result;
        }

        private int GetUserId()=>int.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<List<CartProductResponseDTO>>> StoreCartItems(List<CartItem> cartItems)
        {
    
          var userId = GetUserId();

          cartItems.ForEach(x=>x.UserId = userId);
            _context.CartItems.AddRange(cartItems);
            await _context.SaveChangesAsync();

            return await GetCartProductsFromDB();
        }

        public async Task<ServiceResponse<int>> GetCartCount()
        {
            var count = (await _context.CartItems.Where(c=>c.UserId== GetUserId()).ToListAsync()).Count();

            return new ServiceResponse<int>
            {
                Data = count,
            };
        }

        public async Task<ServiceResponse<List<CartProductResponseDTO>>> GetCartProductsFromDB()
        {
            return await GetCartProducts(await _context.CartItems.Where(c => c.UserId == GetUserId()).ToListAsync());
        }

        public async Task<ServiceResponse<bool>> AddToCard(CartItem cartItem)
        {
           cartItem.UserId = GetUserId();
            var sameItem = await _context.CartItems
                .FirstOrDefaultAsync(c =>
                c.ProductId == cartItem.ProductId
                && c.ProductTypeId == cartItem.ProductTypeId
                && c.UserId == cartItem.UserId);


            if (sameItem == null)
            {
                _context.CartItems.Add(cartItem);
            }
            else
            {
                sameItem.Quantity += cartItem.Quantity;
            }

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem)
        {
            cartItem.UserId = GetUserId();
            var dbItem = await _context.CartItems
                .FirstOrDefaultAsync(c =>
                c.ProductId == cartItem.ProductId
                && c.ProductTypeId == cartItem.ProductTypeId
                && c.UserId == GetUserId());


            if (dbItem == null)
            {
                return new ServiceResponse<bool> { Data = false, Message = "Cart item does not exist.", Sucess = false};
            }

            dbItem.Quantity = cartItem.Quantity;

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<bool>> RemoveItemFromCart(int productId, int productTypeId)
        {
            var dbItem = await _context.CartItems
                .FirstOrDefaultAsync(c =>
                c.ProductId == productId
                && c.ProductTypeId == productTypeId
                && c.UserId == GetUserId());


            if (dbItem == null)
            {
                return new ServiceResponse<bool> { Data = false, Message = "Cart item does not exist.", Sucess = false };
            }

            _context.CartItems.Remove(dbItem);

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }
    }
}
