using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blazor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
        {
            var result = await _productService.GetProductsAsync();
            return Ok(result);
        }



        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Product>>> CreateProduct(Product product)
        {
            var result = await _productService.CreateProduct(product);
            return Ok(result);
        }

        [HttpPut, Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<Product>>> UpdateProduct(Product product)
        {
            var result = await _productService.UpdateProduct(product);
            return Ok(result);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProduct(id);
            return Ok(result);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetFeaturedProducts()
        {
            var result = await _productService.GetFeaturedProducts();
            return Ok(result);
        }


        [HttpGet("admin"), Authorize(Roles ="Admin")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetAdminProducts()
        {
            var result = await _productService.GetAdminProducts();
            return Ok(result);
        }


        [HttpGet]
        [Route("{productId}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProductById(int productId)
        {
            var result = await _productService.GetProductAsync(productId);
            return Ok(result);
        }


        [HttpGet("category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsByCategoryUrl(string categoryUrl)
        {
            var result = await _productService.GetProductsByCategory(categoryUrl);
            return Ok(result);
        }

        [HttpGet("search/{searchTerm}/{page}")]
        public async Task<ActionResult<ServiceResponse<ProductSearchResultDTO>>> SearchProducts(string searchTerm,int page = 1)
        {
            var result = await _productService.SearchProducts(searchTerm, page);
            return Ok(result);
        }

        [HttpGet("searchsuggestions/{searchTerm}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductSearchSuggestions(string searchTerm)
        {
            var result = await _productService.GetProductSearchSuggestions(searchTerm);
            return Ok(result);
        }



    }
}
