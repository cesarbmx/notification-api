using System;
using System.Collections.Generic;
using CesarBmx.Shared.Common.Extensions;

namespace CesarBmx.Notification.Application.FakeResponses
{
    public static class FakeMessage
    {
        public static Responses.Message GetFake_Master()
        {
            return new Responses.Message
            {
                MessageId = Guid.NewGuid(),
                UserId = "master",
                Telegram = true,
                Whatsapp = true,
                Email = true,
                DeliveryType= Domain.Types.DeliveryType.DIRECT,
                MessageStatus = Domain.Types.MessageStatus.PENDING,
                PhoneNumber = "+34666555555",
                Text = "Test message",
                CreatedAt = DateTime.UtcNow.StripSeconds(),
                ScheduledFor = null,
                SentTime = null
            };
        }
        public static Responses.Message GetFake_CesarBmx()
        {
            return new Responses.Message
            {
                MessageId = Guid.NewGuid(),
                UserId = "cesarbmx",
                Telegram = true,
                Whatsapp = true,
                Email = true,
                DeliveryType = Domain.Types.DeliveryType.DIRECT,
                MessageStatus = Domain.Types.MessageStatus.PENDING,
                PhoneNumber = "+34666555555",
                Text = "Test message",
                CreatedAt = DateTime.UtcNow.StripSeconds(),
                ScheduledFor = null,
                SentTime = null
            };
        }
        public static List<Responses.Message> GetFake_List()
        {
            return new List<Responses.Message>
            {
                GetFake_Master(),
                GetFake_CesarBmx()
            };
        }
    }
}
