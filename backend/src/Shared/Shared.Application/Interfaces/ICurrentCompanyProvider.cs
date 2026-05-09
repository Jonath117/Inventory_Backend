namespace Shared.Application.Interfaces;

public interface ICurrentCompanyProvider
{
    int CompanyId { get; }
    public void SetCompanyId(int companyId);
}