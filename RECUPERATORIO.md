## 3. El documento `RECUPERATORIO.md`

Este archivo va en la raíz del repo de Ventas. Tiene tres secciones.

### 3.1 — Estructura del proyecto

Mostrá el árbol de carpetas de tu módulo de Ventas (podés usar `tree` o armarlo a mano):

```
Sales/
├── Sales.Api/
│   ├── Controllers/
│   ├── Middlewares/
│   └── Properties/
├── Sales.Application/
│   ├── Features/
│   │   ├── Catalog/
│   │   ├── Dashboard/
│   │   │   ├── GetKdsStatus/
│   │   │   └── GetTopProducts/
│   │   ├── Kds/
│   │   │   └── UpdateKdsItemStatus/
│   │   ├── PaymentMethods/
│   │   ├── TaxConfiguration/
│   │   │   └── UpdateTaxConfiguration/
│   │   ├── Tickets/
│   │   │   ├── AddItemToTicket/
│   │   │   ├── AssignWaiter/
│   │   │   ├── CancelTicket/
│   │   │   ├── CreateTicket/
│   │   │   ├── GetDailyTickets/
│   │   │   ├── GetTicketByCen/
│   │   │   ├── GetTicketItems/
│   │   │   ├── GetTicketTotals/
│   │   │   ├── PayTicket/
│   │   │   ├── SendToKds/
│   │   │   └── UpdateTicketItem/
│   │   └── Waiters/
│   └── Interfaces/
│       └── ExternalServices/
├── Sales.Domain/
│   ├── Entities/
│   ├── Enums/
│   └── Exceptions/
└── Sales.Infrastructure/
    ├── ExternalServices/
    ├── Migrations/
    └── Persistence/
        ├── Configurations/
        └── Repositories/

```

Luego explicá, en 3–5 oraciones, cómo fluye una venta dentro de tu sistema: desde que llega el request hasta que se guarda en la base de datos y se llama al Inventario.

### 3.2 — Integración con Inventario

Respondé estas preguntas con referencia directa a tu código:

**3.2.1** ¿Desde qué clase/método de tu código hacés la llamada HTTP al Inventario del compañero? Pegá el snippet relevante.  
Lo hago desde la clase encargada de realizar las llamadas HTTP al modulo de Inventario que es InventoryHttpClient, ubicada en la infraestructura del modulo de Ventas(Sales).

```csharp
// Archivo: src/Modules/Sales/Sales.Infrastructure/ExternalServices/InventoryHttpClient.cs
//cabe recalcar que estos comentarios no estan el codigo 
public class InventoryHttpClient : IInventoryHttpClient
{
    private readonly HttpClient _httpClient;

    public InventoryHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Llamada para descontar stock al pagar
    public async Task<StockConsumeResponseDto> ConsumeStockAsync(string companyCen, StockConsumeRequestDto request)
    {
        string endpoint = \$"api/inventory/companies/{companyCen}/stock/consume";
        var response = await _httpClient.PostAsJsonAsync(endpoint, request);

    }

    // Llamada para obtener catálogo de productos vendibles
    public async Task<IEnumerable<SellableProductContractDto>> GetSellableProductsAsync(string companyCen, string? search, string? categoryCen, string? warehouseCen, bool onlyAvailable, int page, int pageSize)
    {
        string endpoint = \$"api/inventory/companies/{companyCen}/sellable-products?{queryParams}";
        var response = await _httpClient.GetAsync(endpoint);

    }

    // Llamada para validar un producto individual antes de agregarlo al ticket
    public async Task<ProductDetailsDto?> GetProductDetailsAsync(string companyCen, string productCen)
    {
        string endpoint = \$"api/inventory/companies/{companyCen}/products/{productCen}";
        var response = await _httpClient.GetAsync(endpoint);
    }
}
```
   * Se inyecta un HttpClient pre-configurado (via IHttpClientFactory).


**3.2.2** ¿Cómo está configurada la URL del Inventario en tu proyecto? ¿Dónde se define? Pegá el fragmento del `.env.example` y del código que lo lee.

Esta configurado como variable de entornos, en un .env que parte de un .env.example. Se define en el archivo de configuracion global .env ubicado en la raiz del directorio backend/

```
INVENTORY_API_URL=http://localhost:TU_URL
SALES_API_URL=http://localhost:TU_URL
SHARED_API_URL=http://localhost:TU_URL
ConnectionStrings__DefaultConnection=Host=localhost;Database=SalesDb;Username=postgres;Password=tu_password
```

EL archivo es cargado al inicio de la aplicacion en el program.cs, mediante una libreria, luego el modulo Sales lo inyecta dinamicamente en su capa de infraestructura para configurar el cliente HTTP.
El codigo que lo lee es el siguiente:

