using MassTransit;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;

namespace NotificationService.Infrastructure.Services
{
    public class ServicesManager(Func<ENotificationChannel, INotificationHandler> handlerFactory)
        : IConsumer<Notification>
    {
        public async Task Consume(ConsumeContext<Notification> context)
        {
            var notification = context.Message;
            
            switch (notification.NotificationChannel)
            {
                case ENotificationChannel.Email:
                case ENotificationChannel.SMS:
                    var handler = handlerFactory(notification.NotificationChannel);
                    await handler.HandleAsync(notification);
                    break;

                case ENotificationChannel.All:
                    var emailHandler = handlerFactory(ENotificationChannel.Email);
                    var smsHandler = handlerFactory(ENotificationChannel.SMS);

                    await Task.WhenAll(
                        emailHandler.HandleAsync(notification),
                        smsHandler.HandleAsync(notification)
                    );
                    break;

                default:
                    throw new ArgumentException("Service type is invalid.");
            }
        }
    }
}