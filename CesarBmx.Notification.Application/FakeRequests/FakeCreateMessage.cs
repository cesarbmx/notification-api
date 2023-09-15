using CesarBmx.Notification.Application.Requests;


namespace CesarBmx.Notification.Application.FakeRequests
{
    public static class FakeCreateMessage
    {
        public static CreateMessage GetFake_1()
        {
            return new CreateMessage
            {
                UserId = "cesarbmx",
                Text = "Test message",
                ScheduledFor = null
            };
        }       
    }
}
