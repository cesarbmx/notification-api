﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CesarBmx.Notification.Application.Conflicts;
using CesarBmx.Shared.Application.Exceptions;
using CesarBmx.Shared.Common.Extensions;
using CesarBmx.Notification.Application.Requests;
using CesarBmx.Notification.Application.Messages;
using CesarBmx.Notification.Persistence.Contexts;
using CesarBmx.Shared.Application.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using CesarBmx.Notification.Domain.Models;

namespace CesarBmx.Notification.Application.Services
{
    public class UserService
    {
        private readonly MainDbContext _mainDbContext;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        private readonly ActivitySource _activitySource;

        public UserService(
            MainDbContext mainDbContext,
            ILogger<UserService> logger,
            IMapper mapper,
            ActivitySource activitySource)
        {
            _mainDbContext = mainDbContext;
            _logger = logger;
            _mapper = mapper;
            _activitySource = activitySource;
        }

        public async Task<List<Responses.User>> GetUsers()
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(GetUsers));

            // Get all users
            var users = await _mainDbContext.Users.ToListAsync();

            // Response
            var response = _mapper.Map<List<Responses.User>>(users);

            // Return
            return response;
        }
        public async Task<Responses.User> GetUser(string userId)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(GetUser));

            // Get user
            var user = await _mainDbContext.Users.FindAsync(userId);

            // User not found
            if (user == null) throw new NotFoundException(UserMessage.UserNotFound);

            // Response
            var response = _mapper.Map<Responses.User>(user);

            // Return
            return response;
        }
        public async Task<Responses.User> AddUser(AddUser request)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(AddUser));

            // Get user
            var user = await _mainDbContext.Users.FindAsync(request.UserId);

            // Check if it exists
            if (user != null) throw new ConflictException(new Conflict<AddUserConflict>(AddUserConflict.USER_ALREADY_EXISTS, UserMessage.UserAlreadyExists));

            // Time
            var now = DateTime.UtcNow.StripSeconds();

            // Create user
            user = new User(request.UserId, request.PhoneNumber, now);

            // Add user
            _mainDbContext.Users.Add(user);

            // Save
            await _mainDbContext.SaveChangesAsync();

            // Response
            var response = _mapper.Map<Responses.User>(user);

            // Log
            _logger.LogInformation("{@Event}, {@Id}, {@UserId}, {@Request}, {@Response}", "UserAdded", Guid.NewGuid(), request.UserId, request, response);

            // Return
            return response;
        }
    }
}
