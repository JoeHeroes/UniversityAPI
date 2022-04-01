using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UniAPI.Entites;

namespace UniAPI.Authorization.Policy
{

    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement,University>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, University university)
        {
            if(requirement.ResourcOperation == ResourcOperation.Read || 
               requirement.ResourcOperation == ResourcOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId =  context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (university.CreateById == int.Parse(userId))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
