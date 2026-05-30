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
        
        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Error al comunicar con Inventario. Status: {response.StatusCode}. Detalle: {error}");
        }

        return await response.Content.ReadAsStringAsync(); 
    }
}