using System.ComponentModel.DataAnnotations;

namespace TeddySmith.API.DTOs.Comment
{
    public class UpdateCommentDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "Tittle must be 5 chareacters")]
        [MaxLength(280, ErrorMessage = "Title cannot be over 280 chareacters")]
        public string Title { get; set; } = String.Empty;
        [Required]
        [MinLength(5, ErrorMessage = "Content must be 5 chareacters")]
        [MaxLength(280, ErrorMessage = "Content cannot be over 280 chareacters")]
        public string Content { get; set; } = String.Empty;
    }
}
