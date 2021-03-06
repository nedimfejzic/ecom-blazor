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
        public List<Product> adminProducts { get; set; } = new List<Product>();

        public string Message { get; set; }= "Loading products...";
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchText { get; set; } = string.Empty;

        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
            return result;
        }
        public async Task GetProducts(string? categoryUrl = null)
        {

            CurrentPage = 1;
            PageCount = 0;

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

            if (products.Count == 0)
            {
                Message = "No products fouynd.";
            }

            ProductsChanged.Invoke();
        }

        public async Task SearchProducts(string searchTerm, int page =1 )
        {
            LastSearchText = searchTerm; 

            var result = await _http.GetFromJsonAsync<ServiceResponse<ProductSearchResultDTO>>($"api/product/search/{searchTerm}/{page}");
            if (result != null && result.Data != null)
            {
                products = result.Data.Products;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;

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

        public async Task GetAdminProducts()
        {
            var results = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/Product/admin");

            adminProducts = results.Data;

            CurrentPage = 1;
            PageCount = 0;

            if (adminProducts.Count == 0) Message = "No products found";


        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var result = await _http.PutAsJsonAsync($"api/product", product);
            var content = await result.Content.ReadFromJsonAsync<ServiceResponse<Product>>();
            return content.Data;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            var result = await _http.PostAsJsonAsync("api/product", product);
            var newProduct = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<Product>>()).Data;
            return newProduct;
        }

        public async Task DeleteProduct(Product product)
        {
            var result = await _http.DeleteAsync($"api/product/{product.Id}");

        }
    }
}
