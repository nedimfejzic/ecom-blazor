using Microsoft.EntityFrameworkCore;

namespace Blazor.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(DataContext Context, IHttpContextAccessor httpContextAccessor)
        {
            _context = Context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<Product>> CreateProduct(Product product)
        {
            foreach (var variant in product.Variants)
            {
                variant.ProductType = null;
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return new ServiceResponse<Product> { Data = product };
        }

        public async Task<ServiceResponse<bool>> DeleteProduct(int id)
        {
            var dbProduct = await _context.Products.FindAsync(id);
            if (dbProduct == null)
            {
                return new ServiceResponse<bool>
                {
                    Sucess = false,
                    Data = false,
                    Message = "Product not found."
                };
            }

            dbProduct.Deleted = true;

            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<List<Product>>> GetAdminProducts()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                  .Where(c => !c.Deleted)
                  .Include(_p => _p.Variants.Where(c => !c.Deleted))
                  .ThenInclude(p=>p.ProductType)
                  .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetFeaturedProducts()
        {
            var response = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                .Where(c => c.Featured == true &&  c.Visible && !c.Deleted)
                .Include(_p => _p.Variants.Where(c => c.Visible && !c.Deleted))
                .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
        {
            var response = new ServiceResponse<Product>();
            Product product = null;

            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin")){
               product = await _context.Products
             .Include(p => p.Variants.Where(c => !c.Deleted))
             .ThenInclude(v => v.ProductType)
             .FirstOrDefaultAsync(f => f.Id == productId && !f.Deleted );
            }
            else
            {
                product = await _context.Products
            .Include(p => p.Variants.Where(c => c.Visible && !c.Deleted))
            .ThenInclude(v => v.ProductType)
            .FirstOrDefaultAsync(f => f.Id == productId && !f.Deleted && f.Visible);
            }


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
                .Where(c=>c.Visible && !c.Deleted)
                .Include(_p => _p.Variants.Where(c => c.Visible && !c.Deleted))
                .ToListAsync();
            var response = new ServiceResponse<List<Product>>
            {
                Data = _data
            };

            return response;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl)
        {
            var response = new ServiceResponse<List<Product>>();

            var result = await _context.Products
                .Where(c => c.Category.Url.ToLower().Equals(categoryUrl.ToLower()) && c.Visible && !c.Deleted)
                .Include(_p => _p.Variants.Where(c => c.Visible && !c.Deleted))
                .ToListAsync();

            response.Data = result;
            return response;

        }

        public async Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchTerm)
        {
            var products = await FindProductsBySearch(searchTerm);
            List<string> result = new List<string>();

            foreach (var product in products)
            {
                if (product.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(product.Name);
                }
            }


            return new ServiceResponse<List<string>> { Data = result };
        }

        public async Task<ServiceResponse<ProductSearchResultDTO>> SearchProducts(string searchTerm, int page)
        {

            var pageResults = 2f;


            var searchResult = await FindProductsBySearch(searchTerm);
            var pageCount = Math.Ceiling(searchResult.Count / pageResults);
            var products = searchResult
                            .Skip((page - 1) * (int)pageResults)
                            .Take((int)pageResults)
                            .ToList();


            //var pageCount = Math.Ceiling((await FindProductsBySearch(searchTerm)).Count / pageResults);
            //var products = await _context.Products
            //                .Where(p => p.Name.ToLower().Contains(searchTerm.ToLower())
            //                ||
            //                p.Description.ToLower().Contains(searchTerm.ToLower()))
            //                .Include(c => c.Variants)
            //                .Skip((page - 1) * (int)pageResults)
            //                .Take((int)pageResults)
            //                .ToListAsync();


            var response = new ServiceResponse<ProductSearchResultDTO>
            {
                Data = new ProductSearchResultDTO
                {
                    Products = products,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };

            return response;

        }

        public async Task<ServiceResponse<Product>> UpdateProduct(Product product)
        {
            var dbProduct = await _context.Products.FindAsync(product.Id);
            if (dbProduct == null)
            {
                return new ServiceResponse<Product>
                {
                    Sucess = false,
                    Message = "Product not found."
                };
            }

            dbProduct.Name = product.Name;
            dbProduct.Description = product.Description;
            dbProduct.ImageUrl = product.ImageUrl;
            dbProduct.CategoryId = product.CategoryId;
            dbProduct.Visible = product.Visible;
            dbProduct.Featured = product.Featured;

            foreach (var variant in product.Variants)
            {
                var dbVariant = await _context.ProductVariants
                    .SingleOrDefaultAsync(v => v.ProductId == variant.ProductId &&
                        v.ProductTypeId == variant.ProductTypeId);
                if (dbVariant == null)
                {
                    variant.ProductType = null;
                    _context.ProductVariants.Add(variant);
                }
                else
                {
                    dbVariant.ProductTypeId = variant.ProductTypeId;
                    dbVariant.Price = variant.Price;
                    dbVariant.OriginalPrice = variant.OriginalPrice;
                    dbVariant.Visible = variant.Visible;
                    dbVariant.Deleted = variant.Deleted;
                }
            }

            await _context.SaveChangesAsync();
            return new ServiceResponse<Product> { Data = product };
        }

        private Task<List<Product>> FindProductsBySearch(string searchTerm)
        {
            return _context.Products
                            .Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()) && p.Visible && !p.Deleted)
                            .Include(c => c.Variants)
                            .ToListAsync();
        }
    }
}
