using CesarBmx.Notification.Application.Requests;


namespace CesarBmx.Notification.Application.FakeRequests
{
    public static class FakeCreateEmail
    {
        public static CreateEmail GetFake_1()
        {
            return new CreateEmail
            {
                UserId = "cesarbmx",
                EmailAddress = "cesarbmx@gmail.com",
                Text = "Test message",
                ScheduledFor = null
            };
        }       
    }
}
