using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Domain.Commands;
using NotificationService.Domain.Entities;

namespace NotificationService.API.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class NotificationServiceController : ControllerBase
{
    [HttpPost("send-notification")]
    public async Task<ActionResult> SendNotification(
        [FromServices] IMediator mediator,
        [FromBody] SendNotificationCommand request)
    {
        var response = await mediator.Send(request);
        
        return Ok(response);
    }
}