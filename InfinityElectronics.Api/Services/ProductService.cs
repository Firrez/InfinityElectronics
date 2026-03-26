using InfinityElectronics.Data;
using InfinityElectronics.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace InfinityElectronics.Services;

public class ProductService(AppDbContext dbContext) : IProductService
{
    public async Task<DetailedProductResponse?> GetProductDetails(string productId)
    {
        try
        {
            var product = await dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .SingleOrDefaultAsync(p => p.Id == productId);

            if (product == null)
                return null;

            // For this I would use a tool like AutoMapper, but for the sake of simplicity, I'm just going to do it manually.
            var response = new DetailedProductResponse
            {
                Id = product.Id,
                Title = product.Title,
                Price = product.Price,
                Image = product.Image,
                Description = product.Description,
                CategoryId = product.CategoryId,
                Category = new CategoryResponse
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name,
                    Description = product.Category.Description
                }
            };
            
            return response;
        }
        catch (InvalidOperationException)
        {
            // Logging placeholder
            Console.WriteLine("More than one product with the same id: [" + productId + "]");

            /*
             * Returning null is also and option. It really depends on the use case.
             * As I don't see the need to show the customer that we got an error, I decided to return null.
             * Indicating that the product was not found.
             *
             * The fact that we found multiple products with the same id is an error on our side.
             * And should not be exposed to the customer.
             */

            return null;
        }
    }

    public async Task<PagedResponse<SimpleProductResponse>> GetProducts(int page, int pageSize)
    {
        var productsTask = dbContext.Products
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new SimpleProductResponse
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                Image = p.Image,
                Description = p.Description,
                CategoryId = p.CategoryId
            })
            .ToListAsync();
        
        var totalCountTask = dbContext.Products.CountAsync();

        await Task.WhenAll(productsTask, totalCountTask);
        
        return new PagedResponse<SimpleProductResponse>
        {
            Items = productsTask.Result,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCountTask.Result
        };
    }
}