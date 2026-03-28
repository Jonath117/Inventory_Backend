namespace Inventory.Domain.DTOs;

public record UnitLookUpDto(int Id, string Name, string? Description);

public record UnitCreateDto(string Name, string? Description, int CompanyId = 0);