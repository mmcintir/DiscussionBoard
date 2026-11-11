using DiscussionBoard.Authorization;
using DiscussionBoard.Models;
using DiscussionBoard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiscussionBoard.Pages.Discussions
{
    [Authorize]
    public class ThreadEditModel : PageModel
    {
        private readonly DiscussionThreadService _threadService;
        private readonly IApplicationAuthorizationService _authorizationService;

        public ThreadEditModel(DiscussionThreadService threadService,  IApplicationAuthorizationService authorizationService)
        {
            _threadService = threadService;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public DiscussionEditViewModel Input { get; set; } = new DiscussionEditViewModel();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var thread = await _threadService.GetAsync(id);

            if (thread == null)
            {
                ViewData["Error"] = ("Thread {ThreadId} not found for update", id);
                return NotFound();
            }


            if (!await _authorizationService.CanEditAsync(User, thread))
            {
                return Forbid();
            }

            Input.Title = thread.Title;
            Input.Content = thread.Content;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var thread = await _threadService.GetAsync(id);

            if (thread == null)
            {
                ViewData["Error"] = ("Thread {ThreadId} not found for update", id);
                return NotFound();
            }

            if (!await _authorizationService.CanEditAsync(User, thread))
            {
                return Forbid();
            }


            thread.Title = Input.Title;
            thread.Content = Input.Content;
            try
            {
                await _threadService.UpdateAsync(thread);
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Error updating thread";
                Console.WriteLine(e);
            }
            return RedirectToPage("/Discussions/Details", new { threadId = thread.Id });
        }
    }
}
