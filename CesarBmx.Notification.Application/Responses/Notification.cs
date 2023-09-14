using System;
using CesarBmx.Notification.Domain.Types;

namespace CesarBmx.Notification.Application.Responses
{
    public class Notification
    {
        public Guid MessageId { get; set; }
        public string UserId { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Text { get; set; }
        public DateTime? ScheduledFor { get; set; }
        public DateTime? SentTime { get; set; }

    }
}
