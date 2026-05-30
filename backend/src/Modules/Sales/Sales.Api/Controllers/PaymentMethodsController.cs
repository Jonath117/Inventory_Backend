using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sales.Application.Features.PaymentMethods;

namespace Sales.Api.Controllers;

[ApiController]
[ApiExplorerSettings(GroupName = "sales")]
[Route("api/sales/payment-methods")]
public class PaymentMethodsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentMethodsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetPaymentMethods()
    {
        var methods = await _mediator.Send(new GetPaymentMethodsQuery());
        return Ok(methods);
    }
}