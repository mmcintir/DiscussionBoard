using System.ComponentModel.DataAnnotations.Schema;

namespace DiscussionBoard.Models
{
    public class Post : DiscussionBase
    {
        public DiscussionThread DiscussionThread  { get; set; }
        public int DiscussionThreadId { get; set; }
        public List<Post> Replies { get; set; }
        public Post? ParentPost { get; set; }
        public int? ParentPostId { get; set; }
    }
}
