namespace Inventory.Domain.DTOs;

public class CompanyDto
{
    public int Id { get; set; }
    public string CompanyCen { get; set; }
    public string Name { get; set; } = string.Empty;
}

public record CompanyContractDto(
    string CompanyCen,
    string Name,
    bool IsActive
);

public record CompanyLookupContractDto(
    int CompanyId,
    string CompanyCen,
    string Name
);