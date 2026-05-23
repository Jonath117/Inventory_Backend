using Sales.Domain.Enums;

namespace Sales.Domain.Entities;

public class Ticket
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public string TicketCen { get; private set; } = null!;
    public string WarehouseCen { get; private set; } = null!;
    
    public string TicketNumber { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public TicketStatus Status { get; private set; }
    
    public string? WaiterCen { get; private set; }
    public string? WaiterName { get; private set; }
    
    public string? CustomerName { get; private set; }
    public string? CustomerPhone { get; private set; }

    public decimal SubTotal { get; private set; }
    public decimal TaxAmount { get; private set; }
    public decimal Total { get; private set; }
    public decimal TaxRate { get; private set; }
    
    public PaymentMethod? PaymentMethod { get; private set; }
    public DateTime? PaidAt { get; private set; }

    private readonly List<TicketItem> _items = new();
    public IReadOnlyCollection<TicketItem> Items => _items.AsReadOnly();

    private readonly List<ComandaItem> _comandaItems = new();
    public IReadOnlyCollection<ComandaItem> ComandaItems => _comandaItems.AsReadOnly();
    
    private Ticket() { }

    public static Ticket CreateOpenTicket(int companyId, string ticketCen, string ticketNumber, decimal currentTaxRate, string warehouseCen)
    {
        return new Ticket
        {
            CompanyId = companyId,
            TicketCen = ticketCen, 
            TicketNumber = ticketNumber,
            CreatedAt = DateTime.UtcNow,
            Status = TicketStatus.Open,
            TaxRate = currentTaxRate,
            WarehouseCen = warehouseCen,
            SubTotal = 0,
            TaxAmount = 0,
            Total = 0
        };
    }

    public void AssignWaiter(string waiterCen, string waiterName)
    {
        if (Status != TicketStatus.Open)
            throw new InvalidOperationException("Solo se puede asignar mesero a cuentas abiertas");
        if (string.IsNullOrWhiteSpace(waiterCen)) throw new ArgumentException("El CEN del mesero es obligatorio");
        
        WaiterCen = waiterCen;
        WaiterName = waiterName;
    }

    public void SetCustomer(string? name, string? phone)
    {
        if (Status != TicketStatus.Open)
            throw new InvalidOperationException("No se puede editar clientes en cuentas cerradas");

        CustomerName = name;
        CustomerPhone = phone;
    }

    public void AddOrUpdateItem(string productCen, string productName, decimal quantity, decimal unitPrice, string? note)
    {
        if (Status != TicketStatus.Open)
            throw new InvalidOperationException("No se pueden agregar items a una cuenta cerrada");

        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero");
    
        var existingItem = _items.FirstOrDefault(i => i.ProductCen == productCen);

        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            string ticketItemCen = Guid.NewGuid().ToString().Substring(0, 8);
            _items.Add(new TicketItem(Id, ticketItemCen, productCen, productName, quantity, unitPrice, note));
        }

        RecalculateTotals();
    }

    public void UpdateItemQuantity(string productCent, decimal newQuantity)
    { 
        if (Status != TicketStatus.Open)
            throw new InvalidOperationException("No se pueden modificar items a una cuenta cerrada");
        
        var item = _items.FirstOrDefault(i => i.ProductCen == productCent);

        if (item == null)
        {
            throw new InvalidOperationException("El item no existe en el ticket");
        }
        
        item.UpdateQuantity(newQuantity);

        RecalculateTotals();
    }

    public void RemoveItem(string productCent)
    {
        if (Status != TicketStatus.Open)
            throw new InvalidOperationException("No se puede eliminar items de una cuenta cerrada");
        
        var item = _items.FirstOrDefault(i => i.ProductCen == productCent);
        if (item != null)
        {
            _items.Remove(item);
            RecalculateTotals();
        }
    }

    private void RecalculateTotals()
    {
        SubTotal = _items.Sum(i => i.SubTotal);
        TaxAmount = SubTotal * (TaxRate / 100);
        Total = SubTotal + TaxAmount;
    }

    public void SendToKds(string ticketItemCen, KdsStation station)
    {
        if (Status != TicketStatus.Open)
            throw new InvalidOperationException("No se puede enviar comandas de cuentas cerradas");
        if (!_comandaItems.Any(c => c.TicketItemCen == ticketItemCen))
        {
            _comandaItems.Add(new ComandaItem(this.Id, ticketItemCen, station));
        }
    }

    public void Pay(PaymentMethod method)
    {
        if (Status != TicketStatus.Open)
            throw new InvalidOperationException("El ticket no está en estado abierto para ser pagado");
        if (string.IsNullOrWhiteSpace(WaiterName))
            throw new InvalidOperationException("Se requiere asignar un mesero antes de cobrar");
        if (!_items.Any()) throw new InvalidOperationException("No se puede cobrar un ticket vacio");

        Status = TicketStatus.Paid;
        PaymentMethod = method;
        PaidAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status != TicketStatus.Open)
            throw new InvalidOperationException("Solo se puedenc cancelar tickets abiertos");
        Status = TicketStatus.Cancelled;
    }
}