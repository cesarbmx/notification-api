using CesarBmx.Notification.Application.FakeResponses;
using CesarBmx.Notification.Application.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace CesarBmx.Notification.Api.ResponseExamples
{
    public class MessageResponseExample : IExamplesProvider<Application.Responses.Notification>
    {
        public Application.Responses.Notification GetExamples()
        {
            return FakeNotification.GetFake_Master();
        }
    }
    public class NotificationListResponseExample : IExamplesProvider<List<Application.Responses.Notification>>
    {
        public List<Application.Responses.Notification> GetExamples()
        {
            return FakeNotification.GetFake_List();
        }
    }
}