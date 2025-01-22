using Microsoft.Extensions.Logging;
using NotificationService.Domain.Commands;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Repositories;
using NotificationService.Domain.Services;
using NotificationService.Infrastructure.DataAccess.DbContext;

namespace NotificationService.Infrastructure.Services;

public class NotificationService(
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork)
    : INotificationService
{
    public async Task<ENotificationStatus> ProcessNotificationAsync(
        Notification notification, 
        CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(15000, cancellationToken);
            //await _messageQueue.EnqueueAsync(notification, cancellationToken);

            notification.ChangeNotificationStatus(ENotificationStatus.Sent);
            notification.HasSent();
        }
        catch (Exception ex)
        {
            notification.ChangeNotificationStatus(ENotificationStatus.Failed);
            notification.Errors.Add(ex.Message);
        }
        finally
        {
            await unitOfWork.CommitAsync(cancellationToken);
        }

        return notification.NotificationStatus;
    }

}
