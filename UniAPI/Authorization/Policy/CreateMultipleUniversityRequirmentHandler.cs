using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniAPI.Entites;

namespace UniAPI.Authorization.Policy
{
    public class CreateMultipleUniversityRequirmentHandler : AuthorizationHandler<CreateMultipleUniversityRequirment>
    {

        private readonly UniversityDbContext _dbContext;
        public CreateMultipleUniversityRequirmentHandler(UniversityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreateMultipleUniversityRequirment requirement)
        {
            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var createdUni = _dbContext.Universities.Count(r => r.CreateById == userId);


            if(createdUni >= requirement.MinimumCreatedUni)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
