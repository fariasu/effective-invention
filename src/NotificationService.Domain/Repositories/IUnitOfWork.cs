namespace NotificationService.Domain.Repositories;

public interface IUnitOfWork
{
    public Task CommitAsync(CancellationToken ct = default);
}