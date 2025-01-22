using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Services.Handlers;

public class EmailNotificationHandler : INotificationHandler
{
    public async Task HandleAsync(Notification notification)
    {
        Console.WriteLine("Sending Email notification!");
        await Task.CompletedTask;
    }
}