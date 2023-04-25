using System;
using CesarBmx.Shared.Common.Extensions;
using CesarBmx.Notification.Domain.Builders;
using CesarBmx.Notification.Domain.Types;


namespace CesarBmx.Notification.Domain.Models
{
    public class Message 
    {
        public Guid MessageId { get; private set; }
        public string UserId { get; set; } = null!;
        public string PhoneNumber { get; private set; }
        public string Text { get; private set; }     
        public DateTime? SentAt { get; private set; }
        public NotificationStatus NotificationStatus => NotificationBuilder.BuildNotificationStatus(SentAt);

        public Message() { }
        public Message(Guid guid, string userId, string phoneNumber, string text)
        {
            MessageId = guid;
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
