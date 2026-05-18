namespace Purchases.Application.Interfaces.ExternalServices;

public record StockValidationItemContractDto(string ProductCen, decimal Quantity);

public record StockIncreaseContractRequest(
    string WarehouseCen, 
    string Source, 
    string ReferenceCen, 
    string? Reason, 
    List<StockValidationItemContractDto> Items
);

public interface IInventoryHttpClient
{
    Task<string> IncreaseStockAsync(string companyCen, StockIncreaseContractRequest request);
}