using System.Collections;
using NotificationService.Domain.Enums;

namespace NotificationService.Domain.Entities;

public class Notification(string recipient, string message, ENotificationChannel notificationChannel)
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Recipient { get; private set; } = recipient;
    public string Message { get; private set; } = message;
    public ENotificationChannel NotificationChannel { get; private set; }
    public ENotificationStatus NotificationStatus { get; private set; } = ENotificationStatus.Pending;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public List<string> Errors { get; set; } = [];
    public DateTime? SentAt { get; private set; } = null;

    public void ChangeNotificationStatus(ENotificationStatus notificationStatus)
    {
        NotificationStatus = notificationStatus;
    }

    public void HasSent()
    {
        SentAt = DateTime.UtcNow;
    }
}