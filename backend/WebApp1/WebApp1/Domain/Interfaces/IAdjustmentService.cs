using WebApp1.Domain.DTOs;

namespace WebApp1.Domain.Interfaces;

public interface IAdjustmentService
{
    Task RegisterAdjustmentAsync(int companyId, AdjustmentDto adjustment);
}