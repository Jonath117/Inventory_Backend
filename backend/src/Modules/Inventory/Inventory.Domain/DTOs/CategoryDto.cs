namespace Inventory.Domain.DTOs;


public record CategoryLookupDto(int Id, string Name);

public record CategoryCreateDto(int CompanyId, string Name, string? Description);

public record CategoryUpdateDto(int Id, int CompanyId, string Name, string? Description);