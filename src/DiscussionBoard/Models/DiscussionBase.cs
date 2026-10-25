
using System.ComponentModel.DataAnnotations;

namespace DiscussionBoard.Models
{
    public class DiscussionBase : EntityBase
    {
        [MinLength(1)]
        [MaxLength(50)]
        public string Title { get; set; }
        [MinLength(1)]
        [MaxLength(500)]
        public string Content { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        [MinLength(1)]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
