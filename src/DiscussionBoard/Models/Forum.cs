using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DiscussionBoard.Models
{
    public class Forum: EntityBase
    {
        [MinLength(1)]
        [MaxLength(50)]
        public string Name { get; set; }
        [MinLength(1)]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        public List<DiscussionThread> DiscussionThreads { get; set; } = [];
    }
}
