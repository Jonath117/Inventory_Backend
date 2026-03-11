using WebApp1.Domain.DTOs;

namespace WebApp1.Domain.Interfaces;

public interface IInventoryService
{
    Task<DashboardDto> GetDashboardMetricsAsync(int companyId);
}