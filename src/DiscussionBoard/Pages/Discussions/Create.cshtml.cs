using System.Security.Claims;
using DiscussionBoard.Models;
using DiscussionBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DiscussionBoard.Pages.Discussions
{
    public class CreateModel : PageModel
    {
        private readonly DiscussionThreadService _threadService;

        public CreateModel(DiscussionThreadService threadService)
        {
            _threadService = threadService;
        }

        [BindProperty]
        public ThreadValidationModel Input { get; set; } = new ThreadValidationModel();
        public int ForumId { get; set; }

        public async void OnPostAsync(int forumId)
        {
            DiscussionThread thread = new DiscussionThread
            {
                Title = Input.Title,
                Content = Input.Content,
                ForumId = forumId,
                ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                CreatedAt = DateTime.UtcNow
            };
            try
            {
                await _threadService.AddAsync(thread);
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Error adding thread";
                Console.WriteLine(e);
            }
            RedirectToPage("/Discussions/Details", new { threadId = thread.Id });
        }
    }
}
