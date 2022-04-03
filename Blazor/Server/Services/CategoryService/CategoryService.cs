using Microsoft.EntityFrameworkCore;

namespace Blazor.Server.Services.CategoryService
{

    public class CategoryService : ICategoryService
    {
        public DataContext _context { get; }

        public CategoryService(DataContext context)
        {
            _context = context;
        }


        public async Task<ServiceResponse<List<Category>>> GetCategories()
        {
            var categories = await _context.Categories
                .Where(c=> !c.Deleted && c.Visible)
                .ToListAsync();

            return new ServiceResponse<List<Category>> { Data = categories };

        }

        public async Task<ServiceResponse<List<Category>>> GetAdminCategories()
        {
             var categories = await _context.Categories
              .Where(c => !c.Deleted)
              .ToListAsync();

            return new ServiceResponse<List<Category>> { Data = categories };

        }

        public async Task<ServiceResponse<List<Category>>> AddCategory(Category category)
        {
            category.Editing = category.IsNew = false;
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return await GetAdminCategories();
        }

        public async Task<ServiceResponse<List<Category>>> UpdateCategories(Category category)
        {
           var dbCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
            if (dbCategory == null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Sucess = false,
                    Message = "Category not found"
                };
            }
            
            dbCategory.Name= category.Name;
            dbCategory.Url = category.Url;
            dbCategory.Visible = category.Visible;

            await _context.SaveChangesAsync();

            return await GetAdminCategories();


        }

        public async Task<ServiceResponse<List<Category>>> DeleteCategories(int id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return new ServiceResponse<List<Category>>
                {
                    Sucess = false,
                    Message = "Category not found"
                };
            }
            else
            {
                category.Deleted = true;
                await _context.SaveChangesAsync();
                return await GetAdminCategories();
            }
        }
    }
}
