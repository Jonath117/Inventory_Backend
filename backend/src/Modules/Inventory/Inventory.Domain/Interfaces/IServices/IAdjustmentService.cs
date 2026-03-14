using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface IAdjustmentService
{
    Task RegisterAdjustmentAsync(int companyId, AdjustmentDto adjustment);
}