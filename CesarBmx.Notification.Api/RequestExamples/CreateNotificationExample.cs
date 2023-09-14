using CesarBmx.Notification.Application.FakeRequests;
using CesarBmx.Notification.Application.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace CesarBmx.CryptoWatcher.Api.RequestExamples
{
    public class CreateNotificationExample : IExamplesProvider<CreateNotification>
    {
        public CreateNotification GetExamples()
        {
            return FakeCreateNotification.GetFake_1();
        }
    }
}