```csharp
// Archivo: src/Modules/Sales/Sales.Infrastructure/DependencyInjection.cs

public static IServiceCollection AddSalesInfrastructure(this IServiceCollection services, IConfiguration configuration)
{

    services.AddScoped<ISalesRepository, SalesRepository>();
    
    // Inyeccion de la URL de Inventario desde el archivo .env al HttpClient
    services.AddHttpClient<IInventoryHttpClient, InventoryHttpClient>(client =>
    {
        var inventoryUrl = configuration["INVENTORY_API_URL"];
        
        client.BaseAddress = new Uri(inventoryUrl); 
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    });
    
    return services;
}
```

**3.2.3** ¿Qué pasa en tu sistema si el Inventario devuelve un error 404 (producto no encontrado)? ¿Y si devuelve 500? Describí el comportamiento actual y pegá el código que lo maneja.  
Yo tengo centralizado el manejo de errores externos en un middlware en los .API mediante un GlobalExceptionHandler. Asi la capa de Application no mezcla logica de negocio con manejo de respuestas HTTP crudas.

Si el Inventario devuelve un 404 (Producto no encontrado):
La clase InventoryHttpClient evalua el codigo de estado de la respuesta. Al detectar un 404 lanza una excepcion nativa del dominio (KeyNotFoundException). Esta excepcion interrumpe el flujo atraviesa el Controller de manera transparente y es interceptada por mi GlobalExceptionHandler, el cual la traduce en una respuesta estandar ProblemDetails con un status HTTP 404 hacia el cliente final (el frontend).

Si el Inventario devuelve un 500 (ya sea por un error interno de mi compañero) o no responde:
El cliente HTTP lanza una HttpRequestException. El GlobalExceptionHandler la intercepta y, por regla de negocio, la traduce a un 503 Service Unavailable. Esto es muuy importante porque un 500 indicaria un error en mi propio codigo de Ventas, mientras que el 503 informa correctamente que el fallo proviene de un servicio de terceros temporalmente indisponible.

```csharp
public async Task<StockConsumeContractResponse> ConsumeStockAsync(string companyCen, StockConsumeRequestDto request)
{
    var response = await _httpClient.PostAsJsonAsync($"/api/inventory/companies/{companyCen}/stock/consume", request);

    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
    {
        throw new KeyNotFoundException("El producto o la bodega no existen en el sistema de Inventario.");
    }
    response.EnsureSuccessStatusCode(); 

    return (await response.Content.ReadFromJsonAsync<StockConsumeContractResponse>())!;
}
```

### 3.3 — Preguntas teóricas

Respondé las siguientes preguntas. Extensión sugerida: 1–2 párrafos por pregunta. **Hacé referencia explícita a tu código o a decisiones que tomaste en el proyecto.**

**Pregunta A — Cambio en el contrato**
Tu compañero te avisa que va a renombrar el campo `"cantidad"` por `"qty"` en la respuesta del endpoint de stock. Tu sistema ya consume ese endpoint.
¿Qué riesgos genera ese cambio? ¿Qué prácticas usarías para que ese cambio no rompa tu sistema?  
Genera un riesgo de falla por deserializacion nula (el stock pareceria ser 0). Una practica para evitarlo seria: usar atributos de mapeo explicitos en los DTOs (como [JsonPropertyName("qty")]) sin cambiar el modelo de dominio interno.

**Pregunta B — Red caída a mitad de una transacción**
Tu sistema de Ventas llama al Inventario para descontar stock. La red se cae justo después de que Inventario procesó el descuento, pero antes de que la respuesta llegue a Ventas.
¿Qué problema se genera? ¿Cómo lo detectarías? ¿Cómo lo manejarías?  
Genera una inconsistencia (Inventario desconto, Ventas no registro el cobro). Se detecta comparando logs o reportes donde se cuadra a diario. Se maneja implementando el patron Outbox, Sagas o teniendo un mecanismo de reintentos y compensacion (transacciones distribuidas).

**Pregunta C — Inventario caído**
Si el Inventario del compañero está caído completamente, ¿debería tu módulo de Ventas seguir permitiendo registrar ventas o rechazarlas? Justificá considerando ventajas y desventajas de cada postura. ¿Qué hace tu sistema hoy en ese caso?  
En un punto de venta real, la prioridad es cobrar (o sea que este disponible); se encolan las ventas localmente y se sincronizan cuando Inventario vuelva. Pero bajo nuestro alcance actual, se prioriza la Consistencia, se rechaza la venta devolviendo un 503 Service Unavailable para evitar sobreventas.

**Pregunta D — URL hardcodeada**
¿Por qué tener la URL del compañero escrita directamente en el código como `"http://localhost:5000"` es un problema? ¿Cómo lo resolviste en tu proyecto?  
Es un problema porque impide desplegar el sistema en diferentes entornos (Desarrollo, QA, Produccion) sin modificar y recompilar, rebuildear el codigo fuente. Se resuelve inyectando la URL mediante variables de entorno (.env) que es lo que hice.