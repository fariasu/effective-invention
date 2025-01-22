using FluentValidation;
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
    INotificationQueueManager notificationQueueManager, 
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<SendNotificationCommand, OperationResult>
{
    public async Task<OperationResult> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var validator = await new SendNotificationValidator().ValidateAsync(request, cancellationToken);
        
        var notification = new Notification(request.Recipient, request.Content);
        notification.ChangeNotificationChannel(request.NotificationChannel);

        await notificationRepository.AddNotificationAsync(notification);

        try
        {
            if (!validator.IsValid)
            {
                notification.ChangeNotificationStatus(ENotificationStatus.Failed);
                throw new ValidationException(errors: validator.Errors);
            }
            
            await SendNotificationAsync(notification, cancellationToken);
        }
        catch (ValidationException ex)
        {
            notification.ChangeNotificationStatus(ENotificationStatus.Failed);
            notification.Errors = string.Join(", ", ex.Errors.Select(x => x.ErrorMessage));
            
        }
        catch (Exception ex)
        {
            notification.ChangeNotificationStatus(ENotificationStatus.Failed);
            notification.Errors = ex.Message;
        }
        finally
        {
            await unitOfWork.CommitAsync(cancellationToken);
        }
        
        return new OperationResult{ Id = notification.Id };
    }

    private async Task SendNotificationAsync(Notification notification, CancellationToken ct)
    {
        var result = await notificationQueueManager.ProcessNotificationAsync(notification, ct);

        if (result.Equals(ENotificationStatus.Failed))
        {
            throw new Exception("Enqueue failed.");
        }
    }
}

