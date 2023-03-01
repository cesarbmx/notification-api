﻿using System;
using CesarBmx.Shared.Domain.Models;


namespace CesarBmx.Notification.Domain.Models
{
    public class IndicatorDependency: IEntity<IndicatorDependency>
    {
        public string Id =>  IndicatorId + "_" + DependencyId ;

        public string IndicatorId { get; private set; }
        public string DependencyId { get; private set; }
        public DateTime Time { get; private set; }

        public Indicator Dependency { get; private set; }

        public IndicatorDependency() { }

        public IndicatorDependency(
            string indicatorId,
            string dependencyId,
            DateTime time)
        {
            IndicatorId = indicatorId;
            DependencyId = dependencyId;
            Time = time;
            Dependency = null;
        }

        public IndicatorDependency Update(IndicatorDependency indicatorDependency)
        {
            DependencyId = indicatorDependency.DependencyId;
            Time = indicatorDependency.Time;

            return this;
        }
    }
}
