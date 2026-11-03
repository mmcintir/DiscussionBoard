using DiscussionBoard.Services;
using Microsoft.AspNetCore.Identity;

namespace DiscussionBoard.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<DiscussionThread> DiscussionThreads { get; set; }
        public List<Post> Posts { get; set; }

        public string? AvatarId { get;
            set
            {
                if (AvatarCatalog.IsValid(value))
                {
                    field = value;
                }
                else
                {
                    field = AvatarCatalog.DefaultId;
                }

            } } = AvatarCatalog.DefaultId;

        public string? DisplayName { get; set;}



    }
}
