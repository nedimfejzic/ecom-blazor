using Microsoft.EntityFrameworkCore;

namespace Blazor.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext Context)
        {
            _context = Context;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var response = new ServiceResponse<Product>();

            var product = await _context.Products
                .Include(p=>p.Variants)
                .ThenInclude(v=>v.ProductType)
                .FirstOrDefaultAsync(f=>f.Id == productId); 


            if (product == null)
            {
                response.Sucess = false;
                response.Message = "Product is not found";
            }
            else
            {
                response.Data = product;
            }

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            var _data = await _context.Products
                .Include(_p => _p.Variants)
                .ToListAsync();
            var response = new ServiceResponse<List<Product>> {
                Data = _data
            };

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>();

            var result = await _context.Products
                .Where(c=>c.Category.Url.ToLower().Equals(categoryUrl.ToLower()))
                .Include(_p => _p.Variants)
                .ToListAsync();

            response.Data = result; 
            return response;

        }
    }
}
