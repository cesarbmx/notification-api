using CesarBmx.Notification.Domain.Types;
using System;


namespace CesarBmx.Notification.Domain.Models
{
    public class PhoneMessage : Notification
    {
        public string PhoneNumber { get; private set; }
        public PhoneApp DestinationApp { get; private set; }

        public PhoneMessage() { }
        public PhoneMessage(
            Guid guid,
            string userId,
            NotificationType notificationType,
            string phoneNumber,
            PhoneApp phoneApp,
            string text,
            DateTime? scheduledFor)
            : base(guid, userId, notificationType, text, scheduledFor)
        {
            PhoneNumber = phoneNumber;
            DestinationApp = phoneApp;
        }

        public PhoneMessage SetDestinationApp(PhoneApp phoneApp, string phoneNumber) {

            DestinationApp = phoneApp;
            PhoneNumber = phoneNumber;

            return this;
        }
    }
}
