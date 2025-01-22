using MassTransit;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Repositories;
using NotificationService.Domain.Services;
using NotificationService.Infrastructure.DataAccess.DbContext;

namespace NotificationService.Infrastructure.Publishers;

public class NotificationPublisher(
    ApplicationDbContext dbContext,
    IUnitOfWork unitOfWork,
    IBus bus)
    : INotificationQueueManager
{
    public async Task<ENotificationStatus> ProcessNotificationAsync(
        Notification notification, 
        CancellationToken cancellationToken)
    {
        try
        {
            await bus.Publish(notification, cancellationToken);

            notification.ChangeNotificationStatus(ENotificationStatus.Sent);
            notification.HasSent();
        }
        catch (Exception ex)
        {
            notification.ChangeNotificationStatus(ENotificationStatus.Failed);
            notification.Errors = ex.Message;
        }
        finally
        {
            await unitOfWork.CommitAsync(cancellationToken);
        }

        return notification.NotificationStatus;
    }

}
