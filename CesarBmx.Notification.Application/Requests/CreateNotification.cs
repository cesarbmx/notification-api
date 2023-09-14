using System;
using System.ComponentModel.DataAnnotations;


namespace CesarBmx.Notification.Application.Requests
{
    public class CreateNotification
    {
        [Required] public string UserId { get; set; } = null!;
        [Required] public string Text { get; set; } = null;
        [Required] public DateTime? ScheduledFor { get; set; } = null;
    }
}
