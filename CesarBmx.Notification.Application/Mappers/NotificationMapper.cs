using AutoMapper;
using CesarBmx.Notification.Domain.Models;
using CesarBmx.Shared.Messaging.Notification.Commands;
using CesarBmx.Shared.Messaging.Notification.Events;
using Twilio.Rest;

namespace CesarBmx.Notification.Application.Mappers
{
    public class NotificationMapper : Profile
    {
        public NotificationMapper()
        {           
            // Model to Response
            CreateMap<Domain.Models.Notification, Responses.Notification>();

            // Model to Event
            CreateMap<Domain.Models.Notification, NotificationSent>();
            CreateMap<PhoneMessage, NotificationSent>();
            CreateMap<Email, NotificationSent>();

            // Command to model
            CreateMap<SendNotification, Domain.Models.Notification>();
            CreateMap<SendNotification, PhoneMessage>();
            CreateMap<SendNotification, Email>();
        }
    }
}
