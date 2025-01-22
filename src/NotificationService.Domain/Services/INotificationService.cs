using MediatR;
using NotificationService.Domain.Commands;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;

namespace NotificationService.Domain.Services;

public interface INotificationService
{
    public Task<ENotificationStatus> ProcessNotificationAsync(Notification notification, CancellationToken cancellationToken);
}