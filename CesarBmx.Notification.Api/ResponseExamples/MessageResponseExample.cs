using CesarBmx.Notification.Application.FakeResponses;
using CesarBmx.Notification.Application.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace CesarBmx.Notification.Api.ResponseExamples
{
    public class MessageResponseExample : IExamplesProvider<Application.Responses.Message>
    {
        public Application.Responses.Message GetExamples()
        {
            return FakeMessage.GetFake_Master();
        }
    }
    public class NotificationListResponseExample : IExamplesProvider<List<Application.Responses.Message>>
    {
        public List<Application.Responses.Message> GetExamples()
        {
            return FakeMessage.GetFake_List();
        }
    }
}