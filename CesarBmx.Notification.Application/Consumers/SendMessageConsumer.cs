using CesarBmx.Notification.Persistence.Contexts;
using CesarBmx.Shared.Messaging.Notification.Commands;
using CesarBmx.Shared.Messaging.Notification.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using AutoMapper;
using CesarBmx.Notification.Domain.Builders;
using CesarBmx.Notification.Domain.Models;
using CesarBmx.Notification.Application.Messages;
using CesarBmx.Notification.Application.Services;

namespace CesarBmx.Notification.Application.Consumers
{
    public class SendMessageConsumer : IConsumer<SendMessage>
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<SendMessageConsumer> _logger;
        private readonly ActivitySource _activitySource;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly MessageService _messageService;

        public SendMessageConsumer(
            MainDbContext mainDbContext,
            IMapper mapper,
            ILogger<SendMessageConsumer> logger,
            ActivitySource activitySource,
            IPublishEndpoint publishEndpoint,
            MessageService messageService)
        {
            _mainDbContext = mainDbContext;
            _mapper = mapper;
            _logger = logger;
            _activitySource = activitySource;
            _publishEndpoint = publishEndpoint;
            _messageService = messageService;
        }

        public async Task Consume(ConsumeContext<SendMessage> context)
        {
            _logger.LogInformation("Send message " + context.Message.Text);

            return Task.CompletedTask;

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
                var message = new Message(sendMessage.MessageId, sendMessage.UserId, sendMessage.PhoneNumber, sendMessage.Text);

                // Send telegram
                await _messageService.SendTelegramMessage(message);

                // Event
                var messageSent = _mapper.Map<MessageSent>(message);

                // Publish event
                await _publishEndpoint.Publish(messageSent);

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
