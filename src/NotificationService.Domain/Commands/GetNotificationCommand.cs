using MediatR;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Responses;

namespace NotificationService.Domain.Commands;

public class GetNotificationCommand : IRequest<List<Notification>>
{
    
}