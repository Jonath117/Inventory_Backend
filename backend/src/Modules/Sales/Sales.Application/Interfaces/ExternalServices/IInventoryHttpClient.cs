namespace Sales.Application.Interfaces.ExternalServices;

public interface IInventoryHttpClient
{
    Task<StockConsumeResponseDto> ConsumeStockAsync(string companyCen, StockConsumeRequestDto request);
    
    Task<ProductDetailsDto?> GetProductDetailsAsync(string companyCen, string productCen);

    Task<IEnumerable<SellableProductContractDto>> GetSellableProductsAsync(string companyCen, string? search, string? categoryCen, string? warehouseCen, bool onlyAvailable, int page, int pageSize);
}