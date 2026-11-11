using DiscussionBoard.Models;
using DiscussionBoard.Services;
using DiscussionBoard.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiscussionBoard.Pages
{
    [Authorize]
    public class MyProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserProfileService _userProfileService;

        [BindProperty]
        public UserProfile Input { get; set; }

        public MyProfileModel(UserManager<ApplicationUser> userManager,  IUserProfileService userProfileService)
        {
            _userManager = userManager;
            _userProfileService = userProfileService;
        }

        public async Task OnGetAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User) ?? new ApplicationUser();

            Input = new UserProfile
            {
                DisplayName = user.DisplayName ?? "",
                AvatarId = user.AvatarId ?? AvatarCatalog.DefaultId
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User) ?? new ApplicationUser();

            Input.DisplayName = Input.DisplayName?.Trim() ?? "";

            if (Input.DisplayName.Length < 2 || Input.DisplayName.Length > 60)
            {
                ModelState.AddModelError("Input.DisplayName", "Display name must be 2–60 characters.");
            }

            if (!AvatarCatalog.IsValid(Input.AvatarId))
            {
                ModelState.AddModelError("Input.AvatarId", "Please choose a valid avatar.");
            }

            if (!await _userProfileService.IsDisplayNameAvailableAsync(Input.DisplayName, user.Id))
            {
                ModelState.AddModelError("Input.DisplayName", "That display name is already taken.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            user.DisplayName = Input.DisplayName;
            user.AvatarId = Input.AvatarId;

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Input.DisplayName", error.Description);
                }
            }

            ViewData["UpdateStatus"] = "Successfully Updated User Profile";
            return Page();

        }
    }
}
