using WebApp1.Domain.DTOs;

namespace WebApp1.Domain.Interfaces;

public interface IMovementService
{
    Task RegisterMovement(int companyId, MovementDto request);
}