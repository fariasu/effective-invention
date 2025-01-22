using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Services.Handlers;

public class SMSNotificationHandler : INotificationHandler
{
    public async Task HandleAsync(Notification notification)
    {
        Console.WriteLine("Sending SMS notification!");
        await Task.CompletedTask;
    }
}