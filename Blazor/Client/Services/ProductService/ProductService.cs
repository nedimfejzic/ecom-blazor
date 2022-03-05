using System.Net.Http.Json;

namespace Blazor.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;
        private readonly IProductService _productService;
        public event Action ProductsChanged;

        public ProductService(HttpClient htpp)
        {
            _http = htpp; 
        }
        public List<Product> products { get; set; } = new List<Product>();
        public string Message { get; set; }= "Loading products...";
        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
            return result;
        }
        public async Task GetProducts(string? categoryUrl = null)
        {
            var result = new ServiceResponse<List<Product>>();

            if (categoryUrl!=null)
            {
                result = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}");
            }
            else
            {
                result = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product");
            }
               
                
            
            if (result!=null && result.Data != null)
            {
                products = result.Data;
            }

            ProductsChanged.Invoke();
        }

        public async Task SearchProducts(string searchTerm)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/search/{searchTerm}");
            if (result != null && result.Data != null)
            {
                products = result.Data;
            }
            if (products.Count == 0)
            {
                Message = "No products found.";
            }
            ProductsChanged.Invoke();
        }

        public async Task<List<string>> GetSearchSuggestions(string searchTerm)
        {

            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<string>>>($"api/product/searchsuggestions/{searchTerm}");
            return result.Data;

        }
    }
}
