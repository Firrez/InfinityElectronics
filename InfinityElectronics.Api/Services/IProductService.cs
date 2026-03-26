using InfinityElectronics.Models.Responses;

namespace InfinityElectronics.Services;

public interface IProductService
{
    Task<DetailedProductResponse?> GetProductDetails(string productId);
    Task<PagedResponse<SimpleProductResponse>> GetProducts(int page = 1, int pageSize = 10);
}