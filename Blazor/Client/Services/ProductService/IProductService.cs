namespace Blazor.Client.Services.ProductService
{
    public interface IProductService
    {


        event Action ProductsChanged;
        List<Product> products { get; set; }
        Task GetProducts(string ? categoryUrl=null);
        Task <ServiceResponse<Product>>GetProduct(int productId);

        string Message { get; set; }
        Task SearchProducts(string searchTerm);
        Task <List<string>> GetSearchSuggestions(string searchTerm);

    }
}
