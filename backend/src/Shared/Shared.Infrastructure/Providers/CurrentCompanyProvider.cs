using Shared.Application.Interfaces;

namespace Shared.Infrastructure.Providers;

public class CurrentCompanyProvider : ICurrentCompanyProvider
{
    private int? _companyId;
    
    public int CompanyId => _companyId ?? throw new InvalidOperationException("CompanyId no ha sido inicializado en el contexto actual");

    public void SetCompanyId(int companyId)
    {
        if (_companyId.HasValue)
            throw new InvalidOperationException("CompanyId ya fue establecido para esta peticion");
        
        _companyId =  companyId;
    }
}