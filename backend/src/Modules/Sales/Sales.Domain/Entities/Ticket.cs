using Sales.Domain.Enums;

namespace Sales.Domain.Entities;

public class Ticket
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public string TicketNumber { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public TicketStatus Status { get; private set; }
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

    public static Ticket CreateOpenTicket(int companyId, string ticketNumber, decimal currentTaxRate)
    {
        return new Ticket
        {
            CompanyId = companyId,
            TicketNumber = ticketNumber,
            CreatedAt = DateTime.UtcNow,
            Status = TicketStatus.Open,
            TaxRate = currentTaxRate,
            SubTotal = 0,
            TaxAmount = 0,
            Total = 0
        };
    }

    public void AssignWaiter(string waiterName)
    {
        if (Status != TicketStatus.Open)
            throw new InvalidOperationException("Solo se puede asignar mesero a cuentas abiertas");
        if (string.IsNullOrWhiteSpace(waiterName)) throw new ArgumentException("El nombre del mesero es obligatorio");
        
        waiterName = waiterName;
    }

    public void SetCustomer(string? name, string? phone)
    {
        if (Status != TicketStatus.Open)
            throw new InvalidOperationException("No se puede editar clientes en cuentas cerradas");

        CustomerName = name;
        CustomerPhone = phone;
    }

    public void AddItem(int productId, string productName, decimal quantity, decimal unitPrice, string? note)
    {
        if (Status != TicketStatus.Open)
            throw new InvalidOperationException("No se pueden agregar items a una cuenta cerrada");

        if (quantity <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero");
        
        var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);

        if (existingItem != null)
            throw new InvalidOperationException("El producto ya existe en el ticket ");
        
        _items.Add(new TicketItem(this.Id, productId, productName, quantity, unitPrice, note));

        RecalculateTotals();
    }

    public void UpdateItemQuantity(int productId, decimal newQuantity)
    { 
        if (Status != TicketStatus.Open)
            throw new InvalidOperationException("No se pueden modificar items a una cuenta cerrada");
        
        var item = _items.FirstOrDefault(i => i.ProductId == productId);

        if (item == null)
        {
            throw new InvalidOperationException("El item no existe en el ticket");
        }
        
        item.UpdateQuantity(newQuantity);

        RecalculateTotals();
    }

    public void RemoveItem(int productId)
    {
        if (Status != TicketStatus.Open)
            throw new InvalidOperationException("No se puede eliminar items de una cuenta cerrada");
        
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
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

    public void SendToKds(int ticketItemId, KdsStation station)
    {
        if (Status != TicketStatus.Open)
            throw new InvalidOperationException("No se puede enviar comandas de cuentas cerradas");
        if (!_comandaItems.Any(c => c.TicketItemId == ticketItemId))
        {
            _comandaItems.Add(new ComandaItem(this.Id, ticketItemId, station));
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