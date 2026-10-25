using DiscussionBoard.Models;
using DiscussionBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiscussionBoard.Pages.Discussions
{
    public class EditModel : PageModel
    {
        private readonly DiscussionThreadService _threadService;

        public EditModel(DiscussionThreadService threadService)
        {
            _threadService = threadService;
        }

        [BindProperty]
        public ThreadValidationModel Input { get; set; } = new ThreadValidationModel();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var thread = await _threadService.GetAsync(id);
            if (thread == null)
            {
                ViewData["Error"] = ("Thread {ThreadId} not found for update", id);
                return Page();
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
                return Page();
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
            return RedirectToPage("/Discussions/Detail", new { threadId = thread.Id });
        }
    }
}
