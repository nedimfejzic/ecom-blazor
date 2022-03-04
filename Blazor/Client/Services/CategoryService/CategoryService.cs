﻿using System.Net.Http.Json;

namespace Blazor.Client.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _http;

        public CategoryService(HttpClient http)
        {
            _http = http;
        }
        public List<Category> Categories { get; set; } = new List<Category>();
        public async Task GetAllCategories()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/Category");

            if (response != null && response.Data != null)
            {
                Categories =response.Data;  
            }
        }
    }
}
