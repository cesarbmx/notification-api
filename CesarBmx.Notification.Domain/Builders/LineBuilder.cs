﻿using System;
using System.Collections.Generic;
using System.Linq;
using CesarBmx.Shared.Common.Extensions;
using CesarBmx.Notification.Domain.Expressions;
using CesarBmx.Notification.Domain.Models;
using CesarBmx.Notification.Domain.Types;


namespace CesarBmx.Notification.Domain.Builders
{
    public static class LineBuilder
    {
        public static List<Line> BuildLines(List<Currency> currencies, List<Indicator> indicators, List<Watcher> watchers)
        {
            var time = DateTime.UtcNow.StripSeconds();
            var lines = new List<Line>();
            var stopAt = indicators.Count > 0 ? indicators.Max(x => x.DependencyLevel) : 0;

            // Build lines
            if (stopAt > 0) BuildLines(currencies, indicators, watchers, lines, time, 0, stopAt);

            // Return
            return lines;
        }
        public static void BuildLines(List<Currency> currencies, List<Indicator> indicators, List<Watcher> watchers, List<Line> lines, DateTime time, int dependencyLevel, int stopAt)
        {
            // For each currency
            foreach (var currency in currencies)
            {
                // For each indicator of the given level
                foreach (var indicator in indicators.Where(x => x.DependencyLevel == dependencyLevel))
                {
                    // Get all watchers for this currency indicator pair
                    var filteredWatchers = watchers.Where(WatcherExpression.Filter(null, currency.CurrencyId, indicator.IndicatorId).Compile()).ToList();

                    // Build value and averages
                    var value = IndicatorBuilder.BuildValue(currency, indicator, lines);
                    var averageBuy = BuildWeightedAverageBuy(filteredWatchers);
                    var averageSell = BuildWeightedAverageSell(filteredWatchers);

                    // Create line
                    var line = new Line(
                        time, 
                        indicator.UserId,
                        currency.CurrencyId,
                        indicator.IndicatorId,
                        value,
                        averageBuy,
                        averageSell,
                        currency.Price);

                    // Add line
                    lines.Add(line);
                }
            }

            // Do the same with the next level recursively
            if (dependencyLevel < stopAt) BuildLines(currencies, indicators, watchers, lines, time, dependencyLevel + 1, stopAt);
        }
        public static Period BuildPeriod(DateTime time)
        {
            if (time.Hour == 0 && time.Minute == 0) return Period.ONE_DAY;

            if (time.Minute == 0) return Period.ONE_HOUR;

            if (time.Minute % 15 == 0) return Period.FIFTEEN_MINUTES;

            if (time.Minute % 5 == 0) return Period.FIVE_MINUTES;

            return Period.ONE_MINUTE;
        }

        public static decimal? BuildWeightedAverageBuy(List<Watcher> watchers)
        {
            // Watchers buying
            var watcherWillingToBuy = watchers.Where(WatcherExpression.WatcherBuying()).ToList();

            // Return if no watchers
            if(watcherWillingToBuy.Count == 0) return null;

            // Total quantity
            var totalQuantity = watcherWillingToBuy.Select(x => x.Quantity).Sum();

            // Weights
            var weights = watcherWillingToBuy.Select(x => x.Buy * x.Quantity);

            // Total weight
            var totalWeight = weights.Sum();

            // Weighted average
            var weightedAverage = totalWeight / totalQuantity;

            // Return
            return weightedAverage;
        }

        public static decimal? BuildWeightedAverageSell(List<Watcher> watchers)
        {
            // Watchers selling
            var watcherWillingToSell = watchers.Where(WatcherExpression.WatcherSelling()).ToList();

            // Return if no watchers
            if (watcherWillingToSell.Count == 0) return null;

            // Total quantity
            var totalQuantity = watcherWillingToSell.Select(x => x.Quantity).Sum();

            // Weights
            var weights = watcherWillingToSell.Select(x => x.Sell * x.Quantity);

            // Total weight
            var totalWeight = weights.Sum();

            // Weighted average
            var weightedAverage = totalWeight / totalQuantity;

            // Return
            return weightedAverage;
        }
    }
}
