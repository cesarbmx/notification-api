using CesarBmx.Notification.Domain.Types;
using System;


namespace CesarBmx.Notification.Domain.Models
{
    public class Email: Notification 
    {
        public string EmailAddress { get; private set; }

        public Email() { }
        public Email(
            Guid guid,
            string userId,
            NotificationType notificationType, 
            string emailAddress,
            string text, 
            DateTime? scheduledFor)
            :base(guid, userId, notificationType, text, scheduledFor)
        {
            EmailAddress = emailAddress;
        }
    }
}
