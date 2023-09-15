using CesarBmx.Notification.Application.FakeRequests;
using CesarBmx.Notification.Application.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace CesarBmx.CryptoWatcher.Api.RequestExamples
{
    public class CreateMessageExample : IExamplesProvider<CreateMessage>
    {
        public CreateMessage GetExamples()
        {
            return FakeCreateMessage.GetFake_1();
        }
    }
}