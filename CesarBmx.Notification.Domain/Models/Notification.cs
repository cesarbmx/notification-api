using System;
using CesarBmx.Shared.Common.Extensions;
using CesarBmx.Notification.Domain.Builders;
using CesarBmx.Notification.Domain.Types;


namespace CesarBmx.Notification.Domain.Models
{
    public class Notification 
    {
        public Guid NotificationId { get; private set; }
        public NotificationType NotificationType { get; private set; }
        public string UserId { get; set; } = null!;
        public string PhoneNumber { get; private set; }
        public string Text { get; private set; }     
        public DateTime? SentAt { get; private set; }
        public NotificationStatus NotificationStatus => NotificationBuilder.BuildNotificationStatus(SentAt);

        public Notification() { }
        public Notification(Guid guid, NotificationType notificationType, string userId, string phoneNumber, string text)
        {
            NotificationId = guid;
            NotificationType = notificationType;
            UserId = userId;
            PhoneNumber = phoneNumber;
            Text = text;
            SentAt = null;
        }

        public void MarkAsSent()
        {
            SentAt = DateTime.UtcNow.StripSeconds();
        }
    }
}
