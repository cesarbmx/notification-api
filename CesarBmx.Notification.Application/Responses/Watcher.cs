﻿using System;
using CesarBmx.Notification.Domain.Types;


namespace CesarBmx.Notification.Application.Responses
{
    public class Watcher
    {
        public int WatcherId { get; set; }
        public string UserId { get; set; }
        public string CurrencyId { get; set; }
        public string IndicatorUserId { get; set; }
        public string IndicatorId { get; set; }
        public decimal Value { get; set; }
        public decimal? Buy { get; set; }
        public decimal? Sell { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? AverageBuy { get; set; }
        public decimal? AverageSell { get; set; }
        public decimal? Price { get; set; }
        public DateTime? EntryAt { get; set; }
        public decimal? EntryPrice { get; set; }
        public DateTime? ExitAt { get; set; }
        public decimal? ExitPrice { get; set; }
        public decimal? Profit { get; set; }
        public WatcherStatus Status { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
