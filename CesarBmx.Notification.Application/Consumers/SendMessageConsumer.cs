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

namespace CesarBmx.Notification.Application.Consumers
{
    public class SendMessageConsumer : IConsumer<SendMessage>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<SendMessageConsumer> _logger;
        private readonly ActivitySource _activitySource;
        private readonly AppSettings _appSettings;

        public SendMessageConsumer(
            MainDbContext mainDbContext,
            IMapper mapper,
            ILogger<SendMessageConsumer> logger,
            ActivitySource activitySource,
            AppSettings appSettings)
        {
            _mainDbContext = mainDbContext;
            _mapper = mapper;
            _logger = logger;
            _activitySource = activitySource;
            _appSettings = appSettings;
        }

        public async Task Consume(ConsumeContext<SendMessage> context)
        {
            try
            {
                // Start watch
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // Start span
                using var span = _activitySource.StartActivity(nameof(SendMessage));

                // Command
                var sendMessage = context.Message;

                // Create message
                var message = new Message(sendMessage.MessageId, sendMessage.UserId, "TODO: Look it up in notification", sendMessage.Text);

                // Connect
                var apiToken = _appSettings.TelegramApiToken;
                var bot = new TelegramBotClient(apiToken);

                // Send telegram
                await bot.SendTextMessageAsync("@crypto_watcher_official", message.Text);

                // Mark notification as sent
                message.MarkAsSent();

                // Update notification
                _mainDbContext.Messages.Update(message);

                // Event
                var messageSent = _mapper.Map<MessageSent>(message);

                // Publish event
                await context.Publish(messageSent);

                // Save
                await _mainDbContext.SaveChangesAsync();

                // Response
                await context.RespondAsync(messageSent);

                // Stop watch
                stopwatch.Stop();

                // Log
                _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", nameof(MessageSent), Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);
            }
            catch (Exception ex)
            {
                // Log
                _logger.LogError(ex, ex.Message);
            }
        }
    }

}
