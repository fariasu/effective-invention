using NotificationService.Domain.Entities;
using NotificationService.Domain.Responses;

namespace NotificationService.Domain.Repositories;

public interface INotificationRepository
{
    Task AddNotificationAsync(Notification notification);
    Task<Notification?> GetNotificationByIdAsync(Guid id);
    Task<List<Notification>> GetAllNotificationsAsync();
}