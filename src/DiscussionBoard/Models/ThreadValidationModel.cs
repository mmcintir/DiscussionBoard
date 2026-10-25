using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DiscussionBoard.Models
{
    [BindProperties]
    public class ThreadValidationModel
    {
        [MinLength(1)]
        [MaxLength(50)]
        public string Title { get; set; }
        [MinLength(1)]
        [MaxLength(500)]
        public string Content { get; set; }
    }
}
