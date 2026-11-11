using DiscussionBoard.Authorization;
using DiscussionBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DiscussionBoard.Pages.Admin
{
    [Authorize(Roles = ApplicationRoles.Admin)]
    public class UsersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationAuthorizationService _authorizationService;

        public UsersModel(UserManager<ApplicationUser> userManager,
            IApplicationAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        public ApplicationUser? SelectedUser { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public bool IsAdmin { get; set; }

        [TempData]
        public string? Message { get; set; }

        public async Task OnGetAsync()
        {
            await LoadUsersAsync();
            if (!string.IsNullOrWhiteSpace(SelectedUserId))
            {
                await GetUser(SelectedUserId);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadUsersAsync();

            if (string.IsNullOrEmpty(SelectedUserId))
            {
                Message = "Please select a user.";
                return Page();
            }

            if (!await GetUser(SelectedUserId))
            {
                Message = "That user could not be found.";
                SelectedUserId = "";
            }

            return Page();
        }

        [BindProperty(SupportsGet = true)]
        public string? SelectedUserId { get; set; }
        [BindProperty]
        public string? Role { get; set; }
        [BindProperty]
        public string? Operation { get; set; }

        public async Task<IActionResult> OnPostAddAsync()
        {
            Operation = Operations.Add;
            Role = ApplicationRoles.Admin;
            return await OnPostUpdateRoleAsync();
        }

        public async Task<IActionResult> OnPostRemoveAsync()
        {
            Operation = Operations.Remove;
            Role = ApplicationRoles.Admin;
            return await OnPostUpdateRoleAsync();
        }


        public async Task<IActionResult> OnPostUpdateRoleAsync()
        {
            await LoadUsersAsync();
            if (string.IsNullOrEmpty(SelectedUserId))
            {
                Message = "Please select a valid user.";
                SelectedUserId = "";
                return Page();
            }

            if (Role != ApplicationRoles.Admin)
            {
                Message = "Invalid role";
                await GetUser(SelectedUserId);
                return Page();
            }

            IdentityResult result;
            if (Operation == "Add")
            {
                result = await _authorizationService.AddRoleAsync(User, SelectedUserId, Role);
            }
            else if (Operation == "Remove")
            {
                result = await _authorizationService.RemoveRoleAsync(User, SelectedUserId, Role);
            }
            else
            {
                Message = "Unknown operation.";
                await GetUser(SelectedUserId);
                return Page();
            }

            if (result.Succeeded)
            {
                Message = "User updated";
            }
            else
            {
                Message = "Error updating user permissions";
            }

            return RedirectToPage();
        }

        public async Task LoadUsersAsync()
        {
            Users = await _userManager.Users.OrderBy(u => u.Email).ToListAsync();
        }
        public async Task<bool> GetUser(string userId)
        {
            SelectedUser = await _userManager.FindByIdAsync(userId);
            if (SelectedUser == null)
            {
                return false;
            }

            IsAdmin = await  _userManager.IsInRoleAsync(SelectedUser, ApplicationRoles.Admin);
            return true;
        }
    }
}
