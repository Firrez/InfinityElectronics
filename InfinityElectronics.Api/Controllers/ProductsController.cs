using InfinityElectronics.Filters;
using InfinityElectronics.Services;
using Microsoft.AspNetCore.Mvc;

namespace InfinityElectronics.Controllers;

[ApiKey]
[ApiController]
[Route("api/v1/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet("{productId}", Name = "GetProductDetails")]
    public async Task<IActionResult> GetProductDetails(string productId)
    {
        var product = await productService.GetProductDetails(productId);
        return product != null ? Ok(product) : NotFound();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page < 1) return BadRequest("Page must be greater than 0");
        if (pageSize is < 1 or > 50) return BadRequest("PageSize must be between 1 and 50");
        
        var products = await productService.GetProducts(page, pageSize);
        return Ok(products);
    }
}