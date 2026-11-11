using System.Security.Claims;
using DiscussionBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DiscussionBoard.Authorization
{
    public class ApplicationAuthorizationService : IApplicationAuthorizationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public ApplicationAuthorizationService(UserManager<ApplicationUser> userManager,
            IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        public async Task<bool> CanEditAsync(ClaimsPrincipal principal, DiscussionBase resource)
        {
            var result = await _authorizationService.AuthorizeAsync(
                principal, resource, ApplicationAuthorizationPolicies.IsOwnerOrAdmin);
            return result.Succeeded;
        }

        public async Task<IdentityResult> AddRoleAsync(ClaimsPrincipal actingUser, string targetUserId, string roleName)
        {
            if (!await IsAdminAsync(actingUser))
                return IdentityResult.Failed(new IdentityError { Description = "Insufficient privileges." });

            if (roleName != ApplicationRoles.Admin)
                return IdentityResult.Failed(new IdentityError { Description = "Role not supported." });

            var target = await _userManager.FindByIdAsync(targetUserId);
            if (target == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            return await _userManager.AddToRoleAsync(target, roleName);
        }

        public async Task<IdentityResult> RemoveRoleAsync(ClaimsPrincipal actingUser, string targetUserId, string roleName)
        {
            if (!await IsAdminAsync(actingUser))
                return IdentityResult.Failed(new IdentityError { Description = "Insufficient privileges." });

            if (roleName != ApplicationRoles.Admin)
                return IdentityResult.Failed(new IdentityError { Description = "Role not supported." });

            var target = await _userManager.FindByIdAsync(targetUserId);
            if (target == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            if (_userManager.GetUserId(actingUser) == targetUserId)
                return IdentityResult.Failed(new IdentityError { Description = "You cannot remove your own admin permissions." });

            return await _userManager.RemoveFromRoleAsync(target, roleName);
        }

        private async Task<bool> IsAdminAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity?.IsAuthenticated != true)
                return false;

            var user = await _userManager.GetUserAsync(principal);
            return user != null && await _userManager.IsInRoleAsync(user, ApplicationRoles.Admin);
        }
    }
}
