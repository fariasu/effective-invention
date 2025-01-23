using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Repositories;
using NotificationService.Domain.Responses;
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

    public async Task<List<Notification>> GetAllNotificationsAsync(int page, int pageSize)
    {
        return await dbContext.Notifications
            .AsNoTracking()
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}