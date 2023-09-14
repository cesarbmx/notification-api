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
    public class NotificationService
    {
        private readonly MainDbContext _mainDbContext;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly ILogger<NotificationService> _logger;
        private readonly ActivitySource _activitySource;

        public NotificationService(
            MainDbContext mainDbContext,
            IMapper mapper,
            AppSettings appSettings,
            ILogger<NotificationService> logger,
            ActivitySource activitySource)
        {
            _mainDbContext = mainDbContext;
            _mapper = mapper;
            _appSettings = appSettings;
            _logger = logger;
            _activitySource = activitySource;
        }

        public async Task<List<Responses.Notification>> GetNotifications(string userId)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(GetNotifications));

            // Get user
            var notifications = await _mainDbContext.Notifications
                .Where(x => x.UserId == userId).ToListAsync();

            // Response
            var response = _mapper.Map<List<Responses.Notification>>(notifications);

            // Return
            return response;
        }
        public async Task<Responses.Notification> GetNotification(Guid messageId)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(GetNotification));

            // Get notification
            var notification = await _mainDbContext.Notifications.FindAsync(messageId);

            // Throw NotFound if the currency does not exist
            if (notification == null) throw new NotFoundException(NotificationMessage.NotificationNotFound);

            // Response
            var response = _mapper.Map<Responses.Notification>(notification);

            // Return
            return response;
        }
        public async Task<Responses.Notification> CreateNotification(CreateNotification request)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(GetNotification));

            // The destinations should come from user settings

            // Create notification
            var phoneMessage = new PhoneMessage(
                Guid.NewGuid(),
                request.UserId,
                NotificationType.PHONE_MESSAGE,
                "+34666333222",
                PhoneApp.TELEGRAM,
                request.Text,
                request.ScheduledFor
                );


            // Response
            var response = _mapper.Map<Responses.Notification>(phoneMessage);

            // Return
            return response;
        }

        public async Task SendPendingNotifications()
        {
            // Start watch
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Start span
            using var span = _activitySource.StartActivity(nameof(SendPendingNotifications));

            // Get pending notifications
            var pendingNotifications = await _mainDbContext.Notifications.Where(NotificationExpression.PendingNotification()).ToListAsync();

            // Connect
            var apiToken = _appSettings.TelegramApiToken;
            var bot = new TelegramBotClient(apiToken);

            // For each pending notification
            var count = 0;
            foreach (var pendingNotification in pendingNotifications)
            {
                switch (pendingNotification.NotificationType)
                {
                    case NotificationType.PHONE_MESSAGE:
                        var phoneMessage = pendingNotification as PhoneMessage;
                        await SendTelegramNotification(phoneMessage);
                        switch (phoneMessage.DestinationApp)
                        {
                            case PhoneApp.TELEGRAM:
                                await SendTelegramNotification(phoneMessage);
                                break;
                            case PhoneApp.WHATSAPP:
                                await SendWhatsappNotification(phoneMessage);
                                break;
                            default:
                                throw new NotImplementedException(nameof(phoneMessage.DestinationApp));
                        }
                        count++;
                        break;
                    case NotificationType.EMAIL:
                        throw new NotImplementedException(nameof(pendingNotification.NotificationType));
                        count++;
                        break;
                    default:
                        throw new NotImplementedException(nameof(pendingNotification.NotificationType));
                }


            }

            // Stop watch
            stopwatch.Stop();

            // Log
            _logger.LogInformation("{@Event}, {@Id}, {@Count}, {@ExecutionTime}", "PendingNotificationsSent", Guid.NewGuid(), count, stopwatch.Elapsed.TotalSeconds);
        }
        public async Task SendTelegramNotification(PhoneMessage phoneMessage)
        {
            // Start watch
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Start span
            using var span = _activitySource.StartActivity(nameof(SendTelegramNotification));

            // Connect
            var apiToken = _appSettings.TelegramApiToken;
            var bot = new TelegramBotClient(apiToken);

            try
            {

                // Send telegram
                await bot.SendTextMessageAsync("@crypto_watcher_official", phoneMessage.Text);

                // Mark notification as sent
                phoneMessage.MarkAsSent();

                // Update notification
                _mainDbContext.Notifications.Update(phoneMessage);

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
            _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", "TelegramNotificationsSent", Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);
        }
        public async Task SendWhatsappNotification(PhoneMessage phoneMessage)
        {
            // Start watch
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Start span
            using var span = _activitySource.StartActivity(nameof(SendNotification));

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
                    to: new PhoneNumber("whatsapp:" + phoneMessage.PhoneNumber),
                    body: phoneMessage.Text
                );
                phoneMessage.MarkAsSent();
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
            _logger.LogInformation("{@Event}, {@Id}, {@ExecutionTime}", "WhatsappNotificationsSent", Guid.NewGuid(), stopwatch.Elapsed.TotalSeconds);
        }
    }
}
