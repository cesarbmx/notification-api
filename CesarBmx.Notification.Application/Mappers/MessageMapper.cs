using AutoMapper;
using CesarBmx.Notification.Domain.Models;
using CesarBmx.Shared.Messaging.Notification.Commands;
using CesarBmx.Shared.Messaging.Notification.Events;
namespace CesarBmx.Notification.Application.Mappers
{
    public class MessageMapper : Profile
    {
        public MessageMapper()
        {           
            // Model to Response
            CreateMap<Message, Responses.Message>();

            // Model to Event
            CreateMap<Message, MessageSent>();

            // Command to model
            CreateMap<SendMessage, Message>();
        }
    }
}
