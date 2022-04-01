﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UniAPI.Authorization.Policy
{
    public class MinimumAgeRequirmentHandler : AuthorizationHandler<MinimumAgeRequirment>
    {
        private readonly ILogger<MinimumAgeRequirmentHandler> _logger;
        public MinimumAgeRequirmentHandler(ILogger<MinimumAgeRequirmentHandler> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirment requirement)
        {
            var dateOfBirth = DateTime.Parse(context.User.FindFirst(c=>c.Type=="DateOfBirth").Value);

            var userEmail = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;

            _logger.LogInformation($"User: {userEmail} with date of birth [{dateOfBirth}]");

            if (dateOfBirth.AddDays(requirement.MinAge) < DateTime.Today)
            {
                _logger.LogInformation("Authorization succedded");
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogInformation("Authorization failed");

            }

            return Task.CompletedTask;
        }
    }
}
