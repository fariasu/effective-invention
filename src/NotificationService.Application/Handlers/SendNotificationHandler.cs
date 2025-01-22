using FluentValidation;
using MassTransit.RabbitMqTransport;
using MediatR;
using NotificationService.Application.Validators;
using NotificationService.Domain.Commands;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Repositories;
using NotificationService.Domain.Responses;
using NotificationService.Domain.Services;

namespace NotificationService.Application.Handlers;

public class SendNotificationHandler(
    INotificationService notificationService, 
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<SendNotificationCommand, OperationResult>
{
    public async Task<OperationResult> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var validator = await new SendNotificationValidator().ValidateAsync(request, cancellationToken);
        
        var notification = new Notification(request.Recipient, request.Content, request.NotificationChannel);

        await notificationRepository.AddNotificationAsync(notification);

        try
        {
            if (!validator.IsValid)
            {
                notification.ChangeNotificationStatus(ENotificationStatus.Failed);
                throw new ValidationException(errors: validator.Errors);
            }
            
            // Simula envio da notificação
            await SendNotificationAsync(notification, cancellationToken);
        }
        catch (ValidationException ex)
        {
            // Atualizar para "Failed" em caso de erro
            notification.ChangeNotificationStatus(ENotificationStatus.Failed);
            notification.Errors.AddRange(ex.Errors.Select(x => x.ErrorMessage));
        }
        catch (Exception ex)
        {
            notification.ChangeNotificationStatus(ENotificationStatus.Failed);
            notification.Errors.Add(ex.Message);
        }
        finally
        {
            await unitOfWork.CommitAsync(cancellationToken);
        }
        
        return new OperationResult{ Id = notification.Id };
    }

    private async Task SendNotificationAsync(Notification notification, CancellationToken ct)
    {
        var result = await notificationService.ProcessNotificationAsync(notification, ct);

        if (result.Equals(ENotificationStatus.Failed))
        {
            throw new Exception("Enqueue failed.");
        }
    }
}

