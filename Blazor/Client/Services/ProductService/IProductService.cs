namespace Blazor.Client.Services.ProductService
{
    public interface IProductService
    {


        event Action ProductsChanged;
        List<Product> products { get; set; }
        List<Product> adminProducts { get; set; }

        Task GetProducts(string ? categoryUrl=null);
        Task <ServiceResponse<Product>>GetProduct(int productId);

        string Message { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        string LastSearchText { get; set; }

        Task SearchProducts(string searchTerm, int page);
        Task GetAdminProducts();
        Task <List<string>> GetSearchSuggestions(string searchTerm);

        Task<Product> CreateProduct(Product product);
        Task<Product> UpdateProduct(Product product);
        Task DeleteProduct(Product product);

    }
}
