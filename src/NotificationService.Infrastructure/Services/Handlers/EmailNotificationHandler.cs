using NotificationService.Domain.Entities;
using NotificationService.Infrastructure.Extensions;
using SendWithBrevo;

namespace NotificationService.Infrastructure.Services.Handlers;

public class EmailNotificationHandler(BrevoOptions options) : INotificationHandler
{
    public async Task HandleAsync(Notification notification)
    {
        BrevoClient client = new BrevoClient(options.ApiKey);
        
        await client.SendAsync(
            new Sender(options.SenderName, options.SenderEmail),
            new List<Recipient> { new Recipient(notification.Recipient.Split("@"[0]).ToString(), notification.Recipient) },
            "Redefinição de senha.",
            notification.Message,
            true
        );
    }
}