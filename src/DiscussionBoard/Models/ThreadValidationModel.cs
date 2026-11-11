using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionBoard.Models
{
    [BindProperties]
    public class ThreadValidationModel
    {
        [MinLength(3)]
        [MaxLength(160)]
        public string Title { get; set; }
        [MinLength(1)]
        [MaxLength(10000)]
        public string Content { get; set; }
    }
}
