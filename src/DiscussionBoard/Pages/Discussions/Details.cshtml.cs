using DiscussionBoard.Models;
using DiscussionBoard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DiscussionBoard.Pages.Discussions
{
    public class DetailsModel : PageModel
    {
        private readonly DiscussionThreadService _threadService;
        private readonly PostService _postService;
        private readonly ILogger<DetailsModel> _logger;
        private List<Post> _posts = new List<Post>();

        public DetailsModel(DiscussionThreadService threadService, PostService postService,  ILogger<DetailsModel> logger)
        {
            _threadService = threadService;
            _postService = postService;
            _logger = logger;
        }

        public DiscussionThread Discussion { get; set; }
        public List<PostTreeBuilder.Node> Nodes { get; set; }

        public async Task OnGetAsync(int threadId)
        {
            var thread = await _threadService.GetAsync(threadId);
            if (thread == null)
            {
                ViewData["Error"] = "Thread not found";
                _logger.LogError("Thread not found");
                return;
            }

            Discussion = thread;
            Nodes = PostTreeBuilder.Build(await _postService.GetForThreadAsync(threadId));
        }


    }
}
