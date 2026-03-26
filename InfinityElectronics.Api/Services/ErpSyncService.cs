using System.Text.Json.Serialization;
using InfinityElectronics.Data;
using InfinityElectronics.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace InfinityElectronics.Services;

public class ErpSyncService(AppDbContext dbContext, HttpClient httpClient)
{
    // These should be placed in user secrets or environment variables, this is just for demo purposes
    private const string CategoriesUrl = "https://ncrecruitmentpubweu.blob.core.windows.net/cases/ecom/categories-sample-v1.json";
    private const string ProductsUrl = "https://ncrecruitmentpubweu.blob.core.windows.net/cases/ecom/products-sample-v1.json";

    public async Task SyncAsync()
    {
        await SyncCategories();
        await SyncProducts();
    }
    
    private async Task SyncCategories()
    {
        var categories = await httpClient.GetFromJsonAsync<List<Category>>(CategoriesUrl);
        if (categories == null) return;

        foreach (var category in categories)
        {
            var existing = await dbContext.Categories.FindAsync(category.Id);
            if (existing is null)
                await dbContext.Categories.AddAsync(category);
            else
            {
                existing.Name = category.Name;
                existing.Description = category.Description;
            }
        }

        await dbContext.SaveChangesAsync();
    }

    private async Task SyncProducts()
    {
        var products = await httpClient.GetFromJsonAsync<List<ErpProduct>>(ProductsUrl);
        if (products == null) return;
        
        var categoryIds = await dbContext.Categories
            .Select(c => c.Id)
            .ToListAsync();

        foreach (var erpProduct in products)
        {
            if (!categoryIds.Contains(erpProduct.Category))
            {
                Console.WriteLine($"Product {erpProduct.Id} has unknown category {erpProduct.Category}, skipping");
                continue;
            }
            
            var existing = await dbContext.Products.FindAsync(erpProduct.Id);
            if (existing is null)
                await dbContext.Products.AddAsync(new Product
                {
                    Id = erpProduct.Id,
                    Title = erpProduct.Title,
                    Price = erpProduct.Price,
                    Description = erpProduct.Description,
                    CategoryId = erpProduct.Category,
                    Image = erpProduct.Image
                });
            else
            {
                existing.Title = erpProduct.Title;
                existing.Price = erpProduct.Price;
                existing.Description = erpProduct.Description;
                existing.CategoryId = erpProduct.Category;
                existing.Image = erpProduct.Image;
            }
        }

        await dbContext.SaveChangesAsync();
    }
    
    internal class ErpProduct
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        [JsonPropertyName("category")]
        public string Category { get; set; }
        public string Image { get; set; }
    }
}