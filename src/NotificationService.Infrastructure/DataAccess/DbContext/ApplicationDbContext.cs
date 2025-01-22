using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;

namespace NotificationService.Infrastructure.DataAccess.DbContext;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<Notification> Notifications { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var statusConverter = new ValueConverter<ENotificationStatus, string>(
            v => v.ToString(),
            v => (ENotificationStatus)Enum.Parse(typeof(ENotificationStatus), v));

        modelBuilder.Entity<Notification>()
            .Property(n => n.NotificationStatus)
            .HasConversion(statusConverter);

        var channelConverter = new ValueConverter<ENotificationChannel, string>(
            v => v.ToString(),
            v => (ENotificationChannel)Enum.Parse(typeof(ENotificationChannel), v));

        modelBuilder.Entity<Notification>()
            .Property(n => n.NotificationChannel)
            .HasConversion(channelConverter);
    }
}