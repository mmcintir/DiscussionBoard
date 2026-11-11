using System.Security.Claims;
using DiscussionBoard.Models;
using Microsoft.AspNetCore.Authorization;

namespace DiscussionBoard.Authorization
{
    public class IsOwnerOrAdminHandler : AuthorizationHandler<IsOwnerOrAdminRequirement,
        DiscussionBase>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsOwnerOrAdminRequirement requirement, DiscussionBase resource)
        {
            try
            {
                if (!context.User.Identity.IsAuthenticated)
                {
                    return Task.CompletedTask;
                }
            }
            catch (Exception e)
            {
                return Task.CompletedTask;
            }

            if(context.User.IsInRole(ApplicationRoles.Admin))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            string userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId) && userId.Equals(resource.ApplicationUserId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
