using Sales.Domain.Enums;

namespace Sales.Domain.Entities;

public class ComandaItem
{
    public int Id { get; private set; }
    public int TicketId { get; private set; }
    public int TicketItemId { get; private set; }
    public KdsStation Station { get; private set; }
    public KdsItemStatus Status { get; private set; }
    public DateTime SentAt { get; private set; }
    
    private ComandaItem() { }

    internal ComandaItem(int ticketId, int ticketItemId, KdsStation station)
    {
        TicketId = ticketId;
        TicketItemId = ticketItemId;
        Station = station;
        Status = KdsItemStatus.Pending;
        SentAt = DateTime.UtcNow;
    }

    public void MarkAsPreparing()
    {
        if (Status != KdsItemStatus.Pending) throw new InvalidOperationException("Solo items pendientes pueden pasar a preparacion");
        Status = KdsItemStatus.Preparing;
    }

    public void MarksAsReady()
    {
        if(Status != KdsItemStatus.Preparing) throw new InvalidOperationException("Solo los items en preparacion pueden pasar a listos");
        Status = KdsItemStatus.Ready;
    }
}