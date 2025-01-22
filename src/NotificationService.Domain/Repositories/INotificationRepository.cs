using NotificationService.Domain.Entities;

namespace NotificationService.Domain.Repositories;

public interface INotificationRepository
{
    Task AddNotificationAsync(Notification notification);
    Task<Notification?> GetNotificationByIdAsync(Guid id);
}