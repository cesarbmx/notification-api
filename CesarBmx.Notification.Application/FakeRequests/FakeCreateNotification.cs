using CesarBmx.Notification.Application.Requests;


namespace CesarBmx.Notification.Application.FakeRequests
{
    public static class FakeCreateNotification
    {
        public static CreateNotification GetFake_1()
        {
            return new CreateNotification
            {
                UserId = "cesarbmx",
                Text = "Test message",
                ScheduledFor = null
            };
        }       
    }
}
