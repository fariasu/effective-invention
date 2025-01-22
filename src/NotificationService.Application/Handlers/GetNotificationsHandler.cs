using MediatR;
using NotificationService.Domain.Commands;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Repositories;

namespace NotificationService.Application.Handlers;

public class GetNotificationsHandler(INotificationRepository notificationRepository) : IRequestHandler<GetNotificationCommand, List<Notification>>
{
    public Task<List<Notification>> Handle(GetNotificationCommand request, CancellationToken cancellationToken)
    {
        return notificationRepository.GetAllNotificationsAsync();
    }
}