﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CesarBmx.Notification.Application.Conflicts;
using CesarBmx.Shared.Application.Exceptions;
using CesarBmx.Shared.Common.Extensions;
using CesarBmx.Shared.Persistence.Extensions;
using CesarBmx.Notification.Application.Requests;
using CesarBmx.Notification.Domain.Expressions;
using CesarBmx.Notification.Application.Messages;
using CesarBmx.Notification.Domain.Builders;
using CesarBmx.Notification.Domain.Models;
using CesarBmx.Notification.Persistence.Contexts;
using CesarBmx.Shared.Application.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CesarBmx.Notification.Application.Services
{
    public class IndicatorService
    {
        private readonly MainDbContext _mainDbContext;
        private readonly ILogger<IndicatorService> _logger;
        private readonly IMapper _mapper;
        private readonly ActivitySource _activitySource;

        public IndicatorService(
            MainDbContext mainDbContext,
            ILogger<IndicatorService> logger,
            IMapper mapper,
            ActivitySource activitySource)
        {
            _mainDbContext = mainDbContext;
            _logger = logger;
            _mapper = mapper;
            _activitySource = activitySource;
        }

        public async Task<List<Responses.Indicator>> GetUserIndicators(string userId)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(GetUserIndicators));

            // Get user
            var user = await _mainDbContext.Users.FindAsync(userId);

            // Check if it exists
            if (user == null) throw new NotFoundException(UserMessage.UserNotFound);

            // Get all indicators
            var indicators = await _mainDbContext.Indicators
                .Include(x=> x.Dependencies)
                .ThenInclude(x=>x.Dependency)
                .Where(IndicatorExpression.Filter(null, userId)).ToListAsync();

            // Response
            var response = _mapper.Map<List<Responses.Indicator>>(indicators);

            // Return
            return response;
        }
        public async Task<Responses.Indicator> GetIndicator(string indicatorId)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(GetIndicator));

            // Get indicator
            var indicator = await _mainDbContext.Indicators
                .Include(x => x.Dependencies)
                .ThenInclude(x => x.Dependency)
                .FirstOrDefaultAsync(x=> x.IndicatorId == indicatorId);

            // Indicator not found
            if (indicator == null) throw new NotFoundException(IndicatorMessage.IndicatorNotFound);

            // Response
            var response = _mapper.Map<Responses.Indicator>(indicator);

            // Return
            return response;
        }
        public async Task<Responses.Indicator> AddIndicator(AddIndicator request)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(AddIndicator));

            // Get user
            var user = await _mainDbContext.Users.FindAsync(request.UserId);

            // User not found
            if (user == null) throw new NotFoundException(UserMessage.UserNotFound);

            // Get indicator
            var indicator = await _mainDbContext.Indicators
                .Include(x => x.Dependencies)
                .FirstOrDefaultAsync(IndicatorExpression.Unique(request.UserId, request.Abbreviation));

            // Throw ConflictException if it exists
            if (indicator != null) throw new ConflictException(new Conflict<AddIndicatorConflict>(AddIndicatorConflict.INDICATOR_ALREADY_EXISTS, IndicatorMessage.IndicatorWithSameIdAlreadyExists));

            // Get dependencies
            var dependencies = await GetIndicators(request.Dependencies);

            // Build dependency level
            var dependencyLevel = IndicatorBuilder.BuildDependencyLevel(dependencies);

            // Build indicator dependencies
            var indicatorDependencies = IndicatorDependencyBuilder.BuildIndicatorDependencies(request.IndicatorId, dependencies);

            // Create
            indicator = new Indicator(
                request.UserId,
                request.Abbreviation,
                request.Name,
                request.Description,
                request.Formula,
                indicatorDependencies,
                dependencyLevel,
                DateTime.UtcNow.StripSeconds());

            // Add
            _mainDbContext.Indicators.Add(indicator);

            // Save
            await _mainDbContext.SaveChangesAsync();            

            // Get indicator
            indicator = await _mainDbContext.Indicators
                .Include(x => x.Dependencies)
                .ThenInclude(x=>x.Dependency)
                .FirstOrDefaultAsync(x=>x.IndicatorId == indicator.IndicatorId);

            // Response
            var response = _mapper.Map<Responses.Indicator>(indicator);

            // Log
            _logger.LogInformation("{@Event}, {@Id}, {@UserId}, {@Request}, {@Response}", "IndicatorAdded", Guid.NewGuid(), request.UserId, request, response);

            // Return
            return response;
        }
        public async Task<Responses.Indicator> UpdateIndicator(UpdateIndicator request)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(UpdateIndicator));

            // Get indicator
            var indicator = await _mainDbContext.Indicators
                .Include(x => x.Dependencies)
                .FirstOrDefaultAsync(x => x.IndicatorId == request.IndicatorId);

            // Indicator not found
            if (indicator == null) throw new NotFoundException(IndicatorMessage.IndicatorNotFound);

            // Get dependencies
            var newDependencies = await GetIndicators(request.Dependencies);

            // Build new indicator dependencies
            var newIndicatorDependencies = IndicatorDependencyBuilder.BuildIndicatorDependencies(indicator.IndicatorId, newDependencies);

            // Get current indicator dependencies 
            var currentIndicatorDependencies = indicator.Dependencies;

            // Update dependencies
            _mainDbContext.UpdateCollection(currentIndicatorDependencies, newIndicatorDependencies);

            // Update indicator
            indicator.Update(request.Name, request.Description, request.Formula);

            // Update
            _mainDbContext.Indicators.Update(indicator);
            
            // Save
            await _mainDbContext.SaveChangesAsync();

            // Get indicator
            indicator = await _mainDbContext.Indicators
                .Include(x => x.Dependencies)
                .ThenInclude(x => x.Dependency)
                .FirstOrDefaultAsync(x => x.IndicatorId == indicator.IndicatorId);

            // Response
            var response = _mapper.Map<Responses.Indicator>(indicator);

            // Log
            _logger.LogInformation("{@Event}, {@Id}, {@UserId}, {@Request}, {@Response}", "IndicatorUpdated", Guid.NewGuid(), request.UserId, request, response);

            // Return
            return response;
        }

        public async Task<List<Indicator>> GetIndicators(List<string> indicatorIds)
        {
            // Start span
            using var span = _activitySource.StartActivity(nameof(GetIndicators));

            var indicators = new List<Indicator>();
            foreach (var indicatorId in indicatorIds)
            {
                // Get indicator
                var indicator = await _mainDbContext.Indicators.FirstOrDefaultAsync(x=> x.IndicatorId == indicatorId);

                // Indicator not found
                if (indicator == null) throw new NotFoundException(string.Format(IndicatorDependencyMessage.IndicatorDependencyNotFound, indicatorId));

                // Detach
                _mainDbContext.Entry(indicator).State = EntityState.Detached;

                // Add
                indicators.Add(indicator);
            }

            // Return
            return indicators;
        }
        public async Task<List<Indicator>> UpdateDependencyLevels()
        {
            // Start watch
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Start span
            using var span = _activitySource.StartActivity(nameof(UpdateDependencyLevels));

            // Get all indicators
            var indicators = await _mainDbContext.Indicators.ToListAsync();

            // Get all indicators dependencies
            var indicatorDependencies = await _mainDbContext.IndicatorDependencies.ToListAsync();

            // Build dependency levels
            IndicatorBuilder.BuildDependencyLevels(indicators, indicatorDependencies);

            // Update
            _mainDbContext.Indicators.UpdateRange(indicators);

            // Save
            await _mainDbContext.SaveChangesAsync();

            // Build max dependency level
            var maxDependencyLevel = IndicatorBuilder.BuildMaxDependencyLevel(indicators);

            // Stop watch
            stopwatch.Stop();

            // Log
            _logger.LogInformation("{@Event}, {@Id}, {@MaxLevel}, {@ExecutionTime}", "DependenciesLevelsUpdated", Guid.NewGuid(), maxDependencyLevel, stopwatch.Elapsed.TotalSeconds);

            // Return
            return indicators;
        }
    }
}
