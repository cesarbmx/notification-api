using System;
using CesarBmx.Notification.Domain.Types;


namespace CesarBmx.Notification.Domain.Builders
{
    public static class NotificationBuilder
    {
        public static NotificationStatus BuildNotificationStatus(DateTime? sentTime)
        {
            // If sent already, Notified
            if (sentTime.HasValue) return NotificationStatus.NOTIFIED;

            // If not sent yet, Pending
            return NotificationStatus.PENDING;
        }
    }
}
