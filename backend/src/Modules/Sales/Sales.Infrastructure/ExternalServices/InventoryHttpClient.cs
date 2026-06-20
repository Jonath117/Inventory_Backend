using System.Net.Http.Json;
using System.Text.Json;
using Sales.Application.Interfaces.ExternalServices;

namespace Sales.Infrastructure.ExternalServices;

public class InventoryHttpClient : IInventoryHttpClient
{
    private readonly HttpClient _httpClient;

    public InventoryHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<StockConsumeResponseDto> ConsumeStockAsync(string companyCen, StockConsumeRequestDto request)
    {
        string endpoint = $"api/inventory/companies/{companyCen}/stock/consume";
            var response = await _httpClient.PostAsJsonAsync(endpoint, request);

            if (!response.IsSuccessStatusCode)
            {
                var errorObj = await response.Content.ReadFromJsonAsync<JsonElement>();
                string msg = errorObj.TryGetProperty("error", out var e) ? e.GetString() ?? "Error desconocido" : "Error de comunicación con Inventario";
                throw new InvalidOperationException(msg);
            }

            var result = await response.Content.ReadFromJsonAsync<StockConsumeResponseDto>(new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });

            return result ?? throw new InvalidOperationException("La respuesta de inventario fue nula.");
    }
    
    public async Task<ProductDetailsDto?> GetProductDetailsAsync(string companyCen, string productCen)
    {
        string endpoint = $"api/inventory/companies/{companyCen}/products/{productCen}";

        try 
        {
            var response = await _httpClient.GetAsync(endpoint);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return null;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error al obtener detalles del producto en Inventario.");
            }

            return await response.Content.ReadFromJsonAsync<ProductDetailsDto>(new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });
        }
        catch (Exception ex)
        {
            throw new Exception($"Error de conexión con Inventario: {ex.Message}");
        }
    }

    public async Task<IEnumerable<SellableProductContractDto>> GetSellableProductsAsync(string companyCen, string? search, string? categoryCen, string? warehouseCen, bool onlyAvailable, int page, int pageSize)
    {
        var queryParams = new URLSearchParams();
        if (!string.IsNullOrEmpty(search)) queryParams.Add("search", search);
        if (!string.IsNullOrEmpty(categoryCen)) queryParams.Add("categoryCen", categoryCen);
        if (!string.IsNullOrEmpty(warehouseCen)) queryParams.Add("warehouseCen", warehouseCen);
        queryParams.Add("onlyAvailable", onlyAvailable.ToString().ToLower());
        queryParams.Add("page", page.ToString());
        queryParams.Add("pageSize", pageSize.ToString());

        string endpoint = $"api/inventory/companies/{companyCen}/sellable-products?{queryParams}";

        try 
        {
            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Inventario respondió {response.StatusCode}. Detalle: {errorContent} | Endpoint intentado: {endpoint}");
            }

            var result = await response.Content.ReadFromJsonAsync<IEnumerable<SellableProductContractDto>>(new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });

            return result ?? Enumerable.Empty<SellableProductContractDto>();
        }
        catch (Exception ex)
        {
            throw new Exception($"Fallo en la comunicación HTTP: {ex.Message}");
        }
    }

    private class URLSearchParams
    {
        private readonly List<string> _params = new();
        public void Add(string key, string value) => _params.Add($"{key}={Uri.EscapeDataString(value)}");
        public override string ToString() => string.Join("&", _params);
    }
}