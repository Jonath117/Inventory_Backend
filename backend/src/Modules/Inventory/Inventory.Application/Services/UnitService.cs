using Inventory.Domain.DTOs;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Domain.Interfaces.IServices;

namespace Inventory.Application.Services;

public class UnitService : IUnitService
{
    private readonly IUnitRepository _repository;

    public UnitService(IUnitRepository repository)
    {
        _repository = repository;
    }


    public async Task<IEnumerable<UnitLookUpDto>> GetUnitsAsync(int companyId)
    {
        var units = await _repository.GetUnitsAsync(companyId); 
        return units.Select(u => new UnitLookUpDto(u.Id, u.Name, u.Description)).ToList();
    }

    public async Task<UnitLookUpDto> CreateUnitAsync(UnitCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new ArgumentException("El nombre de la unidad es obligatorio");
        }

        if (dto.CompanyId <= 0)
        {
            throw new ArgumentException("Id de compañia invalido");
        }

        bool exists = await _repository.ExistsByNameAsync(dto.CompanyId, dto.Name);
        if (exists)
        {
            throw new ArgumentException("Ya existe una unidad con ese nombre");
        }

        var newUnit = new Unit
        {
            CompanyId = dto.CompanyId,
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim()
        };

        var savedUnit = await _repository.AddAsync(newUnit);
        
        return new UnitLookUpDto(savedUnit.Id, savedUnit.Name, savedUnit.Description);
    }
}