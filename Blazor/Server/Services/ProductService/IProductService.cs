namespace Blazor.Server.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetProductsAsync();
        Task<ServiceResponse<Product>> GetProductAsync(int productId);
        Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl);
        Task<ServiceResponse<ProductSearchResultDTO>> SearchProducts(string searchTerm, int page);
        Task<ServiceResponse<List<Product>>> GetFeaturedProducts();
        Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchTerm);
        Task<ServiceResponse<List<Product>>> GetAdminProducts();
        Task<ServiceResponse<Product>> CreateProduct(Product product);
        Task<ServiceResponse<Product>> UpdateProduct(Product product);
        Task<ServiceResponse<bool>> DeleteProduct(int id);
    }
}
