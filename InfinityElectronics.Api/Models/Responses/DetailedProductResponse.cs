#nullable disable

namespace InfinityElectronics.Models.Responses;

public class DetailedProductResponse
{
    public string Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string CategoryId { get; set; }
    public string Image { get; set; }
    
    public CategoryResponse Category { get; set; }
}