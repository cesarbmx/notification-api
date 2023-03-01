﻿using System.ComponentModel.DataAnnotations;


namespace CesarBmx.Notification.Application.Requests
{
    public class AddUser
    {
        [Required] public string UserId { get; set; }
        [Required] public string PhoneNumber { get; set; }
    }
}
