namespace InfinityElectronics.Models.Responses;


// This would normally contain only the minimum data needed to display a product in a list or grid
public class SimpleProductResponse
{
    public string Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string CategoryId { get; set; }
    public string Image { get; set; }
    
    public CategoryResponse? Category { get; set; }
}