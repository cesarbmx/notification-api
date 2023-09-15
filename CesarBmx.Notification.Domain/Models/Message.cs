using System;
using CesarBmx.Shared.Common.Extensions;
using CesarBmx.Notification.Domain.Types;
using CesarBmx.Notification.Domain.Expressions;

namespace CesarBmx.Notification.Domain.Models
{
    public class Message 
    {
        public Guid MessageId { get; private set; }
        public string UserId { get; set; } = null!;
        public bool Telegram { get; private set; }
        public bool Whatsapp { get; private set; }
        public bool Email { get; private set; }
        public DeliveryType DeliveryType { get; private set; }
        public MessageStatus MessageStatus { get; private set; }
        public string PhoneNumber { get; private set; }
        public string EmailAddress { get; private set; }
        public string Text { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ScheduledFor { get; private set; }
        public DateTime? SentAt { get; private set; }

        public Message() { }
        public Message(
            Guid guid, 
            string userId, 
            bool telegram, 
            bool whatsapp, 
            bool email, 
            DeliveryType deliveryType,
            string phoneNumber,
            string emailAddress,
            string text, 
            DateTime? scheduledFor)
        {
            MessageId = guid;
            UserId = userId;
            Telegram = telegram;
            Whatsapp = whatsapp;
            Email = email;
            DeliveryType = deliveryType;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            Text = text;
            CreatedAt = DateTime.UtcNow.StripSeconds();
            ScheduledFor = scheduledFor;
            SentAt = null;
        }

        public void MarkAsSent()
        {
            MessageStatus = MessageStatus.SENT;
            SentAt = DateTime.UtcNow.StripSeconds();
        }

        public void SetDestination() // From settings
        {
            Telegram = true;
            PhoneNumber = "+34666333222";
        }
    }
}
