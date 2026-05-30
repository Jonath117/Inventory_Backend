namespace Inventory.Domain.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string name, object key) : 
        base ($"El recurso {name} con clave {key} no  fue encontrado") { } 
}