using NotificationService.Domain.Entities;
using NotificationService.Domain.Repositories;
using NotificationService.Infrastructure.DataAccess.DbContext;

namespace NotificationService.Infrastructure.DataAccess.Repositories;

public class NotificationRepository(ApplicationDbContext dbContext) : INotificationRepository
{
    public async Task AddNotificationAsync(Notification notification)
    {
        await dbContext.Notifications.AddAsync(notification);
    }

    public Task<Notification?> GetNotificationByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}