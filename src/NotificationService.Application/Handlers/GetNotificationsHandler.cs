using System.Text.Json;
using MediatR;
using MessagePack;
using Microsoft.Extensions.Caching.Distributed;
using NotificationService.Domain.Commands;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Repositories;
using StackExchange.Redis;

namespace NotificationService.Application.Handlers;

public class GetNotificationsHandler(INotificationRepository notificationRepository, IDistributedCache cache) : IRequestHandler<GetNotificationCommand, List<Notification>>
{
    public async Task<List<Notification>> Handle(GetNotificationCommand request, CancellationToken cancellationToken)
    {

        var cacheKey = $"{request.Page}:{request.PageSize}";

        var cachedData = await cache.GetAsync(cacheKey, cancellationToken);
        if (cachedData is { Length: > 0 })
        {
            return MessagePackSerializer.Deserialize<List<Notification>>(cachedData);
        }

        var data = notificationRepository.GetAllNotificationsAsync(request.Page, request.PageSize);

        var notifications = await data;
        var serializedData = MessagePackSerializer.Serialize(notifications, cancellationToken: cancellationToken);


        await cache.SetAsync(cacheKey, serializedData, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
        }, cancellationToken);

        return await data;
    }
}