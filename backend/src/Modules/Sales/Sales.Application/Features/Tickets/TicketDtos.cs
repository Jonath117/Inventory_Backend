namespace Sales.Application.Features.Tickets;

public record CreateTicketContractRequest(string? WaiterCen, string WarehouseCen); 

public record TicketContractResponse(
    string TicketCen,
    DateTime Date,
    string Status,
    string? WaiterName,
    decimal SubTotal,
    decimal TaxAmount,
    decimal Total,
    string? CustomerName,
    string WarehouseCen
);

public record TicketItemContractResponse(
    string TicketItemCen,
    string ProductCen,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal SubTotal,
    string? Note,
    string Status
);

public record CreateTicketItemContractRequest(
    string ProductCen,
    int Quantity,
    string? Note
);

public record TicketTotalsContractResponse(
    decimal SubTotal,
    decimal TaxAmount,
    decimal Total,
    decimal TaxRate
);

public record PayTicketContractRequest(
    int PaymentMethodId // 1: Cash, 2: Qr, 3: Card
);

public record PayTicketContractResponse(
    string TicketCen,
    string Status,
    DateTime PaidAt
);

public record CancelTicketContractRequest(
    string? Reason
);

public record CancelTicketContractResponse(
    string TicketCen,
    string Status
);

public record AssignTicketWaiterContractRequest(
    string WaiterCen
);

public record AssignTicketWaiterContractResponse(
    string TicketCen,
    string WaiterCen,
    string WaiterName
);

public record WaiterContractResponse(
    string WaiterCen,
    string Name,
    bool IsActive
);

public record DailySalesDashboardDto(
    double TotalSales,
    int TicketsCount,
    double AverageTicket
);

public record TopProductDashboardContractResponse(
    string ProductCen,
    string ProductName,
    int QuantitySold,
    double TotalRevenue
);

public record KdsStatusDashboardDto(
    int PendingItems,
    int PreparingItems,
    int ReadyItems
);

public record KdsTeamContractResponse(
    string TeamCen,
    string Name
);

public record KdsItemContractResponse(
    string TicketItemCen,
    string TicketCen,
    string ProductName,
    int Quantity,
    string? Note,
    string Status,
    DateTime SentAt,
    string Station
);

public record UpdateKdsItemStatusContractRequest(
    string Status // created, preparing, delivered, canceled
);

public record TaxConfigurationContractResponse(
    decimal TaxRate
);

public record UpdateTaxConfigurationContractRequest(
    decimal TaxRate
);