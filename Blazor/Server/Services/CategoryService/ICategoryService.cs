﻿namespace Blazor.Server.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<Category>>> GetCategories();
        //Task<ServiceResponse<Category>>GetCategoryAsync(int productId);
        Task<ServiceResponse<List<Category>>> GetAdminCategories();
        Task<ServiceResponse<List<Category>>> AddCategory(Category category);
        Task<ServiceResponse<List<Category>>> UpdateCategories(Category category);
        Task<ServiceResponse<List<Category>>> DeleteCategories(int id);



    }
}
