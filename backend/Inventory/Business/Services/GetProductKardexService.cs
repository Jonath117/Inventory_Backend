using Microsoft.EntityFrameworkCore;
using WebApp1.Domain.DTOs;
using WebApp1.Domain.Interfaces;
using WebApp1.Infrastructure.Data;

namespace WebApp1.Business.Services;

public class GetProductKardexService :  IGetProductKardexService
{
    private readonly ApplicationDbContext _context;

    public GetProductKardexService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MovementHistoryDto>> GetProductKardexAsync(int companyId, int productId, int? warehouseId = null)
    {
        var query = _context.InventoryMovements
            .Include(m => m.Warehouse)
            .Where(m => m.CompanyId == companyId && m.ProductId == productId)
            .AsQueryable();

        if (warehouseId.HasValue && warehouseId.Value > 0)
        {
            query = query.Where(m => m.WarehouseId == warehouseId.Value);
        }

        var history = await query
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new MovementHistoryDto
            {
                Id = m.Id,
                Date = m.CreatedAt,
                MovementType = m.MovementType,
                Quantity = m.Quantity,
                PreviousStock = m.PreviousStock,
                NewStock = m.NewStock,
                Reason = m.Reason,
                Reference = m.Reference,
                WareHouseName = m.Warehouse!.Name,
            })
            .ToListAsync();

        return history; 
    }
}