using System;
using System.Collections.Generic;
using CesarBmx.Shared.Common.Extensions;
using CesarBmx.Notification.Application.Responses;

namespace CesarBmx.Notification.Application.FakeResponses
{
    public static class FakeNotification
    {
        public static Responses.Notification GetFake_Master()
        {
            return new Responses.Notification
            {
                MessageId = Guid.NewGuid(),
                UserId = "master",
                Text = "Test message",
                PhoneNumber = "+34666555555",
                Time = DateTime.UtcNow.StripSeconds(),
                SentTime = null
            };
        }
        public static Responses.Notification GetFake_CesarBmx()
        {
            return new Responses.Notification
            {
                MessageId = Guid.NewGuid(),
                UserId = "cesarbmx",
                Text = "Test message",
                PhoneNumber = "+34666666666",
                Time = DateTime.UtcNow.StripSeconds(),
                SentTime = null
            };
        }
        public static List<Responses.Notification> GetFake_List()
        {
            return new List<Responses.Notification>
            {
                GetFake_Master(),
                GetFake_CesarBmx()
            };
        }
    }
}
