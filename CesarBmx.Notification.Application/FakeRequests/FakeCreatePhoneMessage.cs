using CesarBmx.Notification.Application.Requests;


namespace CesarBmx.Notification.Application.FakeRequests
{
    public static class FakeCreatePhoneMessage
    {
        public static CreatePhoneMessage GetFake_1()
        {
            return new CreatePhoneMessage
            {
                UserId = "cesarbmx",
                PhoneNumber = "+34666555555",
                Text = "Test message",
                ScheduledFor = null
            };
        }       
    }
}
