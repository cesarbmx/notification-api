﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CesarBmx.Shared.Authentication.Attributes;
using Newtonsoft.Json;


namespace CesarBmx.Notification.Application.Requests
{
    public class AddIndicator
    {
        [JsonIgnore] public string IndicatorId => UserId + "." + Abbreviation;
        [JsonIgnore] [Identity] public string UserId { get; set; }
        [Required] public string Abbreviation { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string Formula { get; set; }
        [Required] public List<string> Dependencies { get; set; }
    }
}
