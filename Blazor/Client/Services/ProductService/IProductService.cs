namespace Blazor.Client.Services.ProductService
{
    public interface IProductService
    {


        event Action ProductsChanged;
        List<Product> products { get; set; }
        Task GetProducts(string ? categoryUrl=null);
        Task <ServiceResponse<Product>>GetProduct(int productId);

        string Message { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        string LastSearchText { get; set; }

        Task SearchProducts(string searchTerm, int page);
        Task <List<string>> GetSearchSuggestions(string searchTerm);

    }
}
