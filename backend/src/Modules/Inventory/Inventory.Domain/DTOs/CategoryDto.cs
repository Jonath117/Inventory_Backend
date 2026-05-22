namespace Inventory.Domain.DTOs;


public record CategoryLookupDto(int Id, string Name, string? Description);

public record CategoryCreateDto(int CompanyId, string Name, string? Description);

public record CategoryUpdateDto(string CategoryCen, int CompanyId, string Name, string? Description);

public record CategoryContractDto(
    string CategoryCen, 
    string Name, 
    string? Description, 
    bool IsActive
);

public record CreateCategoryContractRequest(string Name, string? Description);