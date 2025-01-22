using NotificationService.Domain.Repositories;
using NotificationService.Infrastructure.DataAccess.DbContext;

namespace NotificationService.Infrastructure.DataAccess.Repositories;

public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public async Task CommitAsync(CancellationToken ct = default)
    {
        await dbContext.SaveChangesAsync(ct);
    }
}