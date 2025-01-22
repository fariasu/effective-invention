using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Domain.Commands;
using NotificationService.Domain.Entities;

namespace NotificationService.API.Controllers;

[ApiController]
[Route("v1/")]
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

    [HttpGet("get-all-notifications")]
    public async Task<ActionResult> GetNotifications([FromServices] IMediator mediator, [FromRoute] GetNotificationCommand request)
    {
        var response = await mediator.Send(request);
        
        return Ok(response);
    }
}