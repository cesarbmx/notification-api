using AutoMapper;
using CesarBmx.Shared.Messaging.Notification.Commands;
using CesarBmx.Shared.Messaging.Notification.Events;

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

            // Command to model
            CreateMap<SendNotification, Domain.Models.Notification>();
        }
    }
}
