using CesarBmx.Notification.Persistence.Contexts;
using CesarBmx.Shared.Messaging.Notification.Commands;
using CesarBmx.Shared.Messaging.Notification.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using CesarBmx.Notification.Domain.Models;
using Telegram.Bot;
using CesarBmx.Notification.Application.Settings;
using AutoMapper;
using CesarBmx.Shared.Messaging.Ordering.Events;

namespace CesarBmx.Notification.Application.Consumers
{
    public class SendNotificationConsumer : IConsumer<SendNotification>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<SendNotificationConsumer> _logger;
        private readonly ActivitySource _activitySource;
        private readonly AppSettings _appSettings;

        public SendNotificationConsumer(
            MainDbContext mainDbContext,
            IMapper mapper,
            ILogger<SendNotificationConsumer> logger,
            ActivitySource activitySource,
            AppSettings appSettings)
        {
            _mainDbContext = mainDbContext;
            _mapper = mapper;
            _logger = logger;
            _activitySource = activitySource;
            _appSettings = appSettings;
        }

        public async Task Consume(ConsumeContext<SendNotification> context)
        {
            try
            {
                // Start watch
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // Start span
                using var span = _activitySource.StartActivity(nameof(SendNotification));

                // Command
                var sendNotification = context.Message;

                // Create message
                var phoneMessage = _mapper.Map<PhoneMessage>(sendNotification);

                // Set destination app
                phoneMessage.SetDestinationApp(Domain.Types.PhoneApp.TELEGRAM, "+34666333222");

                // Connect
                var apiToken = _appSettings.TelegramApiToken;
                var bot = new TelegramBotClient(apiToken);

                // Send telegram
                await bot.SendTextMessageAsync("@crypto_watcher_official", phoneMessage.Text);

                // Mark notification as sent
                phoneMessage.MarkAsSent();

                // Update notification
                _mainDbContext.PhoneMessages.Update(phoneMessage);

                // Save
                await _mainDbContext.SaveChangesAsync();

                // Stop watch
                stopwatch.Stop();

                // Log
                _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", nameof(NotificationSent), Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);

                // Event
                var messageSent = _mapper.Map<NotificationSent>(phoneMessage);

                // Publish event
                await context.Publish(messageSent);

                // Response
                await context.RespondAsync(messageSent);
            }
            catch (Exception ex)
            {
                // Log
                _logger.LogError(ex, ex.Message);
            }
        }
    }

}
