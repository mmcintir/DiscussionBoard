using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace DiscussionBoard.Models
{
    public class DiscussionThread : DiscussionBase
    {
        public Forum Forum { get; set; }
        public int ForumId { get; set; }
        public List<Post> Posts { get; set; }
    }
}
