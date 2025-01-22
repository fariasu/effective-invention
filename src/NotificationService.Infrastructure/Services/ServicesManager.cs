using MassTransit;
using NotificationService.Domain.Entities;

namespace NotificationService.Infrastructure.Services;

public abstract class ServicesManager : IConsumer<Notification>
{
    public Task Consume(ConsumeContext<Notification> context)
    {
        Console.WriteLine($"Received notification: {context.Message.Message}");
        return Task.CompletedTask;
    }
}