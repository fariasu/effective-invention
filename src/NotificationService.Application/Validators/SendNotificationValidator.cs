using FluentValidation;
using NotificationService.Domain.Commands;

namespace NotificationService.Application.Validators;

public class SendNotificationValidator : AbstractValidator<SendNotificationCommand>
{
    public SendNotificationValidator()
    {
        RuleFor(x => x.Recipient)
            .NotEmpty().WithMessage("Recipient is required.")
            .EmailAddress().WithMessage("Invalid email address.");
        RuleFor(x => x.NotificationChannel)
            .NotNull().WithMessage("Notification Channel is required.")
            .IsInEnum().WithMessage("Invalid Notification Channel.");
        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.")
            .MaximumLength(32676).WithMessage("Content is too long.");
    }
}