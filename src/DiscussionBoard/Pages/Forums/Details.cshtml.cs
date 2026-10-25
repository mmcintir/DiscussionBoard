using Microsoft.AspNetCore.Mvc.RazorPages;
using DiscussionBoard.Services;
using DiscussionBoard.Models;
namespace DiscussionBoard.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly DiscussionThreadService _threadService;
        public List<DiscussionThread> Items { get; private set; } = [];

        public DetailsModel(DiscussionThreadService threadService)
        {
            _threadService = threadService;
        }

        public async Task OnGetAsync(int id)
        {
            try
            {
                Items = await _threadService.GetForForumAsync(id);
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ("Error getting forum: {id}", id);
                Console.WriteLine(ex.Message);
            }
        }
    }
}
