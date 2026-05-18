namespace Inventory.Domain.DTOs;

public record UnitLookUpDto(int Id, string Name, string? Description);

public record UnitCreateDto(string Name, string? Description, int CompanyId = 0);

public record CreateUnitContractRequest(
    string Name, 
    string? Abbreviation
);

public record UnitContractDto(
    string UnitCen, 
    string Name, 
    string? Abbreviation, 
    bool IsActive
);