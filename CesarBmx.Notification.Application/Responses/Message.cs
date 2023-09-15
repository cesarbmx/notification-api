using System;
using CesarBmx.Notification.Domain.Types;

namespace CesarBmx.Notification.Application.Responses
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public string UserId { get; set; }
        public bool Telegram { get; set; }
        public bool Whatsapp { get; set; }
        public bool Email { get;  set; }
        public DeliveryType DeliveryType { get; set; }
        public MessageStatus MessageStatus { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ScheduledFor { get; set; }
        public DateTime? SentTime { get; set; }

    }
}
