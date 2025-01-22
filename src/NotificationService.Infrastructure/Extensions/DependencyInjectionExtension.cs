using System.Reflection;
using FluentMigrator.Runner;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Domain.Repositories;
using NotificationService.Domain.Services;
using NotificationService.Infrastructure.DataAccess.DbContext;
using NotificationService.Infrastructure.DataAccess.Repositories;

namespace NotificationService.Infrastructure.Extensions;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddRabbitMQService(services, configuration);
        ConfigureFluentMigrator(services, configuration);
        services.AddTransient<INotificationService, Services.NotificationService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
    }
    
    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddDbContext<ApplicationDbContext>(config =>
            config.UseNpgsql(connectionString));
    }
    
    private static void ConfigureFluentMigrator(IServiceCollection services, IConfiguration configuration)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(configuration.GetConnectionString("Database"))
                .ScanIn(Assembly.Load("NotificationService.Infrastructure")).For.All())
            .AddLogging(lb => lb.AddFluentMigratorConsole());
    }

    private static void AddRabbitMQService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration.GetValue<string>("RabbitMQ:Host"), host =>
                {
                    host.Username(configuration.GetValue<string>("RabbitMQ:Username")!);
                    host.Password(configuration.GetValue<string>("RabbitMQ:Password")!);
                });

                cfg.ConfigureEndpoints(ctx);
            });
        });
    }
}