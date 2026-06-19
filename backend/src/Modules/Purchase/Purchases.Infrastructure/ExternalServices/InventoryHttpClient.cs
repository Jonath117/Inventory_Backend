using System.Net.Http.Json;
using Purchases.Application.Interfaces.ExternalServices;

namespace Purchases.Infrastructure.ExternalServices;

public class InventoryHttpClient : IInventoryHttpClient
{
    private readonly HttpClient _httpClient;

    public InventoryHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> IncreaseStockAsync(string companyCen, StockIncreaseContractRequest request)
    {
        string endpoint = $"api/inventory/companies/{companyCen}/stock/increase";
        
        var response = await _httpClient.PostAsJsonAsync(endpoint, request);
        
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new KeyNotFoundException("No se pudo aumentar el stock: El producto o el almacén especificado no existen en el sistema de Inventario.");
        }
        
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync(); 
    }
}