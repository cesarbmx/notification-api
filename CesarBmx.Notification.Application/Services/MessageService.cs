using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CesarBmx.Shared.Application.Exceptions;
using CesarBmx.Notification.Domain.Expressions;
using CesarBmx.Notification.Application.Messages;
using CesarBmx.Notification.Application.Settings;
using CesarBmx.Notification.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using CesarBmx.Shared.Messaging.Notification.Commands;
using CesarBmx.Notification.Domain.Models;
using CesarBmx.Notification.Domain.Types;
using CesarBmx.Notification.Application.Requests;

namespace CesarBmx.Notification.Application.Services
{
    public class MessageService
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly ILogger<MessageService> _logger;
        private readonly ActivitySource _activitySource;

        public MessageService(
            MainDbContext mainDbContext,
            IMapper mapper,
            AppSettings appSettings,
            ILogger<MessageService> logger,
            ActivitySource activitySource)
        {
            _mainDbContext = mainDbContext;
            _mapper = mapper;
            _appSettings = appSettings;
            _logger = logger;
            _activitySource = activitySource;
        }

        public async Task<List<Responses.Message>> GetMessages(string userId)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(GetMessages));

            // Get messages
            var messages = await _mainDbContext.Messages
                .Where(x => x.UserId == userId).ToListAsync();

            // Response
            var response = _mapper.Map<List<Responses.Message>>(messages);

            // Return
            return response;
        }
        public async Task<Responses.Message> GetMessage(Guid messageId)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(GetMessage));

            // Get notification
            var notification = await _mainDbContext.Messages.FindAsync(messageId);

            // Throw NotFound if the currency does not exist
            if (notification == null) throw new NotFoundException(MessageMessage.NotificationNotFound);

            // Response
            var response = _mapper.Map<Responses.Message>(notification);

            // Return
            return response;
        }
        public async Task<Responses.Message> CreateMessage(CreateMessage request)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(GetMessage));

            // The destinations should come from user settings

            // Create message
            var message = new Message(
                Guid.NewGuid(),
                request.UserId,
                telegram: true,
                whatsapp: false,
                email:false,
                DeliveryType.DIRECT,
                "+34666333222",
                "kkk@gmail.com",
                request.Text,
                request.ScheduledFor
                );

            // Add
            await _mainDbContext.Messages.AddAsync(message);

            // Save changes
            await _mainDbContext.SaveChangesAsync();

            // Response
            var response = _mapper.Map<Responses.Message>(message);

            // Return
            return response;
        }

        public async Task SendPendingMessages()
        {
            // Start watch
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Start span
            using var span = _activitySource.StartActivity(nameof(SendPendingMessages));

            // Get pending messages
            var pendingMessages= await _mainDbContext.Messages.Where(MessageExpression.PendingMessage()).ToListAsync();

            // Connect
            var apiToken = _appSettings.TelegramApiToken;
            var bot = new TelegramBotClient(apiToken);

            // For each pending notification
            var count = 0;
            foreach (var pendingMessage in pendingMessages)
            {

                // Telegram
                if (pendingMessage.Telegram)
                {
                    await SendTelegramMessage(pendingMessage);
                }

                // Whatsapp
                if (pendingMessage.Telegram)
                {
                    await SendWhatsappMessage(pendingMessage);
                }

                // Email
                if (pendingMessage.Email)
                {
                    throw new NotImplementedException();
                }

                // Count
                count++;             
            }

            // Stop watch
            stopwatch.Stop();

            // Log
            _logger.LogInformation("{@Event}, {@Id}, {@Count}, {@ExecutionTime}", "MessageSent", Guid.NewGuid(), count, stopwatch.Elapsed.TotalSeconds);
        }
        public async Task SendTelegramMessage(Message message)
        {
            // Start watch
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Start span
            using var span = _activitySource.StartActivity(nameof(SendTelegramMessage));

            // Connect
            var apiToken = _appSettings.TelegramApiToken;
            var bot = new TelegramBotClient(apiToken);

            try
            {
                // Send telegram
                await bot.SendTextMessageAsync("@crypto_watcher_official", message.Text);

                // Mark message as sent
                message.MarkAsSent();

                // Update message
                _mainDbContext.Messages.Update(message);

                // Save
                await _mainDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log
                _logger.LogError(ex, ex.Message);
            }

            // Stop watch
            stopwatch.Stop();

            // Log
            _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", "TelegramMessageSent", Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);
        }
        public async Task SendWhatsappMessage(Message message)
        {
            // Start watch
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Start span
            using var span = _activitySource.StartActivity(nameof(SendMessage));

            // Connect
            TwilioClient.Init(
                Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID"),
                Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN")
            );

            try
            {
                // Send whatsapp
                MessageResource.Create(
                    from: new PhoneNumber("whatsapp:" + "+34666666666"),
                    to: new PhoneNumber("whatsapp:" + message.PhoneNumber),
                    body: message.Text
                );

                // Mark message as sent
                message.MarkAsSent();
            }
            catch (Exception ex)
            {
                // Log
                _logger.LogError(ex, ex.Message);
            }

            // Save
            await _mainDbContext.SaveChangesAsync();

            // Stop watch
            stopwatch.Stop();

            // Log
            _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", "WhatsappMessageSent", Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);
        }
    }
}
