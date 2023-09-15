using System;
using System.Linq.Expressions;

namespace CesarBmx.Notification.Domain.Expressions
{
    public static class MessageExpression
    {
        public static Expression<Func<Models.Message, bool>> Filter(string userId = null)
        {
            return x => string.IsNullOrEmpty(userId) || x.UserId == userId;
        }
        public static Expression<Func<Models.Message, bool>> PendingMessage()
        {
            return x => !x.SentAt.HasValue;
        }
    }
}
