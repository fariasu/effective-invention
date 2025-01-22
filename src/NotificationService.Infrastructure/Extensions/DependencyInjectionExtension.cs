using System.Reflection;
using FluentMigrator.Runner;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Repositories;
using NotificationService.Domain.Services;
using NotificationService.Infrastructure.DataAccess.DbContext;
using NotificationService.Infrastructure.DataAccess.Repositories;
using NotificationService.Infrastructure.Publishers;
using NotificationService.Infrastructure.Services;
using NotificationService.Infrastructure.Services.Handlers;

namespace NotificationService.Infrastructure.Extensions;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddRabbitMQService(services, configuration);
        ConfigureFluentMigrator(services, configuration);
        services.AddTransient<INotificationQueueManager, NotificationPublisher>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        
        //Notification Handlers
        ConfigureNotificationHandlers(services);
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
                .ScanIn(Assembly.Load("NotificationService.Infrastructure")).For.All());
    }

    private static void AddRabbitMQService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumer<ServicesManager>();
            
            busConfigurator.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.UseNewtonsoftJsonSerializer();
                
                cfg.Host(configuration.GetValue<string>("RabbitMQ:Host"), host =>
                {
                    host.Username(configuration.GetValue<string>("RabbitMQ:Username")!);
                    host.Password(configuration.GetValue<string>("RabbitMQ:Password")!);
                });

                cfg.ReceiveEndpoint("notifications", e =>
                {
                    e.ConfigureConsumer<ServicesManager>(ctx);
                });
                
                cfg.ConfigureEndpoints(ctx);
            });
        });
    }

    private static void ConfigureNotificationHandlers(IServiceCollection services)
    {
        //Notification Handlers
        services.AddScoped<EmailNotificationHandler>();
        services.AddScoped<SMSNotificationHandler>();
        services.AddScoped<ServicesManager>();
        
        services.AddScoped<Func<ENotificationChannel, INotificationHandler>>(provider => type =>
        {
            return type switch
            {
                ENotificationChannel.Email => provider.GetRequiredService<EmailNotificationHandler>(),
                ENotificationChannel.SMS => provider.GetRequiredService<SMSNotificationHandler>(),
                _ => throw new ArgumentException("Service type is invalid.", nameof(type)),
            };
        });
    }
}