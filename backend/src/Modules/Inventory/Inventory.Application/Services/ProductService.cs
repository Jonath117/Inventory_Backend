using Inventory.Domain.DTOs;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Domain.Interfaces.IServices;

namespace Inventory.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitRepository _unitRepository;
    private readonly IGetStockRepository _stockRepository;

    public ProductService(
        IProductRepository productRepository, 
        ICategoryRepository categoryRepository,
        IUnitRepository unitRepository,
        IGetStockRepository stockRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _unitRepository = unitRepository;
        _stockRepository = stockRepository;
    }

    public async Task<IEnumerable<ProductContractDto>> GetProductsAsync(int companyId)
    {
        var products = await _productRepository.GetProductsAsync(companyId);

        if (products == null || !products.Any())
        {
            return new List<ProductContractDto>();
        }

        return products.Select(p => new ProductContractDto(
            p.ProductCen,
            p.Sku,
            p.Name,
            p.Description,
            p.Category?.CategoryCen ?? string.Empty,
            p.Category?.Name ?? "Sin Categoría",
            p.Unit?.UnitCen ?? string.Empty,
            p.Unit?.Name ?? "Sin Unidad",
            p.SalePrice,
            p.Price,
            p.MinStockAlert,
            p.IsActive ? "ACTIVE" : "INACTIVE",
            p.StationCode
        )).ToList();
    }

    public async Task<ProductContractDto> CreateProductAsync(int companyId, CreateProductContractRequest request)
    {
        bool skuExists = await _productRepository.ExistsBySkuAsync(companyId, request.Sku);
        if (skuExists) throw new ArgumentException($"Ya existe un producto con el codigo SKU: {request.Sku}");
        
        var categoryInfo = await _categoryRepository.GetInfoByCenAsync(companyId, request.CategoryCen);
        if (categoryInfo.Id == 0) throw new ArgumentException("Categoría no válida");

        var unitInfo = await _unitRepository.GetInfoByCenAsync(companyId, request.UnitCen);
        if (unitInfo.Id == 0) throw new ArgumentException("Unidad no válida");


        var newProduct = new Product(
            companyId: companyId,
            sku: request.Sku,
            name: request.Name,
            categoryId: categoryInfo.Id,
            unitId: unitInfo.Id,
            price: request.CostPrice ?? 0,
            salePrice: request.SalePrice,
            minStockAlert: request.ReorderLevel,
            description: request.Description,
            stationCode: request.StationCode
        );
        
        var savedProduct = await _productRepository.AddAsync(newProduct);
        
        return new ProductContractDto(
            savedProduct.ProductCen,
            savedProduct.Sku,
            savedProduct.Name,
            savedProduct.Description,
            request.CategoryCen,     
            categoryInfo.Name,       
            request.UnitCen,         
            unitInfo.Name,           
            savedProduct.SalePrice,
            savedProduct.Price,
            savedProduct.MinStockAlert,
            savedProduct.IsActive ? "ACTIVE" : "INACTIVE",
            savedProduct.StationCode
        );
    }

    public async Task<ProductContractDto> EditProductAsync(int companyId, string productCen, UpdateProductContractRequest request)
    {
        var product = await _productRepository.GetByProductCenAsync(companyId, productCen);
        if (product == null) throw new KeyNotFoundException("El producto no existe");
        
        bool skuExists = await _productRepository.ExistsBySkuAsync(companyId, request.Sku, productCen);
        if (skuExists) throw new InvalidOperationException($"El codigo SKU {request.Sku} ya esta en uso por otro producto");
        
        var categoryInfo = await _categoryRepository.GetInfoByCenAsync(companyId, request.CategoryCen);
        if (categoryInfo.Id == 0) throw new ArgumentException("Categoría no válida");

        var unitInfo = await _unitRepository.GetInfoByCenAsync(companyId, request.UnitCen);
        if (unitInfo.Id == 0) throw new ArgumentException("Unidad no válida");
        
        product.Update(
            sku: request.Sku,
            name: request.Name,
            description: request.Description,
            price: request.CostPrice ?? 0,
            salePrice: request.SalePrice,
            minStockAlert: request.ReorderLevel,
            categoryId: categoryInfo.Id,
            unitId: unitInfo.Id,
            isActive: product.IsActive,
            stationCode: request.StationCode
        );
        
        await _productRepository.UpdateAsync(product);
        
        return new ProductContractDto(
            product.ProductCen,
            product.Sku,
            product.Name,
            product.Description,
            request.CategoryCen,
            categoryInfo.Name,
            request.UnitCen,
            unitInfo.Name,
            product.SalePrice,
            product.Price,
            product.MinStockAlert,
            product.IsActive ? "ACTIVE" : "INACTIVE",
            product.StationCode
        );
    }

    public async Task<ProductContractDto> UpdateProductStatusAsync(int companyId, string productCen, string status)
    {
        var product = await _productRepository.GetByProductCenAsync(companyId, productCen);
        if (product == null) throw new KeyNotFoundException("El producto no existe");

        if (status.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase))
            product.Activate();
        else if (status.Equals("INACTIVE", StringComparison.OrdinalIgnoreCase))
            product.Deactivate();
        else
            throw new ArgumentException("Estado no soportado. Usa ACTIVE o INACTIVE.");

        await _productRepository.UpdateAsync(product);
        
        return new ProductContractDto(
            product.ProductCen,
            product.Sku,
            product.Name,
            product.Description,
            product.Category?.CategoryCen ?? string.Empty,
            product.Category?.Name ?? "Sin Categoría",
            product.Unit?.UnitCen ?? string.Empty,
            product.Unit?.Name ?? "Sin Unidad",
            product.SalePrice,
            product.Price,
            product.MinStockAlert,
            product.IsActive ? "ACTIVE" : "INACTIVE",
            product.StationCode
        );
    }

    public async Task<IEnumerable<ProductContractDto>> LookupProductsAsync(int companyId, ProductLookupContractRequest request)
    {
        var products = await _productRepository.LookupProductsAsync(companyId, request.ProductCens, request.Skus);
        return products.Select(p => new ProductContractDto(
            p.ProductCen,
            p.Sku,
            p.Name,
            p.Description,
            p.Category?.CategoryCen ?? string.Empty,
            p.Category?.Name ?? "Sin Categoría",
            p.Unit?.UnitCen ?? string.Empty,
            p.Unit?.Name ?? "Sin Unidad",
            p.SalePrice,
            p.Price,
            p.MinStockAlert,
            p.IsActive ? "ACTIVE" : "INACTIVE",
            p.StationCode
        ));
    }

    public async Task<IEnumerable<SellableProductContractDto>> GetSellableProductsAsync(int companyId, string? search, string? categoryCen, string? warehouseCen, bool onlyAvailable, int page, int pageSize)
    {

        var products = await _productRepository.GetProductsByQueryAsync(companyId, search, categoryCen, "ACTIVE");
        
        var stocks = await _stockRepository.GetCurrentStockAsync(companyId, null, warehouseCen);
        var stockMap = stocks.GroupBy(s => s.Product!.ProductCen)
                             .ToDictionary(g => g.Key, g => (double)g.Sum(s => s.CurrentStock));

        var pagedProducts = products.Skip((page - 1) * pageSize).Take(pageSize);

        var result = new List<SellableProductContractDto>();
        foreach (var p in pagedProducts)
        {
            stockMap.TryGetValue(p.ProductCen, out double availableStock);

            if (onlyAvailable && availableStock <= 0) continue;

            result.Add(new SellableProductContractDto(
                p.ProductCen,
                p.Sku,
                p.Name,
                p.Description,
                p.Category?.Name ?? "Sin Categoría",
                p.Unit?.Name ?? "Sin Unidad",
                (double)p.SalePrice,
                availableStock,
                availableStock > 0
            ));
        }
        return result;
    }
    
    public async Task<ProductDetailsContractResponse?> GetProductDetailsByCenAsync(int companyId, string productCen)
    {
        var product = await _productRepository.GetByCenAsync(companyId, productCen);
    
        if (product == null) return null;

        return new ProductDetailsContractResponse(
            ProductCen: product.ProductCen,
            Name: product.Name,
            SalePrice: product.SalePrice,
            IsAvailable: product.IsActive 
        );
    }
}