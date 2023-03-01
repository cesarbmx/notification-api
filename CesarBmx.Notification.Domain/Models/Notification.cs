﻿using System;
using CesarBmx.Shared.Common.Extensions;
using CesarBmx.Notification.Domain.Builders;
using CesarBmx.Notification.Domain.Types;


namespace CesarBmx.Notification.Domain.Models
{
    public class Notification 
    {
        public int NotificationId { get; private set; }
        public string UserId { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Message { get; private set; }
        public DateTime? SentTime { get; private set; }
        public DateTime Time { get; private set; }
        public NotificationStatus NotificationStatus => NotificationBuilder.BuildNotificationStatus(SentTime);

        public Notification() { }
        public Notification(string userId, string phoneNumber, string message, DateTime time)
        {
            NotificationId = 0;
            UserId = userId;
            PhoneNumber = phoneNumber;
            Message = message;
            SentTime = null;
            Time = time;
        }

        public void MarkAsSent()
        {
            SentTime = DateTime.UtcNow.StripSeconds();
        }
    }
}
