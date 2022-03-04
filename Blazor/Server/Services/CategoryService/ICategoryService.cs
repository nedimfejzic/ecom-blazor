namespace Blazor.Server.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<Category>>> GetCategories();
        //Task<ServiceResponse<Category>>GetCategoryAsync(int productId);
    }
}
