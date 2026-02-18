using WebApp1.Core.DTOs;

namespace WebApp1.Core.Interfaces;

public interface IInventoryService
{
    Task<DashboardDto> GetDashboardMetricsAsync(int companyId);
}