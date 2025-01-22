using MediatR;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Responses;

namespace NotificationService.Domain.Commands;

public class SendNotificationCommand : IRequest<OperationResult>
{
    public string Recipient { get; set; } = string.Empty;
    public ENotificationChannel NotificationChannel { get; set; }
    public string Content { get; set; } = string.Empty;
}