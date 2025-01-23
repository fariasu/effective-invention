using MediatR;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Responses;

namespace NotificationService.Domain.Commands;

public class GetNotificationCommand : IRequest<List<Notification>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
}