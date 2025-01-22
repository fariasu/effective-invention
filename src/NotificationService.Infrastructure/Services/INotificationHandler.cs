using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Services;

public interface INotificationHandler
{
    Task HandleAsync(Notification notification);
}