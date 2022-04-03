namespace Blazor.Client.Services.CategoryService
{
    public interface ICategoryService
    {
        event Action CategoryChanged;

        List<Category> Categories { get; set; }
        List<Category> AdminCategories { get; set; }
        Task GetAllCategories();
        Task GetAdminCategories();
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(int id);

        Category CreateNewCategory();

    }
}
