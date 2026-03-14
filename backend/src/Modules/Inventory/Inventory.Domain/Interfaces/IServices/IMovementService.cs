using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface IMovementService
{
    Task RegisterMovement(int companyId, MovementDto request);
}