﻿using System.ComponentModel.DataAnnotations;
using CesarBmx.Shared.Authentication.Attributes;
using Newtonsoft.Json;

namespace CesarBmx.Notification.Application.Requests
{
    public class EnableWatcher
    {
        [JsonIgnore] [Identity] public string UserId { get; set; }
        [JsonIgnore] public int  WatcherId { get; set; }
        [Required] public bool Enabled { get; set; }
    }
}
