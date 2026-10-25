using Microsoft.AspNetCore.Identity;

namespace DiscussionBoard.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<DiscussionThread> DiscussionThreads { get; set; }
        public List<Post> Posts { get; set; }
    }
}
