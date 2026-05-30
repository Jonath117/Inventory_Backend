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


    public async Task<IEnumerable<UnitContractDto>> GetUnitsAsync(int companyId)
    {
        var units = await _repository.GetUnitsAsync(companyId);
        
        return units.Select(u => new UnitContractDto(
            u.UnitCen, 
            u.Name, 
            u.Abbreviation, 
            u.IsActive
        )).ToList();
    }

    public async Task<UnitContractDto> CreateUnitAsync(int companyId, CreateUnitContractRequest request)
    {
        var newUnit = new Unit(companyId, request.Name, request.Abbreviation);
        
        var savedUnit = await _repository.AddAsync(newUnit);
        
        return new UnitContractDto(
            savedUnit.UnitCen, 
            savedUnit.Name, 
            savedUnit.Abbreviation, 
            savedUnit.IsActive
        );
    }
    public async Task<UnitContractDto> UpdateUnitAsync(int companyId, string unitCen, CreateUnitContractRequest request)
    {
        var unit = await _repository.GetByUnitCenAsync(companyId, unitCen);

        if (unit == null)
            throw new KeyNotFoundException($"La unidad con código {unitCen} no existe.");

        unit.Update(request.Name, request.Abbreviation);
        
        await _repository.UpdateAsync(unit);

        return new UnitContractDto(
            unit.UnitCen, 
            unit.Name, 
            unit.Abbreviation, 
            unit.IsActive
        );
    }
}