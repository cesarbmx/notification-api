using System;
using CesarBmx.Shared.Common.Extensions;
using CesarBmx.Notification.Domain.Types;


namespace CesarBmx.Notification.Domain.Models
{
    public abstract class Notification 
    {
        public Guid NotificationId { get; private set; }
        public string UserId { get; set; } = null!;
        public NotificationType NotificationType { get; private set; }
        public NotificationStatus NotificationStatus { get; private set; }
        public string Text { get; private set; }
        public DateTime? ScheduledFor { get; private set; }
        public DateTime? SentAt { get; private set; }

        public Notification() { }
        public Notification(Guid guid, string userId, NotificationType notificationType, string text, DateTime? scheduledFor)
        {
            NotificationId = guid;
            UserId = userId;
            NotificationType = notificationType;
            Text = text;
            SentAt = null;
            ScheduledFor = scheduledFor;
        }

        public void MarkAsSent()
        {
            NotificationStatus = NotificationStatus.SENT;
            SentAt = DateTime.UtcNow.StripSeconds();
        }
    }
}
