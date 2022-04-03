using System.Net.Http.Json;

namespace Blazor.Client.Services.ProductTypeService
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly HttpClient _http;

        public List<ProductType> ProductTypes { get; set; } = new List<ProductType>();
        public event Action OnChange;

        public ProductTypeService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        public async Task GetProductTypes()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<ProductType>>>("api/ProductTypes");

            ProductTypes = result.Data;
        }

        public async Task AddProductType(ProductType productType)
        {
            var response = await _http.PostAsJsonAsync("api/ProductTypes", productType);
            ProductTypes = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
            OnChange.Invoke();
        }

        public async Task UpdateProductType(ProductType productType)
        {
            var response = await _http.PutAsJsonAsync("api/ProductTypes", productType);
            ProductTypes = (await response.Content
                .ReadFromJsonAsync<ServiceResponse<List<ProductType>>>()).Data;
            OnChange.Invoke();
        }

        public ProductType CreateNewProductType()
        {
            var newProductType = new ProductType { IsNew = true, Editing = true };

            ProductTypes.Add(newProductType);
            OnChange.Invoke();
            return newProductType;
        }
    }
}
