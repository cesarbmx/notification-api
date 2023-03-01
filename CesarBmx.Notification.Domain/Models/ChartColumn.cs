﻿


namespace CesarBmx.Notification.Domain.Models
{
    public class ChartColumn
    {
        public string Label { get; private set; }
        public string Type { get; private set; }
       

        public ChartColumn() { }
        public ChartColumn(string type, string label)
        {
            Type = type;
            Label = label;
        }
    }
}
